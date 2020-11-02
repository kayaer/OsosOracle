using Listener.Entities;
using Listener.Helpers;
using log4net;
using Newtonsoft.Json;
using OsosOracle.DataLayer.Concrete.EntityFramework.Dal;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.EntIsEmriComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Entities.Enums;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Listener
{
    public class TcpServer
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ListenerWindowsService));
        private int _port { get; set; }
        private static int BaglantiSayisi = 0;
        private TcpListener tcpListener;
        private Thread listenThread;
        HexCon hexCon = new HexCon();
        public bool ZamanSenkronizasyonu = true;
        public int ZamanTolerans = 60; // Default 60 Saniye
        public int PROJEKOD = 1;
        public Hashtable hu = new Hashtable();
        public bool FIRMWARE_UPDATE = false;
        public TcpServer(int port)
        {
            _port = port;
        }

        public bool StartListen()
        {
            try
            {
              
                tcpListener = new TcpListener(IPAddress.Any, _port);
                listenThread = new Thread(new ThreadStart(ListenForClients));
                listenThread.Start();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                CevapYaz("Server başlatılamadı !!! Server Ip : " + GetLocalIP() + " Port : " + _port + " " + DateTime.Now.ToString() + "\r\n" + ex.Message, ConsoleColor.Red);
                return false;
            }
            CevapYaz("Server Başladı... Server Ip : " + GetLocalIP() + " Port : " + _port + " " + DateTime.Now.ToString());
            _log.Info("Server Başladı... Server Ip : " + GetLocalIP() + " Port : " + _port);
            return true;
        }

        private void ListenForClients()
        {
            try
            {

                this.tcpListener.Start();
            }
            catch (Exception ex)
            {

                CevapYaz("ListenForClients - tcpListener.Start\r\n" + "Bu port açık ve dinlemede zaten !!", ConsoleColor.Red);
                _log.Error("ListenForClients - tcpListener.Start\r\n" + "Bu port açık ve dinlemede zaten !!");
                return;
            }

            while (true)
            {
                try
                {
                    TcpClient client = this.tcpListener.AcceptTcpClient(); // Gelen bir haberleşme varsa HandleClientComm metoduna gidiyor.
                    Thread clientThread = new Thread(() => HandleClientComm(client));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
                catch (SocketException ex)
                {
                    CevapYaz("ListenForClients while içinde hata \n" + ex.Message);
                    _log.Error("ListenForClients while içinde hata" + ex.Message);
                    return;
                }
            }


        }

        private void HandleClientComm(TcpClient tcpClient)
        {
            Interlocked.Increment(ref BaglantiSayisi);

            try
            {
                ServerIslem(tcpClient); // tüm listener işlemleri burada yapılıyor
            }
            catch (Exception ex)
            {
                //if (tcpClient.Connected) tcpClient.Close();
                CevapYaz("HandleClientComm Hata :\r\n" + "Tarih : " + DateTime.Now.ToString() + "\r\n" + ex.Message + "\r\n" + ex.StackTrace);
                _log.Error("HandleClientComm Hata :\r\n" + "Tarih : " + DateTime.Now.ToString() + "\r\n" + ex.Message + "\r\n" + ex.StackTrace);

            }

            Interlocked.Decrement(ref BaglantiSayisi);
        }

        public void ServerIslem(TcpClient tcpClient)
        {
            #region watch

            Stopwatch sw = new Stopwatch(); // DB de tutulan işlem süresini bu hesaplıyor.
            TimeSpan IslemSuresiTs;
            int IslemSuresi = 0;
            try
            {
                sw.Start();
                IslemSuresiTs = new TimeSpan();
            }
            catch (Exception ex)
            {
                IslemSuresi = 0;
            }

            #endregion

            string KonsIp = "";
            string[] ipPort = tcpClient.Client.RemoteEndPoint.ToString().Split(new char[] { ':' });
            KonsIp = ipPort[0];
            string cevap = string.Concat("Bağlantı Geldi\nIp : ", KonsIp, "\n", DateTime.Now.ToString(), "\n");
            Thread.Sleep(1);
            CevapYaz(cevap, ConsoleColor.Red);

            bool Logined = false;
            List<string> SayacDataList = new List<string>();

            tcpClient.ReceiveTimeout = 60000; // 1 dakika
            tcpClient.SendTimeout = 60000;

            //IList<AmrServis> dbSrvList;

            byte[] buffer = new byte[16000];
            int bytesRead = 0;

            byte[] header = null;

            #region gelen paket kontrol
            try
            {
                bytesRead = tcpClient.GetStream().Read(buffer, 0, buffer.Length);

                header = new byte[bytesRead];

                Buffer.BlockCopy(buffer, 0, header, 0, bytesRead);
                

                bool crc = hexCon.CrcKontrol(header);

                if (!crc)
                {
                    string hd = hexCon.BytesTostr(header, 2, bytesRead - 4);

                    string hex = BitConverter.ToString(header);

                    CevapYaz("CRC hata " + "\r\n" +
                             "Hex : " + hex + "\r\n" +
                             "KonsIp : " + KonsIp + "\r\n" +
                             "Header : " + hd,
                             ConsoleColor.Red);

                    return;
                }
            }
            catch (Exception ex)
            {
                CevapYaz("Bağlantı hata \r\n" + "KonsIp : " + KonsIp, ConsoleColor.Red);

                if (tcpClient.Connected)
                {
                    tcpClient.Close();
                }

                return;
            }

            string headerPaket = hexCon.BytesTostr(buffer, bytesRead); // veri byte olarak geliyor. burada string e çevriliyor. bağlanır bağlanmaz headerpaket geliyor.
            ElmServerIslem serverIslem = new ElmServerIslem(tcpClient);
            CevapYaz(headerPaket);
            _log.Info(headerPaket);
            try
            {
                Header hd = new Header(header); // header ın parse işlemi burada oluyor.

                serverIslem.KonsantratorPaket = hd;
                if (!HeaderCheck(hd.StrPaket))
                {
                    CevapYaz("BOZUK HEADER\r\n" + headerPaket + "\r\nKonsIp : " + KonsIp, ConsoleColor.Red);
                    return;
                }
            }
            catch (Exception ex)
            {

                CevapYaz("BOZUK HEADER\r\n" + headerPaket + "\r\nKonsIp : " + KonsIp, ConsoleColor.Red);
                CevapYaz(headerPaket + "\r\n" + "KonsIp : " + KonsIp);

                if (tcpClient.Connected)
                {
                    tcpClient.Close();
                }

                return;
            }




            #endregion

            #region readout

            // burada readout isteme başlıyoruz. sayaç readout göndermiyor. readout u göndermesini biz istiyoruz. sayaç sadece bağlantı yapıyor. bağlandığında sayacın ne yapacağını biz yönetiyoruz.
            CommandFactory sm = new CommandFactory();
            CommandResponse[] res = null;

            if (tcpClient.Connected)
            {
                Logined = serverIslem.LoginIslemleri(0, header); // login işlemi ile ilgili kolay kolay değişiklik olmaz.

                if (!Logined)
                {
                    CevapYaz("Login hata \r\n" + "KonsIp : " + KonsIp + "\r\n" + headerPaket, ConsoleColor.Red);
                    return;
                }

                if (Logined) // login olduktan sonra komutları oluşturmaya başlıyoruz.
                {
                    List<AmiCommand> srvList = new List<AmiCommand>();

                    // ByteToBit state = new ByteToBit(Convert.ToInt32(serverIslem.KonsantratorPaket.State));
                    /*
                    if (state.Bit15 == 1) // ceza durumları kontrol ediliyor
                    {
                        tcpClient.Close();

                        #region stop watch

                        try
                        {
                            sw.Stop();
                            IslemSuresiTs = sw.Elapsed;
                            IslemSuresi = Convert.ToInt32(IslemSuresiTs.TotalSeconds);
                        }
                        catch (Exception ex)
                        {
                            IslemSuresi = 0;
                        }

                        #endregion

                        ServisResponseYaz(res, serverIslem.KonsantratorPaket, IslemSuresi, "");
                        return;
                    }

                    if (state.Bit1 == 1 || state.Bit3 == 1) // ceza durumu varsa ve alındıysa modem e cezaları sil diyoruz.
                    {
                        bool konsCeza = false;
                        bool panoCeza = false;
                        if (state.Bit1 == 1)
                        {
                            konsCeza = true;
                        }
                        if (state.Bit3 == 1)
                        {
                            panoCeza = true;
                        }
                        AmiCommand konsCezaDurumu = sm.ClearKonsPenalty("1", konsCeza, panoCeza);
                        srvList.Add(konsCezaDurumu);

                    }
                    */

                    CevapYaz("Login Ok..\r\n");



                    #region saat güncelleme
                    if (ZamanSenkronizasyonu) // modemin saati bozulduysa saati düzeltmek için saat komutu
                    {
                        DateTime simdi = DateTime.Now;
                        AmiCommand srvSaat = sm.SetKonsDateTime("402", simdi);
                        DateTime konsDateTime;
                        // konsantratör tarih saat hatalı ise direk saat servisini gönder. 
                        try
                        {
                            konsDateTime = Convert.ToDateTime(serverIslem.KonsantratorPaket.TarihSaat);
                            //konsantratör tarih saat şimdiki tarihten +- 40 sn farklı ise saat servisi gönder
                            TimeSpan ts = simdi.Subtract(konsDateTime);
                            if ((ts.TotalSeconds > ZamanTolerans) || (ts.TotalSeconds < (-1) * (ZamanTolerans)))
                                srvList.Add(srvSaat);
                            else if ((konsDateTime.Year != simdi.Year) || (konsDateTime.Month != simdi.Month) || (konsDateTime.Day != simdi.Day) ||
                                    (konsDateTime.Hour != simdi.Hour) || (konsDateTime.Minute != simdi.Minute))
                                srvList.Add(srvSaat);
                        }
                        catch (Exception ex)
                        {
                            srvList.Add(srvSaat);
                        }
                    }
                    #endregion

                    #region Db Servis Kontrol // db de bekleyen bir komut varsa onu kontrol ediyoruz.

                    try
                    {
                        CevapYaz("Db komut kontrol ediliyor: " + serverIslem.KonsantratorPaket.KonsSeriNo);
                        EfEntIsEmriDal entIsEmriDal = new EfEntIsEmriDal();
                        var list = entIsEmriDal.DetayGetir(new EntIsEmriAra { KonsSeriNo = serverIslem.KonsantratorPaket.KonsSeriNo, IsEmriDurumKayitNo = enumIsEmirleriDurum.Bekliyor.GetHashCode() });
                        if (list.Count > 0)
                        {
                            foreach (var item in list)
                            {
                                CevapYaz("Bulunan komut:" + item.SayacKayitNo + ": " + item.Parametre);
                                AmiCommand komut = new AmiCommand();
                                komut.Id = item.KayitNo.ToString();  // ServisIdUret().ToString();
                                                                     //komut.Kod = item.IsEmriKayitNo;
                                komut.Kod = Convert.ToInt32(item.IsEmriKod);
                                komut.Params = item.Parametre;
                                srvList.Add(komut);

                            }
                        }
                        else
                        {
                            CevapYaz("Komut Bulunamadı :" + serverIslem.KonsantratorPaket.KonsSeriNo);
                        }



                    }
                    catch (Exception ex)
                    {
                        CevapYaz(ex.Message);
                    }


                    #endregion

                    #region readout servisi

                    if (PROJEKOD == 1 || PROJEKOD == 99)
                    {
                        AmiCommand srvReadOut = sm.GetReadouts("401");

                        if (serverIslem.KonsantratorPaket.Versiyon.Contains("U") || serverIslem.KonsantratorPaket.Versiyon.Contains("M"))
                        {
                            if (hu != null && hu.Count > 0)
                            {
                                if (hu.Contains(serverIslem.KonsantratorPaket.KonsSeriNo.ToString()))
                                {
                                    string tablolar = Convert.ToString(hu[serverIslem.KonsantratorPaket.KonsSeriNo.ToString()]);
                                    srvReadOut = sm.GetReadoutsByTableNum("1", tablolar);
                                }
                            }
                        }

                        srvList.Add(srvReadOut);
                    }

                    #endregion
                    // kullanılmıyor.
                    #region FIRMWARE_UPDATE veya Close socket

                    if (!FIRMWARE_UPDATE)
                    {

                        #region close socket

                        AmiCommand srvcloseSocket = sm.CloseSocket("403");
                        srvList.Add(srvcloseSocket);

                        #endregion
                    }
                    else
                    {
                        #region firmware update

                        AmiCommand firmware = sm.ProgrammingMode("300");
                        srvList.Add(firmware);

                        #endregion
                    }

                    #endregion



                    #region servisleri gönder

                    AmiCommand[] Servisler = srvList.ToArray();

                    res = serverIslem.ServisGonder(Servisler);

                    #endregion // tüm dataları burada gönderiyoruz.

                    // haberleşme bitti timer ı durdur.
                    #region stop watch

                    try
                    {
                        sw.Stop();
                        IslemSuresiTs = sw.Elapsed;
                        IslemSuresi = Convert.ToInt32(IslemSuresiTs.TotalSeconds);
                    }
                    catch (Exception ex)
                    {
                        IslemSuresi = 0;
                    }

                    #endregion


                    ServisResponseYaz(res, serverIslem.KonsantratorPaket, IslemSuresi, KonsIp);
                }
            }

            #endregion

        }

        public void ServisResponseYaz(CommandResponse[] sonuc, Header konsPaket, int OkumaSuresi, string KonsIp)
        {
            try
            {
                string data = konsPaket.StrPaket;

                if (sonuc != null && sonuc.Length > 0)
                {
                    foreach (CommandResponse res in sonuc)
                    {
                        if (res == null) continue;
                        if (string.IsNullOrEmpty(res.Kod)) continue;

                        if (!hexCon.IsNumeric(res.Kod))
                        {

                            continue;
                        }

                        if (Convert.ToInt32(res.Kod) == CommandCode.CMD_SEND_READOUTS)
                        {
                            if (res.Readouts != null && res.Readouts.Length > 0)
                            {
                                List<string> list = new List<string>();

                                list.AddRange(res.Readouts);
                                //versiyonları R,U,M ile başlatınca aşağıdaki satır gibi düzenledik
                                if (konsPaket.Versiyon.Contains("R")) list.Reverse();

                                data += InsertDataOlustur(list);

                            }

                            CevapYaz(data);

                        }
                        else if (Convert.ToInt32(res.Kod) == CommandCode.CMD_GET_PULSE)
                        {
                            data += "|" + res.Pulse + "-" + res.NegatifPulse;
                        }
                        else if (Convert.ToInt32(res.Kod) == CommandCode.CMD_ERASE_FLASH)
                        {
                            CevapYaz(konsPaket.KonsSeriNo + " Güncelleme sonucu = " + res.GetSonuc(res.Sonuc));
                        }
                        else if (Convert.ToInt32(res.Kod) == CommandCode.CMD_CLOSE_SOCKET)
                        {

                        }
                        else if (Convert.ToInt32(res.Kod) == CommandCode.CMD_CLEAR_PENALTY)
                        {
                        }
                        else
                        {
                            try
                            {
                                //Sonuçlanan Komutlar Burada kayıt edilecek
                                CevapYaz("Response Id:" + res.Id + "- Response KodTanim: " + res.KodTanim + " - Cevap:" + res.Cevap+"  -Sonuç :"+res.Sonuc);
                                EfEntIsEmriDal entIsEmriDal = new EfEntIsEmriDal();
                                var isemri = entIsEmriDal.Getir(new EntIsEmriAra { KayitNo = Convert.ToInt32(res.Id) }).FirstOrDefault();
                                isemri.Cevap = res.Cevap;
                                isemri.IsEmriCevap = res.Sonuc;
                                isemri.GuncellemeTarih = DateTime.Now;
                                isemri.IsEmriDurumKayitNo = enumIsEmirleriDurum.Cevaplandi.GetHashCode();
                               
                                entIsEmriDal.Guncelle(isemri.ConvertEfList<EntIsEmri, EntIsEmriEf>());
                            }
                            catch (Exception ex)
                            {

                                CevapYaz(ex.Message);
                            }
                        }
                    }
                }

                try
                {
                    EntHamData rd = new EntHamData();
                    rd.Data = StrToByteArray(data);
                    rd.KonsSeriNo = konsPaket.KonsSeriNo;
                    rd.Ip = KonsIp;

                    // AmrData.GetInstance().Kuyruk.Enqueue(rd);
                    //RabbitMq kuyruga atılıyor readout dataları
                    RabbitmqHelper.AddQueue(rd);
                    //_log.Info("READOUT DATA: " + JsonConvert.SerializeObject(rd));
                    _log.Info("Konsserino:"+rd.KonsSeriNo+ "- Readout Data:"+data);
                }
                catch (Exception ex)
                {
                    _log.Error("Rabbitmq kuyruk yazma hata: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Servis Cevap Hata: " + ex.Message);
            }
        }

        #region Yardımcı Metotlar

        //konsola çıktı verilecekmi

        public bool output = true;
        static object lockObject = new object();
        public void CevapYaz(string text, ConsoleColor color = ConsoleColor.White)
        {
            if (output)
            {
                lock (lockObject)
                {
                    Console.ForegroundColor = color;
                    Console.WriteLine(text);
                }
            }
        }

        public string GetLocalIP()
        {
            string _IP = null;
            IPHostEntry _IPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress _IPAddress in _IPHostEntry.AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    _IP = _IPAddress.ToString();
                }
            }
            return _IP;
        }

        public bool HeaderCheck(string header)
        {
            bool sonuc = false;

            try
            {
                if (!string.IsNullOrEmpty(header))
                {
                    if (header.Contains(":"))
                    {
                        string[] hd = header.Split(':');
                        if (hd.Length >= 10) sonuc = true;
                        else sonuc = false;
                    }
                    else
                    {
                        sonuc = false;
                    }
                }
            }
            catch (Exception ex)
            {
                sonuc = false;
            }



            return sonuc;
        }

        public string InsertDataOlustur(List<string> data)
        {
            string dataPaket = "";

            if (data != null && data.Count > 0)
            {
                dataPaket += "|";

                foreach (string d in data)
                {
                    dataPaket += d + "|";
                }

                dataPaket = dataPaket.Remove(dataPaket.LastIndexOf("|"));
            }

            return dataPaket;
        }

        public string InsertDataOlustur(Header konsPaket, List<string> data)
        {
            string dataPaket = "";

            dataPaket += konsPaket.StrPaket + "|" + "\r\n";

            if (data != null && data.Count > 0)
            {
                foreach (string d in data)
                {
                    dataPaket += d + "|";
                }

                dataPaket = dataPaket.Remove(dataPaket.LastIndexOf("|"));
            }

            CevapYaz(dataPaket);

            return dataPaket;
        }

        public static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }
        private uint ServisIdUret()
        {
            System.Threading.Thread.Sleep(100);
            try
            {
                UInt32 sId = Convert.ToUInt32((DateTime.Now.Ticks / 100000) % (0xffffffff / 2));
                return sId;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion
    }
}
