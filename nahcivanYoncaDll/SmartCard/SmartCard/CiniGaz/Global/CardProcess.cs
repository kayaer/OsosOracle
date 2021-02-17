using SCLibWin;
using System;
using System.Collections;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace SmartCard.CiniGaz.Global
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    internal class CardProcess
    {
        //CinigazSmartCard.WebServiceSys.WebServiceSoapClient webServiceSys;

        public string zone = "CG";

        Abone abone;
        Yetki yetki;
        Uretim uretim;

        public Enums.CardType cardType;

        public byte[] MasterKey = { 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23 };
        public byte[] DKAbone = { 0x12, 0x53, 0x83, 0x26, 0x26, 0x61, 0x15, 0x98, 0x42, 0x11, 0x45, 0x76, 0x13, 0x62, 0x77, 0x19 };
        public byte[] PinYetki = { 0x34, 0x61, 0x29, 0x56, 0x25, 0x07, 0x13, 0x68 };
        public byte[] PinUretim = { 0x01, 0x01, 0x48, 0x29, 0x53, 0x72, 0x49, 0x06 };
        public byte[] PinAbone = { 0x01, 0x01, 0x48, 0x29, 0x53, 0x72, 0x49, 0x06 };
        public string DFName = "NN";
        public string EFIssuerName = "EI";
        public string EFDataName1 = "E1";
        public string EFDataName2 = "E2";
        public string EFDataName3 = "E3";
        public string EFDataName4 = "E4";

        public byte[] buffer = new byte[100];
        public byte[] inBuf = new byte[256];
        public byte[] outBuf = new byte[256];
        public byte refLength = 0;

        public byte[] csc0 = new byte[4];
        public byte[] csc1 = new byte[4];
        public byte[] csc2 = new byte[4];

        public SCResMgr mng;
        public SCReader rd;
        public string rdName;

        public bool sysLock = false;
        public int sysCount = 0;
        public string macId = "";
        public string macName = "";

        public CardProcess()
        {
            abone = new Abone(this);

            //BasicHttpBinding binding = new BasicHttpBinding();
            //EndpointAddress address = new EndpointAddress("http://yonca.elektromed.com.tr/Services/WebService.asmx");

            //webServiceSys = new CinigazSmartCard.WebServiceSys.WebServiceSoapClient(binding, address);
        }

        #region GLOBAL
        private bool CheckNetwork()
        {
            bool result = false;

            if (!sysLock)
            {
                try
                {
                    TcpClient tcpClient = new System.Net.Sockets.TcpClient("www.google.com.tr", 80);
                    tcpClient.Close();
                    result = true;

                    if (result)
                    {
                        string addr = "";

                        foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
                        {
                            addr += n.GetPhysicalAddress().ToString();

                            break;
                        }

                        macId = addr;
                        macName = Environment.MachineName;

                        //result = webServiceSys.DurumSorgula(macId, macName);
                    }
                }
                catch (Exception e)
                {
                    result = false;
                }
            }

            return result;
        }

        private void SendLog(UInt32 devNo, Int32 anaKredi, Enums.ProcessType processType, Enums.ResultSC resultSC)
        {
            //if (!webServiceSys.LogEkle(Convert.ToInt32(devNo), anaKredi.ToString(), processType.ToString(), resultSC.ToString(), macId))
            //{
            //    if (++sysCount > 2)
            //        sysLock = true;
            //}
            //else
            //    sysCount = 0;
        }

        public void Sleep(int time)
        {
            System.Threading.Thread.Sleep(time);
        }

        public int InitCard()
        {
            try
            {
                mng = new SCResMgr();
                mng.EstablishContext(SCardContextScope.System);
                rd = new SCReader(mng);
                ArrayList al = new ArrayList();
                mng.ListReaders(al);

                if (al.Count > 0)
                {
                    foreach (string st in al)
                    {
                        rdName = st;

                        if (!rd.IsConnected)
                        {
                            if (rdName.Contains(" ICC"))
                            {
                                rd.Connect(rdName, SCardAccessMode.Shared, SCardProtocolIdentifiers.T1);
                                cardType = Enums.CardType.CONTACT;
                            }
                            else if (rdName.Contains(" PICC"))
                            {
                                rd.Connect(rdName, SCardAccessMode.Shared, SCardProtocolIdentifiers.CL);
                                cardType = Enums.CardType.CONTACTLESS;
                            }
                            else
                                rd.Connect(rdName, SCardAccessMode.Shared, SCardProtocolIdentifiers.T0);
                        }
                        else break;
                    }

                    if (rd.IsConnected)
                    {
                        rd.BeginTransaction();
                        if (!CheckNetwork())
                            return (int)Enums.ResultSC.NETWORK_HATA;
                        else
                            return (int)Enums.ResultSC.BASARILI;
                    }
                    else
                    {
                        return (int)Enums.ResultSC.KART_TAKILI_DEGIL;
                    }
                }
                else
                {
                    return (int)Enums.ResultSC.KART_OKUYUCU_BULUNAMADI;
                }
            }
            catch (Exception ex)
            {
                return (int)Enums.ResultSC.BASARISIZ;
            }
        }

        public int UpdateCard(byte adres)
        {
            int r = (int)Enums.ResultSC.BASARISIZ;

            switch (cardType)
            {
                case Enums.CardType.CONTACT:
                    r = UpdateCard_CT(adres);
                    break;
                case Enums.CardType.CONTACTLESS:
                    r = UpdateCard_CL(adres);
                    break;
            }

            return r;
        }

        public void ResetCard()
        {
            if (rd != null && rd.IsConnected) rd.EndTransaction(SCardDisposition.ResetCard);
        }

        public void FinishCard()
        {
            if (rd != null && rd.IsConnected) rd.EndTransaction(SCardDisposition.LeaveCard);
        }
        #endregion

        #region CONTACT
        public int ReadCard(byte adres)
        {
            byte[] command = new byte[5];
            byte[] response = new byte[6];

            command[0] = 0X80;
            command[1] = 0XBE;
            command[2] = 0X00;
            command[3] = adres; //Read adres
            command[4] = 0X04;

            rd.Transmit(command, out response);
            if (response.Length > 5)
            {
                if (response[4] == 0X90 && response[5] == 0X00)
                {
                    Array.Copy(response, 0, inBuf, 0, 4);
                    return (int)Enums.ResultSC.BASARILI;
                }
                else
                    return (int)Enums.ResultSC.OKUMA_HATASI;
            }
            else
            {
                return (int)Enums.ResultSC.OKUMA_HATASI;
            }
        }

        public int VerifyCard(byte adres)
        {
            byte[] command = new byte[9];
            byte[] response = new byte[2];

            command[0] = 0X00;
            command[1] = 0X20;
            command[2] = 0X00;
            command[3] = adres;  //verify  adres  = 7
            command[4] = 0X04;
            Array.Copy(outBuf, 0, command, 5, 4);

            try
            {
                rd.Transmit(command, out response);
                if (response[0] == 0X90 && response[1] == 0X00)
                    return (int)Enums.ResultSC.BASARILI;
                else
                    return (int)Enums.ResultSC.SIFRE_HATA;
            }
            catch (Exception ex)
            {
                return (int)Enums.ResultSC.SIFRE_HATA;
            }
        }

        public int UpdateCard_CT(byte adres)
        {
            byte[] command = new byte[9];
            byte[] response = new byte[2];

            command[0] = 0X80;
            command[1] = 0XDE;
            command[2] = 0X00;
            command[3] = adres;  //update adres
            command[4] = 0X04;
            Array.Copy(outBuf, 0, command, 5, 4);

            try
            {
                rd.Transmit(command, out response);
                if (response[0] == 0X90 && response[1] == 0X00)
                    return (int)Enums.ResultSC.BASARILI;
                else
                    return (int)Enums.ResultSC.SIFRE_HATA;
            }
            catch (Exception ex)
            {
                return (int)Enums.ResultSC.SIFRE_HATA;
            }
        }
        #endregion

        #region CONTACTLESS
        public int SelectBlock(string block)
        {
            byte[] command = new byte[7];
            byte[] response = new byte[2];

            command[0] = 0X00;
            command[1] = 0XA4;
            command[2] = 0X00;
            command[3] = 0X0C;
            command[4] = 0X02;

            char[] ID = block.ToCharArray();
            byte[] Veriler = { Convert.ToByte(ID[0]), Convert.ToByte(ID[1]) };

            Array.Copy(Veriler, 0, command, 5, 2);

            try
            {
                rd.Transmit(command, out response);
                if (response[0] == 0X90 && response[1] == 0X00)
                    return (int)Enums.ResultSC.BASARILI;
                else
                    return (int)Enums.ResultSC.BLOCK_HATA;
            }
            catch (Exception ex)
            {
                return (int)Enums.ResultSC.BLOCK_HATA;
            }
        }

        public int ExternalAuthenticate(byte KeyId, byte[] PinAbone)
        {
            byte[] command = new byte[13];
            byte[] response = new byte[2];

            command[0] = 0X00;
            command[1] = 0X82;
            command[2] = 0X00;
            command[3] = KeyId;
            command[4] = 0X08;

            Array.Copy(PinAbone, 0, command, 5, 8);

            try
            {
                rd.Transmit(command, out response);
                if (response[0] == 0X90 && response[1] == 0X00)
                    return (int)Enums.ResultSC.BASARILI;
                else
                    return (int)Enums.ResultSC.SIFRE_HATA;
            }
            catch (Exception ex)
            {
                return (int)Enums.ResultSC.SIFRE_HATA;
            }
        }

        public byte[] GetChallenge(byte[] PinAbone)
        {
            byte[] command = new byte[5];
            byte[] response = new byte[8];

            byte[] RandomSayi = new byte[8];
            byte[] kriptoPin;

            command[0] = 0X00;
            command[1] = 0X84;
            command[2] = 0X00;
            command[3] = 0X00;
            command[4] = 0X08;

            try
            {
                rd.Transmit(command, out response);
                if (response[8] == 0X90 && response[9] == 0X00)
                {
                    Array.Copy(response, 0, RandomSayi, 0, 8);
                    kriptoPin = new byte[8];

                    return classDes.TripleDes(DKAbone, RandomSayi, kriptoPin);
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }

            return kriptoPin;
        }

        public int ReadCard(byte adress, byte length)
        {
            byte[] command = new byte[5];
            byte[] response = new byte[6];

            Integer2Byte index = new Integer2Byte(adress);

            command[0] = 0X00;
            command[1] = 0XB0;
            command[2] = index.iki;
            command[3] = index.bir;
            command[4] = length;

            rd.Transmit(command, out response);
            if (response.Length > 5)
            {
                if (response[response.Length - 2] == 0X90 && response[response.Length - 1] == 0X00)
                {
                    Array.Copy(response, 0, inBuf, 0, response.Length - 2);
                    return (int)Enums.ResultSC.BASARILI;
                }
                else
                    return (int)Enums.ResultSC.OKUMA_HATASI;
            }
            else
            {
                return (int)Enums.ResultSC.OKUMA_HATASI;
            }
        }

        public int VerifyCard(byte keyID, byte[] pass)
        {
            byte[] command = new byte[13];
            byte[] response = new byte[2];

            command[0] = 0X00;
            command[1] = 0X20;
            command[2] = 0X00;
            command[3] = keyID;
            command[4] = 0X08;

            Array.Copy(pass, 0, command, 5, 8);

            try
            {
                rd.Transmit(command, out response);
                if (response[0] == 0X90 && response[1] == 0X00)
                    return (int)Enums.ResultSC.BASARILI;
                else
                    return (int)Enums.ResultSC.SIFRE_HATA;
            }
            catch (Exception ex)
            {
                return (int)Enums.ResultSC.SIFRE_HATA;
            }
        }

        public int UpdateCard_CL(UInt16 adres)
        {
            byte[] command = new byte[5 + refLength];
            byte[] response = new byte[2];

            Integer2Byte index = new Integer2Byte(adres);

            command[0] = 0X00;
            command[1] = 0XD6;
            command[2] = index.iki;
            command[3] = index.bir;
            command[4] = Convert.ToByte(refLength);

            Array.Copy(outBuf, 0, command, 5, refLength);

            rd.Transmit(command, out response);

            if (response[response.Length - 2] == 0X90 && response[response.Length - 1] == 0X00)
                return (int)Enums.ResultSC.BASARILI;
            else
                return (int)Enums.ResultSC.YAZMA_HATASI;
        }

        public int UpdateIssuer(string issuerName, UInt32 devNo)
        {
            char[] issuer = issuerName.ToCharArray();

            for (int i = 0; i < issuer.Length; i++)
            {
                outBuf[i] = Convert.ToByte(issuer[i]);
            }

            Integer4Byte cihaz = new Integer4Byte(devNo);

            outBuf[8] = cihaz.bir;
            outBuf[9] = cihaz.iki;
            outBuf[10] = cihaz.uc;
            outBuf[11] = cihaz.dort;
            refLength = 12;

            return UpdateCard(12);
        }

        public int WriteKey(byte keyID, byte[] keyData)
        {
            byte[] command = new byte[8 + keyData.Length];
            byte[] response = new byte[2];

            command[0] = 0X80;
            command[1] = 0XF4;
            command[2] = 0X01;
            command[3] = 0X00;
            command[4] = 0X0B;
            command[5] = 0XC2;
            command[6] = 0X09;
            command[7] = keyID;

            Array.Copy(keyData, 0, command, 8, 8);

            try
            {
                rd.Transmit(command, out response);
                if (response[0] == 0X90 && response[1] == 0X00)
                    return (int)Enums.ResultSC.BASARILI;
                else
                    return (int)Enums.ResultSC.SIFRE_HATA;
            }
            catch (Exception ex)
            {
                return (int)Enums.ResultSC.SIFRE_HATA;
            }
        }
        #endregion

        public string KartTipi()
        {
            string result = "";
            int r = 0;

            r = InitCard();
            if (r != (int)Enums.ResultSC.BASARILI)
            {
                ResetCard();
                return r.ToString();
            }

            switch (cardType)
            {
                case Enums.CardType.CONTACT:
                    result = KartTipi_CT();
                    break;
                case Enums.CardType.CONTACTLESS:
                    result = KartTipi_CL();
                    break;
            }

            FinishCard();
            return result;
        }

        public string KartTipi_CL()
        {
            string result = "";
            string issuer = "";
            int r = 0;

            r = SelectBlock(DFName);
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
            r = ExternalAuthenticate(0x84, GetChallenge(DKAbone));
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
            r = SelectBlock(EFIssuerName);
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

            r = ReadCard(12, 12);
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

            UInt32 CihazNo = Converter.Byte4toUInt32(inBuf[8], inBuf[9], inBuf[10], inBuf[11]);

            result += "ISSUER(HX)|" + inBuf[0] + " " + inBuf[1] + " " + inBuf[2] + " " + inBuf[3] + "\n";
            result += "ISSUER(CH)|" + (Char)inBuf[0] + " " + (Char)inBuf[1] + " " + (Char)inBuf[2] + " " + (Char)inBuf[3] + "\n";
            result += "Cihaz No  |" + CihazNo + "\n";
            issuer = ((Char)inBuf[0]).ToString() + ((Char)inBuf[1]).ToString() + ((Char)inBuf[2]).ToString() + ((Char)inBuf[3]).ToString();

            issuer = TipTanimlama(issuer);

            result += "KART TİPİ|" + issuer + "\n";

            return result;
        }

        public string KartTipi_CT()
        {
            string result = "";
            string issuer = "";
            int r = 0;

            r = ReadCard(1);
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

            result += "ISSUER(HX)|" + inBuf[0] + " " + inBuf[1] + " " + inBuf[2] + " " + inBuf[3] + "\n";
            result += "ISSUER(CH)|" + (Char)inBuf[0] + " " + (Char)inBuf[1] + " " + (Char)inBuf[2] + " " + (Char)inBuf[3] + "\n";
            result += "Cihaz No  |" + " " + "\n";
            issuer = ((Char)inBuf[0]).ToString() + ((Char)inBuf[1]).ToString() + ((Char)inBuf[2]).ToString() + ((Char)inBuf[3]).ToString();

            issuer = TipTanimlama(issuer);

            result += "KART TİPİ|" + issuer + "\n";

            return result;
        }

        public string TipTanimlama(string issuer)
        {
            if (zone + "A " == issuer)
            {
                issuer = "Abone Karti";
            }
            else if (zone + "YA" == issuer)
            {
                issuer = "Yetki Açma Karti";
            }
            else if (zone + "YE" == issuer)
            {
                issuer = "Yetki Bilgi Karti";
            }
            else if (zone + "YC" == issuer)
            {
                issuer = "Yetki İptal Karti";
            }
            else if (zone + "YK" == issuer)
            {
                issuer = "Yetki Kapama";
            }
            else if (zone + "YR" == issuer)
            {
                issuer = "Yetki Reset";
            }
            else if (zone + "YT" == issuer)
            {
                issuer = "Yetki Tüketim";
            }
            else if (zone + "YS" == issuer)
            {
                issuer = "Yetki Saat";
            }
            else if (zone + "Y4" == issuer)
            {
                issuer = "Yetki Ceza4";
            }
            else if (zone + "YV" == issuer)
            {
                issuer = "Yetki Avans";
            }
            else
            {
                switch (issuer)
                {
                    case "KSA ":
                        issuer = "Abone Karti";
                        break;

                    case "ASUS":
                        issuer = "Üretim Sıfırlama";
                        break;

                    case "ASUR":
                        issuer = "Üretim Reel";
                        break;

                    case "ASUT":
                        issuer = "Üretim Test";
                        break;

                    case "ASUA":
                        issuer = "Üretim Açma";
                        break;

                    case "ASUK":
                        issuer = "Üretim Kapama";
                        break;

                    case "ASUF":
                        issuer = "Üretim Format";
                        break;

                    case "ASUZ":
                        issuer = "Üretim Cihaz No";
                        break;

                    case "ASUI":
                        issuer = "Zone Kartı";
                        break;

                    case "BTLD":
                        issuer = "BootLoader";
                        break;

                    case "\0\0\0\0":
                        issuer = "Boş Kart";
                        break;
                }
            }

            return issuer;
        }

        public string AboneOku()
        {
            string result = "";
            int r = 0;

            r = InitCard();
            if (r != (int)Enums.ResultSC.BASARILI)
            {
                ResetCard();
                return r.ToString();
            }

            switch (cardType)
            {
                case Enums.CardType.CONTACT:
                    result = abone.AboneOku_CT();
                    break;
                case Enums.CardType.CONTACTLESS:
                    result = abone.AboneOku_CL();
                    break;
            }

            FinishCard();
            return result;
        }

        public int AboneYap(UInt32 devNo, UInt32 AboneNo, byte KartNo,
                            byte Cap, byte Tip, byte Donem,
                            UInt32 Fiyat1, UInt32 Fiyat2, UInt32 Fiyat3,
                            UInt32 Limit1, UInt32 Limit2,
                            byte Bayram1Gunn, byte Bayram1Ayy, byte Bayram1Suree,
                            byte Bayram2Gunn, byte Bayram2Ayy, byte Bayram2Suree,
                            byte AvansOnayi)
        {
            int r = 0;

            r = InitCard();
            if (r != (int)Enums.ResultSC.BASARILI)
            {
                ResetCard();
                return r;
            }

            switch (cardType)
            {
                case Enums.CardType.CONTACT:
                    r = abone.AboneYap_CT(Fiyat1, Fiyat2, Fiyat3, Limit1, Limit2, devNo, KartNo, AboneNo, Cap, Tip, Donem);
                    break;
                case Enums.CardType.CONTACTLESS:
                    r = abone.AboneYap_CL(devNo, AboneNo, KartNo, Cap, Tip, Donem, Fiyat1, Fiyat2, Fiyat3, Limit1, Limit2, Bayram1Gunn, Bayram1Ayy, Bayram1Suree, Bayram2Gunn, Bayram2Ayy, Bayram2Suree, AvansOnayi);
                    break;
            }

            FinishCard();

            SendLog(devNo, 0, Enums.ProcessType.ABONE_YAP, (Enums.ResultSC)r);
            return r;
        }

        public string KrediOku()
        {
            string result = "";
            int r = 0;

            r = InitCard();
            if (r != (int)Enums.ResultSC.BASARILI)
            {
                ResetCard();
                return r.ToString();
            }

            switch (cardType)
            {
                case Enums.CardType.CONTACT:
                    result = abone.KrediOku_CT();
                    break;
                case Enums.CardType.CONTACTLESS:
                    result = abone.KrediOku_CL();
                    break;
            }

            FinishCard();
            return result;
        }

        public int KrediYaz(UInt32 devNo, Int32 AnaKredi, Int32 YedekKredi,
                              byte Cap, byte Donem,
                              UInt32 Fiyat1, UInt32 Fiyat2, UInt32 Fiyat3,
                              UInt32 Limit1, UInt32 Limit2,
                              byte Bayram1Gunn, byte Bayram1Ayy, byte Bayram1Suree,
                              byte Bayram2Gunn, byte Bayram2Ayy, byte Bayram2Suree,
                              byte AvansOnayi)
        {
            int r = 0;

            r = InitCard();
            if (r != (int)Enums.ResultSC.BASARILI)
            {
                ResetCard();
                return r;
            }

            switch (cardType)
            {
                case Enums.CardType.CONTACT:
                    r = abone.KrediYaz_CT(Fiyat1, Fiyat2, Fiyat3, Limit1, Limit2, devNo, AnaKredi, YedekKredi, Donem, Cap);
                    break;
                case Enums.CardType.CONTACTLESS:
                    r = abone.KrediYaz_CL(devNo, AnaKredi, YedekKredi, Cap, Donem, Fiyat1, Fiyat2, Fiyat3, Limit1, Limit2, Bayram1Gunn, Bayram1Ayy, Bayram1Suree, Bayram2Gunn, Bayram2Ayy, Bayram2Suree, AvansOnayi);
                    break;
            }

            FinishCard();

            SendLog(devNo, AnaKredi, Enums.ProcessType.KREDI_YAZ, (Enums.ResultSC)r);
            return r;
        }

        public int AboneBosalt()
        {
            int r = 0;

            r = InitCard();
            if (r != (int)Enums.ResultSC.BASARILI)
            {
                ResetCard();
                return r;
            }

            switch (cardType)
            {
                case Enums.CardType.CONTACT:
                    r = abone.AboneBosalt_CT();
                    break;
                case Enums.CardType.CONTACTLESS:
                    r = abone.AboneBosalt_CL();
                    break;
            }

            FinishCard();
            return r;
        }
    }
}
