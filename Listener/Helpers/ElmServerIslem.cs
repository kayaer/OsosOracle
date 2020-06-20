using Listener.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Listener.Helpers
{
    public class ElmServerIslem
    {
        private TcpClient _Client;
        private NetworkStream stream;
        private Header konP;
        public bool Logined = false;
        byte[] sessionKey = new byte[24];
        private byte[] DesKey = new byte[8];
        Paket paket = new Paket();
        private bool KriptoKontrol = false;
        Error Error = new Error();
        public Header KonsantratorPaket = new Header();
        int LoginMode = 0;
        CommandFactory sm = new CommandFactory();
        HexCon hexCon = new HexCon();
        const byte ACK = 0X06;
        const byte NAK = 0X15;
        public string FIRMWARE = "";
        public event OnPaketSend PaketSent;
        public ElmServerIslem(TcpClient client)
        {
            _Client = client;
            stream = client.GetStream();
        }
        public bool CloseConnection()
        {
            bool sonuc = false;
            Logined = false;
            //konP = null;

            try
            {
                //if (Client.Connected)
                //{
                
                _Client.Client.Shutdown(SocketShutdown.Both);
                _Client.Close();
                stream.Dispose();
                sonuc = _Client.Connected;
                //}
            }
            catch (Exception ex)
            {
                Error.Set(6);
                //gb.LogYaz(AMR_SOFTWARE.AMR_SERVER, AMR_EXCEPTION_TYPE.ERROR, ex, "ELM Konsantratörle bağlantı kesilemedi");

                Logined = false;
                //konP = null;
            }

            return false;
        }
        public bool LoginIslemleri(int Mode, byte[] HeaderPaket)
        {
            try
            {
                if (konP == null) konP = new Header(HeaderPaket);

                //ByteToBit state = new ByteToBit(Convert.ToInt32(konP.State));

                if (!Logined)
                {
                    Logined = paket.Login(stream, Convert.ToUInt32(konP.RandomSayi), Mode, KriptoKontrol, ref sessionKey);

                    if (sessionKey == null)
                    {
                        Error.Set(2);
                        Logined = false;
                    }
                    else
                    {
                        Buffer.BlockCopy(sessionKey, 16, DesKey, 0, 8);
                        Logined = true;
                    }

                }

            }
            catch (Exception ex)
            {
                //Logined = false;
                //konP = null;
                //gb.LogYaz(AMR_SOFTWARE.AMR_SERVER, AMR_EXCEPTION_TYPE.ERROR, ex, "ELM konsantrator Login hata");
            }

            return Logined;
        }

        public CommandResponse[] ServisGonder(AmiCommand[] Servisler)
        {
            List<CommandResponse> res = new List<CommandResponse>();

            byte[] message = null;

            if (!Logined) LoginIslemleri(LoginMode, null);

            if (Logined)
            {
                try
                {
                    //string[] ipPort = TcpClient.Client.RemoteEndPoint.ToString().Split(new char[] { ':' });
                    //string KonsIp = ipPort[0];



                    #region servis kontrol ve programming mode
                    int dongu = 0;
                    foreach (AmiCommand servis in Servisler)
                    {
                        dongu++;
                        //her servisin cevabı başta başarasız olarak set ediliyor. Her servise cevap vermek gerek
                        CommandResponse srvRes = new CommandResponse(servis.Id, servis.Kod, "-1");

                        #region servis gönder
                        try
                        {

                            byte[] srvPaket = sm.KomutPaketHazirla(servis);

                            paket.PaketGonder(stream, srvPaket, KriptoKontrol, DesKey);

                            message = paket.PaketOku(stream, KriptoKontrol, DesKey);
                            Console.WriteLine(dongu.ToString() + "/" + Servisler.Count() + " Servis Modeme Yollandi...");
                            Console.WriteLine("Servis Id: " + servis.Id);
                            Console.WriteLine("Servis Kod: " + servis.Kod);
                            Console.WriteLine("Servis Cevabı: " + ASCIIEncoding.ASCII.GetString(message));

                            if (message == null)
                            {
                                Error.Set(4);
                                Console.WriteLine(string.Concat("SERVİS HATA: ", dongu.ToString(), message));
                                //throw new Exception("ELM Paket Okuma Hatası");
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(dongu.ToString() + " " + ex.Message);
                            res.Add(srvRes);
                            return res.ToArray();
                        }
                        Console.WriteLine("\r\n");


                        #endregion


                        #region servis cevapları

                        if (message != null)
                        {
                            string servisCevap = hexCon.BytesTostr(message);


                            //servis cevabı alındı 
                            srvRes = new CommandResponse(servisCevap);


                            if (srvRes.Sonuc == "0")
                            {
                                List<string> DataList = new List<string>();
                                string YukProfili = "";
                                string AnlikReadoutdata = "";


                                #region Readout servisi ise

                                try
                                {
                                    if (servis.Kod == CommandCode.CMD_SEND_READOUTS)
                                    {
                                        paket.SendAck(stream, KriptoKontrol, DesKey);

                                        while (true)
                                        {
                                            #region readoutları çek

                                            message = paket.PaketOku(stream, KriptoKontrol, DesKey);

                                            if (message == null)
                                            {
                                                Error.Set(4);
                                                throw new Exception("ELM Paket Okuma Hatası");
                                            }

                                            if (message[0] == NAK && message.Length < 10) break;
                                            else
                                            {
                                                string data = hexCon.BytesTostr(message);

                                                DataList.Add(data);

                                                paket.SendAck(stream, KriptoKontrol, DesKey);
                                            }

                                            #endregion
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (DataList.Count > 0) srvRes.Readouts = DataList.ToArray();
                                    res.Add(srvRes);
                                    return res.ToArray();
                                }

                                srvRes.Readouts = DataList.ToArray();


                                #endregion


                                #region Anlık readout servisi ise

                                try
                                {
                                    if (servis.Kod == CommandCode.CMD_INSTANT_READOUT)
                                    {
                                        paket.SendAck(stream, KriptoKontrol, DesKey);

                                        message = paket.PaketOku(stream, KriptoKontrol, DesKey);

                                        if (message == null)
                                        {
                                            Error.Set(4);
                                            throw new Exception("ELM Paket Okuma Hatası");
                                        }

                                        AnlikReadoutdata = hexCon.BytesTostr(message);

                                        //if (!PaketKontrol(AnlikReadoutdata))
                                        //{
                                        //    Hata.Set(9);
                                        //    throw new Exception("ELM Bozuk Paket okundu");
                                        //}
                                    }
                                }
                                catch (Exception ex)
                                {
                                    srvRes = new CommandResponse(servis.Id, servis.Kod, "-1");
                                    continue;
                                }

                                srvRes.AnlikReadout = AnlikReadoutdata;

                                #endregion


                                #region Yük profili ise

                                try
                                {

                                    if (servis.Kod == CommandCode.CMD_LOAD_PROFILE)
                                    {
                                        paket.SendAck(stream, KriptoKontrol, DesKey);

                                        while (true)
                                        {
                                            #region Yük profili bilgilerini oku

                                            message = paket.PaketOku(stream, KriptoKontrol, DesKey);

                                            if (message == null)
                                            {
                                                Error.Set(8);
                                                throw new Exception("ELM yük profili okuma Hatası");
                                            }

                                            if (message[0] != NAK) //nak değilse
                                            {
                                                string data = hexCon.BytesTostr(message);
                                                YukProfili += data;
                                                //paket.SendAck(stream, KriptoKontrol, DesKey);

                                            }
                                            else break;

                                            #endregion
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (!string.IsNullOrEmpty(YukProfili))
                                    {
                                        srvRes.YukProfili = YukProfili;
                                        res.Add(srvRes);
                                    }
                                    else
                                    {
                                        srvRes = new CommandResponse(servis.Id, servis.Kod, "-1");
                                        res.Add(srvRes);
                                    }
                                    continue;
                                }

                                srvRes.YukProfili = YukProfili;

                                #endregion


                                #region firmware update ise

                                try
                                {

                                    if (servis.Kod == CommandCode.CMD_ERASE_FLASH)
                                    {
                                        if (konP != null)
                                        {
                                            if (konP.Versiyon == FIRMWARE)
                                            {
                                                srvRes = new CommandResponse(servis.Id, servis.Kod, "0");
                                                res.Add(srvRes);
                                                continue;
                                            }

                                        }
                                        else
                                        {
                                            srvRes = new CommandResponse(servis.Id, servis.Kod, "-1");
                                            res.Add(srvRes);
                                            continue;
                                        }

                                        List<byte> alFirmware = new List<byte>();
                                        AmiGlobals gb = new AmiGlobals();
                                        alFirmware = gb.GetHexDosya(KonsModel.RScUltra, "rsc.hex");

                                        if (alFirmware == null || alFirmware.Count <= 0)
                                        {
                                            srvRes = new CommandResponse(servis.Id, servis.Kod, "-1");
                                            res.Add(srvRes);
                                            continue;
                                        }

                                        byte[] firmwarePaket = new byte[1048];
                                        int firmwareUzunluk = alFirmware.Count;
                                        int uzunluk = 0;
                                        int kalanUzunluk = 0;
                                        int paketSent = 0;
                                        int paketBoyutu = 1000;


                                        #region firmware paketini gönder

                                        while (firmwareUzunluk > 0)
                                        {

                                            if (firmwareUzunluk > paketBoyutu)
                                            {
                                                kalanUzunluk = paketBoyutu;
                                            }
                                            else
                                            {
                                                kalanUzunluk = firmwareUzunluk;
                                            }


                                            firmwarePaket[0] = (byte)'W'; // yazma komutu
                                            firmwarePaket[1] = (byte)'R'; // yazma komutu

                                            for (int i = 0; i < kalanUzunluk; i++)
                                            {
                                                firmwarePaket[i + 2] = alFirmware[uzunluk + i];
                                            }

                                            uzunluk += kalanUzunluk;
                                            firmwareUzunluk -= kalanUzunluk;

                                            byte[] data = new byte[kalanUzunluk + 2];
                                            Buffer.BlockCopy(firmwarePaket, 0, data, 0, kalanUzunluk + 2);

                                            paket.PaketGonder(stream, data, KriptoKontrol, DesKey);

                                            if (this.PaketSent != null)
                                            {
                                                paketSent += kalanUzunluk;
                                                this.PaketSent(paketBoyutu, paketSent, alFirmware.Count);
                                            }

                                            message = paket.PaketOku(stream, KriptoKontrol, DesKey);

                                            if (message[0] != ACK)
                                            {
                                                Error.Set(5);
                                                throw new Exception("Programlama hata");
                                            }
                                        }

                                        //     message = paket.PaketOku(stream, KriptoKontrol, DesKey);

                                        //                        if (message[0] == ACK)

                                        byte[] pgPaket = new byte[2];


                                        pgPaket[0] = (byte)'P'; // yazma komutu
                                        pgPaket[1] = (byte)'G';

                                        paket.PaketGonder(stream, pgPaket, KriptoKontrol, DesKey);

                                        message = paket.PaketOku(stream, KriptoKontrol, DesKey);

                                        if (message[0] == ACK)
                                        {
                                            paket.SendAck(stream, KriptoKontrol, DesKey);
                                        }
                                        else
                                        {
                                            throw new Exception("Programlama hata");
                                        }

                                        CloseConnection();

                                        #endregion
                                    }
                                }
                                catch (Exception ex)
                                {
                                    CloseConnection();
                                    srvRes = new CommandResponse(servis.Id, servis.Kod, "-1");
                                    res.Add(srvRes);
                                    continue;
                                }

                                #endregion

                            }

                        }

                        #endregion

                        res.Add(srvRes);
                    }


                    #endregion

                }
                catch (Exception ex)
                {

                    CloseConnection();

                }

            }

            return res.ToArray();
        }

    }
}
