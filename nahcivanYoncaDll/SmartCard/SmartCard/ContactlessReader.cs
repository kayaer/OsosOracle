using SCLibWin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SmartCard
{
    public class ContactlessReader : IKartOkuyucu
    {
        #region değişkenler

        byte[] MasterKey = { 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23 };
        byte[] DKAbone = { 0x12, 0x53, 0x83, 0x26, 0x26, 0x61, 0x15, 0x98, 0x42, 0x11, 0x45, 0x76, 0x13, 0x62, 0x77, 0x19 };
        byte[] PinYetki = { 0x34, 0x61, 0x29, 0x56, 0x25, 0x07, 0x13, 0x68 };
        byte[] PinUretim = { 0x01, 0x01, 0x48, 0x29, 0x53, 0x72, 0x49, 0x06 };
        byte[] PinAbone = { 0x01, 0x01, 0x48, 0x29, 0x53, 0x72, 0x49, 0x06 };

        string DFName = "NN";
        string EFIssuerName = "EI";
        string EFDataName1 = "E1";
        string EFDataName2 = "E2";
        string EFDataName3 = "E3";
        string EFDataName4 = "E4";

        SCResMgr mng;
        SCReader rd;
        string rdName;

        private string _ATR = "";
        SCardReaderState state;
        public string CardSeriNo = "";

        private byte[] inBuf = new byte[255];
        private byte[] outBuf = new byte[255];

        #endregion

        #region ICardReader Members

        public byte[] InBuf
        {
            get
            {
                if (inBuf == null) return new byte[255];
                else return inBuf;
            }
            set
            {
                inBuf = value;
            }
        }

        public byte[] OutBuf
        {
            get
            {
                if (outBuf == null) return new byte[255];
                else return outBuf;
            }
            set
            {
                outBuf = value;
            }
        }

        public string ATR
        {
            get { return _ATR; }
        }

        public int ReadCard(UInt16 Adres, byte Adet)
        {
            inBuf = ReadBinary(Adres, Adet);

            if (inBuf == null) return 0;
            else return 1;
        }

        public int VerifyCard(byte Adres)
        {
            throw new NotImplementedException();
        }

        public int UpdateCard(UInt16 Adres)
        {
            throw new NotImplementedException();
        }

        public int Eject()
        {
            return 1;
        }

        #endregion

        #region ACR128 Kart Reader fonksyonları

        private string GetCardSeriNo()
        {
            byte[] SeriNo = GetCardData();
            string seri = "";
            if (SeriNo != null)
            {
                foreach (byte b in SeriNo)
                {
                    seri += b.ToString("X2");
                }
            }

            return seri;
        }

        public int InitCard()
        {

            try
            {
                mng = new SCResMgr();
                mng.EstablishContext(SCardContextScope.User);
                rd = new SCReader(mng);
                ArrayList al = new ArrayList();
                mng.ListReaders(al);
                foreach (string st in al)
                {
                    rdName = st;
                    if (!rd.IsConnected)
                        rd.Connect(rdName, SCardAccessMode.Shared, SCardProtocolIdentifiers.CL);
                    else break;
                }

                if (rd.IsConnected)
                {
                    state = rd.Status();
                    _ATR = rd.m_ATR;
                    rd.BeginTransaction();

                    CardSeriNo = GetCardSeriNo();

                    return Convert.ToInt32(rd.IsConnected);
                }
                else return Convert.ToInt32(rd.IsConnected);
            }
            catch (Exception ex)
            {
                return Convert.ToInt32(rd.IsConnected);
            }
        }

        public string FinishCard()
        {
            try
            {
                if (rd.IsConnected)
                {
                    rd.EndTransaction(SCardDisposition.UnpowerCard);
                    rd.Disconnect(SCardDisposition.UnpowerCard);
                    mng.ReleaseContext();
                }

            }
            catch (Exception ex)
            {
                return "0";
            }

            return "0";
        }

        #region test fonksiyonları

        //public bool Rats()
        //{
        //    byte[] command = new byte[5];
        //    byte[] response = new byte[2];

        //    //command[0] = 0XE0; //CLA
        //    //command[1] = 0X50; //INS
        //    command[0] = 0Xff;
        //    command[1] = 0Xca;
        //    command[2] = 0X00;
        //    command[3] = 0X00;
        //    command[4] = 0X00;




        //    try
        //    {
        //        rd.Transmit(command, out response);
        //        if (response[0] == 0X90 && response[1] == 0X00) return true;
        //        else return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //public string GetSerial()
        //{
        //    byte[] command = new byte[3];
        //    byte[] response = new byte[2];

        //    command[0] = 0X22; //CLA
        //    command[1] = 0X01; //INS
        //    command[2] = 0X0A; //INS
        //    //command[2] = 0X80; //P1 SC : Selection Kontrol
        //    // command[3] = 0X00; //P2 SO : Selection Options
        //    // command[4] = 0X00; // Lc : Command Length
        //    //command[3] = 0X03;

        //    try
        //    {
        //        rd.Control((int)WinSCard.IOCTL_SMARTCARD_ACR128_ESCAPE_COMMAND, command, out response);
        //        if (response[0] == 0X90 && response[1] == 0X00) return "1";
        //        else return "0";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "0";
        //    }

        //    return "1";

        //}

        //public void LEDOn(bool param1, bool param2)
        //{
        //    byte[] command = new byte[3];
        //    byte[] response = new byte[2];


        //    command[0] = 0X29; //CLA
        //    command[1] = 0X01; //INS
        //    command[2] = 0x00;
        //    if (param1)
        //        command[2] |= 0X01; //INS
        //    if (param2)
        //        command[2] |= 0X02;
        //    //command[2] = 0X80; //P1 SC : Selection Kontrol
        //    // command[3] = 0X00; //P2 SO : Selection Options
        //    // command[4] = 0X00; // Lc : Command Length
        //    //command[3] = 0X03;

        //    try
        //    {
        //        rd.Control((int)WinSCard.IOCTL_SMARTCARD_ACR128_ESCAPE_COMMAND, command, out response);
        //        //if (response[0] == 0X90 && response[1] == 0X00) return "1";
        //        //else return "0";
        //    }
        //    catch (Exception ex)
        //    {
        //        //return "0";
        //    }

        //    //return "1";
        //}

        //public void AntenOn(bool param)
        //{
        //    byte[] command = new byte[3];
        //    byte[] response = new byte[2];


        //    command[0] = 0X25; //CLA
        //    command[1] = 0X01; //INS
        //    if (param)
        //        command[2] = 0X01; //INS
        //    else command[2] = 0X00;
        //    //command[2] = 0X80; //P1 SC : Selection Kontrol
        //    // command[3] = 0X00; //P2 SO : Selection Options
        //    // command[4] = 0X00; // Lc : Command Length
        //    //command[3] = 0X03;

        //    try
        //    {
        //        rd.Control((int)WinSCard.IOCTL_SMARTCARD_ACR128_ESCAPE_COMMAND, command, out response);
        //        //if (response[0] == 0X90 && response[1] == 0X00) return "1";
        //        //else return "0";
        //    }
        //    catch (Exception ex)
        //    {
        //        //return "0";
        //    }

        //    //return "1";
        //}

        //public string level1()
        //{
        //    InitCard();

        //    byte[] command = new byte[3];
        //    byte[] response = new byte[2];

        //    command[0] = 0X23; //CLA
        //    command[1] = 0X01; //INS
        //    command[2] = 0X08; //INS
        //    //command[2] = 0X80; //P1 SC : Selection Kontrol
        //    // command[3] = 0X00; //P2 SO : Selection Options
        //    // command[4] = 0X00; // Lc : Command Length
        //    //command[3] = 0X03;

        //    try
        //    {
        //        rd.Control((int)WinSCard.IOCTL_SMARTCARD_ACR128_ESCAPE_COMMAND, command, out response);
        //        if (response[0] == 0X90 && response[1] == 0X00) return "1";
        //        else return "0";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "0";
        //    }

        //    return "1";

        //}

        //public string ManuelCardDetection()
        //{
        //    //InitKart();
        //    byte[] command = new byte[3];
        //    byte[] response = new byte[2];

        //    command[0] = 0X22; //CLA
        //    command[1] = 0X01; //INS
        //    command[2] = 0X0A; //INS
        //    //command[2] = 0X80; //P1 SC : Selection Kontrol
        //    // command[3] = 0X00; //P2 SO : Selection Options
        //    // command[4] = 0X00; // Lc : Command Length
        //    //command[3] = 0X03;

        //    try
        //    {
        //        rd.Control((int)WinSCard.IOCTL_SMARTCARD_ACR128_ESCAPE_COMMAND, command, out response);
        //        if (response[5] == 0X00) return "1";
        //        else return "0";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "0";
        //    }

        //    return "1";

        //}

        #endregion

        public bool SelectFile(string File)
        {
            //string ss = GetSerial();

            byte[] command = new byte[7];
            byte[] response = new byte[2];

            command[0] = 0X00; //CLA
            command[1] = 0XA4; //INS
            command[2] = 0X00; //P1 SC : Selection Kontrol
            command[3] = 0X0C; //P2 SO : Selection Options
            command[4] = 0X02; // Lc : Command Length
            // DATA   

            char[] ID = File.ToCharArray();
            byte[] Veriler = { Convert.ToByte(ID[0]), Convert.ToByte(ID[1]) };

            Array.Copy(Veriler, 0, command, 5, 2);

            try
            {
                rd.Transmit(command, out response);
                if (response[0] == 0X90 && response[1] == 0X00) return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool ExternalAuthenticate(byte KeyId, byte[] PinAbone)
        {
            byte[] command = new byte[13];
            byte[] response = new byte[2];

            command[0] = 0X00; //CLA
            command[1] = 0X82; //INS
            command[2] = 0X00; //P1 Symmetric Algorithm
            command[3] = KeyId; //P2 KeyId
            command[4] = 0X08; // Lc : Command Length
            // PİN   

            Array.Copy(PinAbone, 0, command, 5, 8);

            try
            {
                rd.Transmit(command, out response);
                if (response[0] == 0X90 && response[1] == 0X00) return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool MutualAuthentication(byte KeyId, byte[] Pin)
        {
            byte[] command = new byte[13];
            byte[] response = new byte[2];

            command[0] = 0X80; //CLA
            command[1] = 0X8A; //INS
            command[2] = 0X00; //P1 Symmetric Algorithm
            command[3] = KeyId; //P2 KeyId
            command[4] = 0X18; // Lc : Command Length
            // PİN   

            Array.Copy(Pin, 0, command, 5, 8);

            try
            {
                rd.Transmit(command, out response);
                if (response[0] == 0X90 && response[1] == 0X00) return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public byte[] GetCardData()
        {
            byte[] command = new byte[5];
            byte[] response = new byte[2];
            byte[] SeriNo = new byte[8];

            command[0] = 0X80; //CLA
            command[1] = 0XF6; //INS
            command[2] = 0X00; //P1 Symmetric Algorithm
            command[3] = 0X00; //P2 KeyId
            command[4] = 0X08; // Lc : Command Length

            try
            {
                rd.Transmit(command, out response);
                if (response[8] == 0X90 && response[9] == 0X00)
                {
                    Array.Copy(response, 0, SeriNo, 0, 8);
                    return SeriNo;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }

        public byte[] GetChallenge(byte[] PinAbone)
        {

            byte[] command = new byte[5];
            byte[] response = new byte[8];

            byte[] RandomSayi = new byte[8];
            byte[] kriptoPin;

            command[0] = 0X00; //CLA
            command[1] = 0X84; //INS
            command[2] = 0X00; //P1 
            command[3] = 0X00; //P2
            command[4] = 0X08; // Le : Command Length
            // PİN   

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

        public byte[] ReadBinary(UInt16 Ofset, byte Adet)
        {
            byte[] command = new byte[5];
            byte[] response = new byte[2];

            Integer2Byte Index = new Integer2Byte(Ofset);

            command[0] = 0X00; //CLA
            command[1] = 0XB0; //INS
            command[2] = Index.iki; //P1, OH = Offset High Byte for implicit selection 
            command[3] = Index.bir; //P2, OL = Offset Low Byte
            command[4] = Adet; // Le : Number of digits to be read  


            try
            {
                rd.Transmit(command, out response);

                if (response[response.Length - 2] == 0X90 && response[response.Length - 1] == 0X00)
                {
                    return response;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }

        public bool VerifyPin(byte KeyID, byte[] Sifre)
        {
            byte[] command = new byte[13];
            byte[] response = new byte[2];

            command[0] = 0X00; //CLA
            command[1] = 0X20; //INS
            command[2] = 0X00; //P1 
            command[3] = KeyID; //P2 KeyId
            command[4] = 0X08; // Lc : Command Length
            // PİN   

            Array.Copy(Sifre, 0, command, 5, 8);

            try
            {
                rd.Transmit(command, out response);
                if (response[0] == 0X90 && response[1] == 0X00) return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public bool UpdateBinary(UInt16 Ofset, params byte[] data)
        {
            byte[] command = new byte[5 + data.Length];
            byte[] response = new byte[2];

            Integer2Byte Index = new Integer2Byte(Ofset);

            command[0] = 0X00; //CLA
            command[1] = 0XD6; //INS
            command[2] = Index.iki; //P1, OH = Offset High Byte for implicit selection 
            command[3] = Index.bir; //P2, OL = Offset Low Byte
            command[4] = Convert.ToByte(data.Length); // Le : Number of digits to be write  

            Array.Copy(data, 0, command, 5, data.Length);

            try
            {
                rd.Transmit(command, out response);
                if (response[response.Length - 2] == 0X90 && response[response.Length - 1] == 0X00)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool UpdateBinaryAdet(UInt16 Ofset, byte Adet, params byte[] data)
        {
            byte[] Veriler = new byte[Adet];
            byte[] command = new byte[5 + Veriler.Length];
            byte[] response = new byte[2];


            Array.Copy(data, Ofset, Veriler, 0, Adet);

            Integer2Byte Index = new Integer2Byte(Ofset);

            command[0] = 0X00; //CLA
            command[1] = 0XD6; //INS
            command[2] = Index.iki; //P1, OH = Offset High Byte for implicit selection 
            command[3] = Index.bir; //P2, OL = Offset Low Byte
            command[4] = Convert.ToByte(Veriler.Length); // Le : Number of digits to be write  

            Array.Copy(Veriler, 0, command, 5, Veriler.Length);

            try
            {
                rd.Transmit(command, out response);
                if (response[response.Length - 2] == 0X90 && response[response.Length - 1] == 0X00)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool WriteKey(byte KeyID, byte[] KeyData)
        {
            byte[] command = new byte[8 + KeyData.Length];
            byte[] response = new byte[2];


            command[0] = 0X80; //CLA
            command[1] = 0XF4; //INS
            command[2] = 0X01; //OP (Operation Mode): 01 : Update, 00: Install, 02:Update Mifare access key  (DATA only contains the key)
            command[3] = 0X00; //SE If OP = ’00’ or OP= ‘01’ 
            command[4] = 0X0B; // Lc : key length  - is always ‘10’
            command[5] = 0XC2;
            command[6] = 0X09;
            command[7] = KeyID;




            Array.Copy(KeyData, 0, command, 8, 8);

            try
            {
                rd.Transmit(command, out response);
                if (response[0] == 0X90 && response[1] == 0X00) return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        #endregion

        #region Erişim fonksiyonları

        private bool EFDataNameEris(UInt32 CihazNo, string EFDataName)
        {
            byte[] PinAbone = new byte[8];

            Integer4Byte CihazNoo = new Integer4Byte(CihazNo);

            byte[] Gonderilecek = { CihazNoo.bir, CihazNoo.iki, CihazNoo.uc, CihazNoo.dort,
                                    Convert.ToByte(CihazNoo.bir^0x0F), Convert.ToByte(CihazNoo.iki^0x0F),
                                    Convert.ToByte(CihazNoo.uc^0x0F), Convert.ToByte(CihazNoo.dort^0x0F) };

            PinAbone = classDes.TripleDes(DKAbone, Gonderilecek, PinAbone);

            bool Status = VerifyPin(0x83, PinAbone);
            if (!Status) { FinishCard(); return false; }

            Status = SelectFile(EFDataName);
            if (!Status) { FinishCard(); return false; }

            return true;
        }

        private bool EFDataNameEris(string KartTipi, string EFDataName)
        {
            bool Status = true;

            switch (KartTipi)
            {
                case "Uretim":
                    Status = VerifyPin(0x81, PinUretim);
                    if (!Status) { FinishCard(); return false; }
                    break;
                case "Yetki":
                    Status = VerifyPin(0x82, PinYetki);
                    if (!Status) { FinishCard(); return false; }
                    break;
            }

            Status = SelectFile(EFDataName);
            if (!Status) { FinishCard(); return false; }

            return true;
        }

        private string[] EFDataNameDosyasinaErisimAyarlari()
        {
            string[] DonusDegeri = { "0", "0", "0" };

            bool Status = SelectFile(DFName);
            if (!Status) { FinishCard(); DonusDegeri[0] = "0"; return DonusDegeri; }

            Status = ExternalAuthenticate(0x84, GetChallenge(DKAbone));
            if (!Status) { FinishCard(); DonusDegeri[0] = "0"; return DonusDegeri; }

            Status = SelectFile(EFIssuerName);
            if (!Status) { FinishCard(); DonusDegeri[0] = "0"; return DonusDegeri; }

            byte[] OkunanDegerler = ReadBinary(0, 12);

            string IssuerArea = Hexcon.BytetoString(0, 4, OkunanDegerler);
            DonusDegeri[1] = IssuerArea;

            UInt32 CihazNo = Hexcon.Byte4toUInt32(OkunanDegerler[8], OkunanDegerler[9], OkunanDegerler[10], OkunanDegerler[11]);
            DonusDegeri[2] = CihazNo.ToString();

            DonusDegeri[0] = "1";

            return DonusDegeri;
        }

        private bool UpdateIssuer(string IssuerName, UInt32 CihazNo)
        {
            char[] Issuer = IssuerName.ToCharArray();

            byte[] YazilacakData = new byte[12];

            for (int i = 0; i < Issuer.Length; i++)
            {
                YazilacakData[i] = Convert.ToByte(Issuer[i]);
            }

            Integer4Byte Cihaz = new Integer4Byte(CihazNo);

            YazilacakData[8] = Cihaz.bir;
            YazilacakData[9] = Cihaz.iki;
            YazilacakData[10] = Cihaz.uc;
            YazilacakData[11] = Cihaz.dort;

            bool Sonuc = UpdateBinary(0, YazilacakData);

            return Sonuc;
        }

        #endregion
    }
}
