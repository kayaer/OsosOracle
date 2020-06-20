using System;
using System.Collections.Generic;
using System.Text;

namespace Listener.Helpers
{
    [Serializable]
    public class CommandResponse
    {


        public string Id;
        public string Kod;
        public string KodTanim;
        public string Sonuc;
        public string ServisCevap;
        public string SayacId;
        public string ServisResponseStr;
        public string Cevap;

        public EventLog[] EventLogs;
        public string[] ListenIpList;
        public string[] ServerIpList;
        public Config Config;
        public string[] ObisKodList;
        public string[] ObisEkleSonucKodList;
        public string[] ObisSilSonucKodList;
        public string[] SayacList;
        public string SayacRoleDurum;
        public string KonsInputlar;
        public string[] Readouts;
        public string AnlikReadout;
        public string YukProfili;
        public string[] SayacEkleCikar;
        public string TarihSet;
        public string SaatSet;
        public string HGunSet;
        public string TarifeSaatSet;
        public string TarifeDilimSet;
        public string[] ZamanlanmisGorevEkleSonuc;
        public string ZamanlanmisGorevSilSonuc;
        public string[] ZamanlanmisGorevler;

        public string ListenPort = "";
        public string TuketimLimit = "";
        public string ApnAd = "";
        public string ApnUser = "";
        public string ApnPass = "";
        public string[] ObisTabloList;
        public string VersiyonGuncellemeSonuc = "";
        public string[] CezaList;

        public string SelfTestSonuc = "";

        public string Pulse = "";
        public string NegatifPulse = "";

        public string gazBirimFiyatlar;
        public string fiyatlar;
        public string limit;
        public string bayram;
        public string kalibre;
        public string ayarlar1;
        public string ayarlar2;
        public string readLog;
        public string sendLog;
        public string setApn;
        public string sendServerIp;
        public string AddCreditForDsi;
        public string AddCredit;
        public string FaturaMod;
        public string OnOdemeMod;

        public CommandResponse() { }

        public CommandResponse(string ServisId, int ServisKod, string ServisSonuc)
        {
            Id = ServisId.ToString();
            Kod = ServisKod.ToString();
            Sonuc = ServisSonuc;

            ServisCevap = GetSonuc(Sonuc, ServisKod);
            KodTanim = GetKodTanim(Kod);
        }

        public CommandResponse(string srv)
        {
            ServisResponseStr = srv;
            try
            {
                string[] servis = srv.Split(',');

                Id = servis[0];
                Kod = servis[1];
                Sonuc = servis[2];

                if (servis.Length > 3)
                {
                    Cevap = servis[3];
                }

                ServisCevap = GetSonuc(Sonuc, Convert.ToInt32(Kod));
                KodTanim = GetKodTanim(Kod);

                #region event logs

                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_SEND_EVENT_LOGS))
                {
                    #region
                    //123456,113,0:2297939197,128,0,16-12-2011-11-10;2;88.255.31.105,16-12-2011-11-10;2;88.255.31.98,16-12-2011-11-10;2;88.255.31.104,16-12-2011-11-00;13;2,16-12-2011-11-00;13;2,16-12-2011-11-00;13;4,15-12-2011-20-00;13;4,15-12-2011-19-45;13;4,15-12-2011-19-45;13;4,15-12-2011-18-15;13;2,71-82-2042-35-20;13;2,73-82-2042-90-20;13;22,73-82-2042-90-20;13;4,73-82-2042-92-75;13;4,73-82-2042-92-75;13;2,14-12-2011-16-04;3;88.255.31.104,14-12-2011-15-54;2;88.255.31.105,14-12-2011-15-54;2;88.255.31.104,14-12-2011-15-54;2;88.255.31.98,14-12-2011-11-40;7;2:12345622,103,0
                    // sample event log : 2297939197,128,0,16-12-2011-11-10;2;88.255.31.105,16-12-2011-11-10;2;88.255.31.98,16-12-2011-11-10;2;88.255.31.104,16-12-2011-11-00;13;2,16-12-2011-11-00;13;2,16-12-2011-11-00;13;4,15-12-2011-20-00;13;4,15-12-2011-19-45;13;4,15-12-2011-19-45;13;4,15-12-2011-18-15;13;2,71-82-2042-35-20;13;2,73-82-2042-90-20;13;22,73-82-2042-90-20;13;4,73-82-2042-92-75;13;4,73-82-2042-92-75;13;2,14-12-2011-16-04;3;88.255.31.104,14-12-2011-15-54;2;88.255.31.105,14-12-2011-15-54;2;88.255.31.104,14-12-2011-15-54;2;88.255.31.98,14-12-2011-11-40;7;2
                    #endregion
                    List<EventLog> elList = new List<EventLog>();

                    for (int i = 3; i < servis.Length; i++)
                    {
                        EventLog log = new EventLog(servis[i]);
                        elList.Add(log);
                    }

                    EventLogs = elList.ToArray();
                }
                #endregion

                #region Listen Ip list
                else
                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_SEND_LISTEN_IP_LIST))
                {
                    List<string> lisIpList = new List<string>();

                    for (int i = 3; i < servis.Length; i++)
                    {
                        lisIpList.Add(servis[i]);
                    }

                    ListenIpList = lisIpList.ToArray();
                }

                #endregion

                #region Server Ip List
                else
                    if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_SEND_SERVER_IP_LIST))
                {
                    List<string> serIpList = new List<string>();

                    for (int i = 3; i < servis.Length; i++)
                    {
                        serIpList.Add(servis[i]);
                    }

                    ServerIpList = serIpList.ToArray();
                }

                #endregion

                #region config
                else
                        if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_SEND_CONFIG))
                {
                    string cfg = "";
                    for (int i = 3; i < servis.Length; i++)
                    {
                        cfg += servis[i] + ",";

                    }
                    cfg = cfg.Remove(cfg.LastIndexOf(","));

                    Config = GetConfig(cfg);
                }
                #endregion

                #region obis kod list
                else
                            if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_SEND_OBIS_LIST))
                {
                    List<string> obisList = new List<string>();

                    for (int i = 3; i < servis.Length; i++)
                    {
                        obisList.Add(servis[i]);
                    }

                    ObisKodList = obisList.ToArray();
                }
                #endregion

                #region obis ekle sonuc kod list
                else
                                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_ADD_OBIS))
                {
                    List<string> obisList = new List<string>();

                    for (int i = 3; i < servis.Length; i++)
                    {
                        obisList.Add(servis[i]);
                    }

                    ObisEkleSonucKodList = obisList.ToArray();
                }
                #endregion

                #region obis sil sonuc kod list
                else
                                    if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_DEL_OBIS))
                {
                    List<string> obisList = new List<string>();

                    for (int i = 3; i < servis.Length; i++)
                    {
                        obisList.Add(servis[i]);
                    }

                    ObisSilSonucKodList = obisList.ToArray();
                }
                #endregion

                #region obis table kod list
                else
                                        if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_SEND_OBIS_TABLE))
                {
                    List<string> obisList = new List<string>();

                    for (int i = 3; i < servis.Length; i++)
                    {
                        obisList.Add(servis[i]);
                    }

                    ObisKodList = obisList.ToArray();
                }
                #endregion

                #region obis table list
                else
                                            if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_SEND_ALL_OBIS_TABLE))
                {
                    string[] obs = srv.Split('\n');

                    List<string> obisTabloList = new List<string>();

                    for (int i = 1; i < obs.Length; i++)
                    {
                        obisTabloList.Add(obs[i]);
                    }

                    ObisTabloList = obisTabloList.ToArray();
                }

                #endregion

                #region sayaç list
                else
                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_SEND_METER_IDS))
                {
                    List<string> sayacList = new List<string>();

                    for (int i = 3; i < servis.Length; i++)
                    {
                        sayacList.Add(servis[i]);
                    }

                    SayacList = sayacList.ToArray();
                }

                #endregion

                #region sayaç röle durum
                else
                                                    if ((Sonuc == "0" || Sonuc == "9") && (Convert.ToInt32(Kod) == CommandCode.CMD_METER_ROLE_CONTROL))
                {
                    SayacRoleDurum = servis[3];
                }

                #endregion

                #region kons röle durum
                else
                                                        if ((Sonuc == "0" || Sonuc == "9") && (Convert.ToInt32(Kod) == CommandCode.CMD_CONSANTRATOR_ROLE))
                {
                    KonsInputlar = servis[3];
                }

                #endregion

                #region Yük profili
                else
                                                            if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_LOAD_PROFILE))
                {
                    SayacId = servis[3];
                }

                #endregion

                #region Sayaç Ekleme Çıkarma işlemi
                else
                                                                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_ADD_METER || Convert.ToInt32(Kod) == CommandCode.CMD_DEL_METER))
                {
                    List<string> syList = new List<string>();
                    for (int i = 3; i < servis.Length; i++)
                    {
                        string sy = servis[i];
                        syList.Add(sy);
                    }
                    SayacEkleCikar = syList.ToArray();
                }

                #endregion

                #region Sayaç Tarih Saat
                else
                                                                    if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_SET_METER_DATETIME))
                {
                    TarihSet = servis[3];
                    SaatSet = servis[4];
                    HGunSet = servis[5];
                }

                #endregion

                #region Sayaç Tarife Saat Set
                else
                                                                        if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_SET_METER_TARIFF_HOUR))
                {
                    TarifeSaatSet = servis[3];
                }

                #endregion

                #region Sayaç Tarife Dilim Set
                else
                                                                            if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_SET_METER_TARIFF_SEGMENT))
                {
                    TarifeDilimSet = servis[3];
                }

                #endregion

                #region Zamanlanmış görevler

                #region ekle
                else
                                                                                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_ADD_SCHEDULE_TASK))
                {
                    List<string> zamGorEkleList = new List<string>();
                    for (int i = 3; i < servis.Length; i++)
                    {
                        string zg = servis[i];
                        zamGorEkleList.Add(zg);
                    }
                    ZamanlanmisGorevEkleSonuc = zamGorEkleList.ToArray();
                }

                #endregion

                #region sil
                else
                                                                                    if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_DEL_SCHEDULE_TASK))
                {
                    ZamanlanmisGorevSilSonuc = servis[2];
                }

                #endregion

                #region oku
                else
                                                                                        if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_GET_SCHEDULE_TASK))
                {
                    List<string> zamGorList = new List<string>();
                    for (int i = 3; i < servis.Length; i++)
                    {
                        string zg = servis[i];
                        zamGorList.Add(zg);
                    }
                    ZamanlanmisGorevler = zamGorList.ToArray();
                }

                #endregion

                #endregion

                #region Listenport
                else
                                                                                            if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_SEND_LISTEN_PORT))
                {
                    ListenPort = servis[3];
                }
                #endregion

                #region apn parametreleri
                else
                                                                                                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_SEND_GPRS_APN))
                {
                    ApnAd = servis[3];
                    ApnUser = servis[4];
                    ApnPass = servis[5];
                }

                #endregion

                #region TuketimLimit
                else
                                                                                                    if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_SEND_TUKETIM_SINIRI))
                {
                    TuketimLimit = servis[3];
                }
                #endregion

                #region Kons / Pano Kapak Ceza temizleme
                else
                                                                                                        if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_CLEAR_PENALTY))
                {
                    List<string> cezalist = new List<string>();
                    for (int i = 3; i < servis.Length; i++)
                    {
                        string sy = servis[i];

                        string[] syDizi = sy.Split('-');

                        string sn = "";

                        if (syDizi[0] == "1") sn += "Konsantrator Ceza - ";
                        else if (syDizi[0] == "2") sn += "Pano Kapak Ceza - ";

                        if (syDizi[1] == "0") sn += "Başarılı";
                        else if (syDizi[1] == "C") sn += "Başarısız";


                        cezalist.Add(sn);
                    }
                    CezaList = cezalist.ToArray();
                }
                #endregion

                #region Self Test
                else
                                                                                                            if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_SELF_TEST))
                {
                    SelfTestSonuc = servis[3];
                }
                #endregion

                #region Get Pulse
                else
                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_GET_PULSE))
                {
                    Pulse = servis[3];
                    NegatifPulse = servis[4];
                }
                #endregion

                #region Gaz Birim Fiyatlar
                else
                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_PRICE_LOG))
                {
                    gazBirimFiyatlar = servis[3];
                }
                #endregion



                //Deneme
                else
                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_RF_SET_FIYAT))
                {
                    fiyatlar = servis[3];
                }

                else
                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_RF_SET_LIMIT))
                {
                    limit = servis[3];
                }

                else
                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_RF_SET_BAYRAM))
                {
                    bayram = servis[3];
                }

                else
                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_RF_SET_KALIBRE))
                {
                    kalibre = servis[3];
                }

                else
                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_RF_SET_AYARLAR1))
                {
                    ayarlar1 = servis[3];
                }

                else
                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_RF_SET_AYARLAR2))
                {
                    ayarlar2 = servis[3];
                }

                else
                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_RF_READ_LOG))
                {
                    readLog = servis[3];
                }

                else
                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_RF_SEND_LOG))
                {
                    sendLog = servis[3];
                }
                else
                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_SET_APN))
                {
                    setApn = servis[3];
                }
                else
                if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_ADD_SERVER_IP))
                {
                    sendServerIp = servis[3];
                }
                else
                    if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_SET_METER_CREDIT))
                {
                    AddCreditForDsi = servis[3];
                }
                else
                    if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_RF_ADD_CREDIT))
                {
                    AddCredit = servis[3];
                }
                else
                    if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_HIBRID_FATURA))
                {
                    FaturaMod = servis[3];
                }
                else
                    if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_HIBRID_ONODEME))
                {
                    OnOdemeMod = servis[3];
                }
                else
                    if ((Sonuc == "0") && (Convert.ToInt32(Kod) == CommandCode.CMD_GET_PERIOD))
                {
                    Cevap = srv;
                }

                //deneme
            }
            catch (Exception ex)
            {

            }

        }

        public string GetSonuc(string cevapKod)
        {
            return GetSonuc(cevapKod, 0);
        }

        public string GetSonuc(string cevapKod, int komutKodu)
        {
            #region servis cevap kodları
            /*
            SERVIS PAKETI HATALI    '-2'
            SUCCESS                 '0'
            ERR_NOT_FOUND           '1'
            ERR_ALREADY_EXIST       '2'
            ERR_OVERFLOW            '3'
            ERR_UNKNOWN_COMMAND     '4'
            ERR_NOT_STRING          '5'
            ERR_NOT_NUMBER_VALUE    '6'
            ERR_PROGRAMMING_MODE    '7'
            ERR_NOT_SUPPORT_TYPE    'A' LOAD PROFİL CEVAP
            ERR_VALF_FAILURE        'B'
            ERR_COMM_CHANNEL        'B'
            FAIL                    'C'
            */
            #endregion

            #region Cevap kodları yorumlama

            string Aciklama = "Bilinmeyen Servis Cevap Kodu";
            switch (cevapKod)
            {
                case "":
                    if (komutKodu == 131)
                        Aciklama = "Başarılı";
                    else
                        Aciklama = "Bilinmeyen Servis Cevap Kodu";
                    break;
                case "-2":
                    Aciklama = "Gönderilen servis paketi hatalı";
                    break;
                case "-1":
                    Aciklama = "Başarısız";
                    break;
                case "0":
                    Aciklama = "Başarılı";
                    break;
                case "1":
                    Aciklama = "İşlem yapmak istediğiniz sayaç konsantratöre kayıtlı değil";
                    break;
                case "2":
                    Aciklama = "İşlem yapmak istediğiniz sayaç konsantratöre zaten Kayıtlı";
                    break;
                case "3":
                    Aciklama = "Maximum sayıya ulaşılmış. Register edilemedi.";
                    break;
                case "4":
                    Aciklama = "Bilinmeyen Komut";
                    break;
                case "5":
                    Aciklama = "String bilgi hatası";
                    break;
                case "6":
                    Aciklama = "Numerik bilgi hatası";
                    break;
                case "7":
                    Aciklama = "Sayaçla haberleşme yok";
                    break;
                case "8":
                    Aciklama = "Yanlış Komut";
                    break;
                case "9":
                    Aciklama = "Röle/Vana kontrol işlemi başarısız";
                    break;
                case "A":
                    Aciklama = "Desteklenmeyen Tip(Yük profili okunamıyor)";
                    break;
                case "B":
                    Aciklama = "Kanal Hatası";
                    break;
                case "C":
                    Aciklama = "Başarısız";
                    break;
                default:
                    Aciklama = "Bilinmeyen Servis Cevap Kodu";
                    break;
            }

            #endregion

            return Aciklama;
        }

        public string GetKodTanim(string servisKod)
        {
            string Aciklama = "Bilinmeyen servis kodu";
            switch (Convert.ToInt32(servisKod))
            {
                case CommandCode.CMD_ADD_LISTEN_IP:
                    Aciklama = "Listen Ip ekleme";
                    break;
                case CommandCode.CMD_ADD_METER:
                    Aciklama = "Sayaç ekleme";
                    break;
                case CommandCode.CMD_ADD_OBIS:
                    Aciklama = "Obis kod ekleme";
                    break;
                case CommandCode.CMD_ADD_SERVER_IP:
                    Aciklama = "Server Ip ekleme";
                    break;
                case CommandCode.CMD_CLIENT_MODE:
                    Aciklama = "Client mode değiştirme";
                    break;
                case CommandCode.CMD_CONNECTION_PERIOD:
                    Aciklama = "Server bağlantı periyodu";
                    break;
                case CommandCode.CMD_INSTANT_READOUT:
                    Aciklama = "Anlık readout okuma";
                    break;
                case CommandCode.CMD_DEL_ALL_METERS:
                    Aciklama = "Tüm sayaçları sil";
                    break;
                case CommandCode.CMD_DEL_ALL_OBIS:
                    Aciklama = "Tüm obis kod filtresini sil";
                    break;
                case CommandCode.CMD_DEL_LISTEN_IP:
                    Aciklama = "Listen Ip sil";
                    break;
                case CommandCode.CMD_DEL_LISTEN_IP_LIST:
                    Aciklama = "Tüm Listen Ip'leri sil";
                    break;
                case CommandCode.CMD_DEL_METER:
                    Aciklama = "Sayaç sil";
                    break;
                case CommandCode.CMD_DEL_OBIS:
                    Aciklama = "Obis kod sil";
                    break;
                case CommandCode.CMD_DEL_READOUTS:
                    Aciklama = "Kayıtlı readoutları sil";
                    break;
                case CommandCode.CMD_DEL_SERVER_IP:
                    Aciklama = "Server Ip sil";
                    break;
                case CommandCode.CMD_ERASE_FLASH:
                    Aciklama = "Programming mode";
                    break;
                case CommandCode.CMD_SEND_METER_IDS:
                    Aciklama = "Kayıtlı sayaç listesi";
                    break;
                case CommandCode.CMD_LOAD_PROFILE:
                    Aciklama = "Yük profili okuma";
                    break;
                case CommandCode.CMD_READOUT_PERIOD:
                    Aciklama = "Sayaç okuma periyodu";
                    break;
                case CommandCode.CMD_RESET:
                    Aciklama = "Konsantratör reset";
                    break;
                case CommandCode.CMD_METER_ROLE_CONTROL:
                    Aciklama = "Sayaç Röle/Vana kontrol";
                    break;
                case CommandCode.CMD_SEND_CONFIG:
                    Aciklama = "Konfigürasyon bilgileri";
                    break;
                case CommandCode.CMD_SEND_EVENT_LOGS:
                    Aciklama = "Konsantratör olay kayıtları";
                    break;
                case CommandCode.CMD_SEND_LISTEN_IP_LIST:
                    Aciklama = "Listen Ip listesi";
                    break;
                case CommandCode.CMD_SEND_OBIS_LIST:
                    Aciklama = "Filtrelenen Obis kod listesi";
                    break;
                case CommandCode.CMD_SEND_READOUTS:
                    Aciklama = "Readoutları getir";
                    break;
                case CommandCode.CMD_SEND_SERVER_IP_LIST:
                    Aciklama = "Server Ip listesi";
                    break;
                case CommandCode.CMD_SET_DATE_TIME:
                    Aciklama = "Konsantratör tarih saat ayarı";
                    break;
                case CommandCode.CMD_SET_LISTEN_PORT:
                    Aciklama = "Listen port değiştir";
                    break;
                case CommandCode.CMD_SMS_CONTROL:
                    Aciklama = "Sms gönderme";
                    break;
                case CommandCode.CMD_TUKETIM_SINIRI:
                    Aciklama = "Tüketim sınırı değiştir";
                    break;
                case CommandCode.CMD_SET_APN:
                    Aciklama = "APN değiştirme";
                    break;
                case CommandCode.CMD_FACTORY_DEFAULT:
                    Aciklama = "Fabrika ayarları";
                    break;
                case CommandCode.CMD_CLEAR_METER_PENALTY:
                    Aciklama = "Sayaç Ceza Temizleme";
                    break;
                case CommandCode.CMD_CLEAR_PENALTY:
                    Aciklama = "Kons / Pano Kapak Ceza Temizleme";
                    break;
                case CommandCode.CMD_DELETE_EVENT_LOG:
                    Aciklama = "Konsantratör olay kayıtları sil";
                    break;
                case CommandCode.CMD_CONSANTRATOR_ROLE:
                    Aciklama = "Konsantrator Röle kontrol";
                    break;
                case CommandCode.CMD_SET_METER_DATETIME:
                    Aciklama = "Sayaç tarih saat ayarı";
                    break;
                case CommandCode.CMD_SET_METER_TARIFF_HOUR:
                    Aciklama = "Sayaç tarife saat ayarı";
                    break;
                case CommandCode.CMD_SET_METER_TARIFF_SEGMENT:
                    Aciklama = "Sayaç tarife dilim ayarı";
                    break;
                case CommandCode.CMD_ADD_SCHEDULE_TASK:
                    Aciklama = "Zamanlanmış görev ekleme";
                    break;
                case CommandCode.CMD_DEL_SCHEDULE_TASK:
                    Aciklama = "Zamanlanmış görev silme";
                    break;
                case CommandCode.CMD_GET_SCHEDULE_TASK:
                    Aciklama = "Zamanlanmış görevleri okuma";
                    break;
                case CommandCode.CMD_CLOSE_SOCKET:
                    Aciklama = "Bağlantı kapat";
                    break;
                case CommandCode.CMD_SET_ETHERNET_CONFIG:
                    Aciklama = "Ethernet Ip Ayarları";
                    break;
                case CommandCode.CMD_DEL_OBIS_TABLE:
                    Aciklama = "Obis tablo sil";
                    break;
                case CommandCode.CMD_SEND_OBIS_TABLE:
                    Aciklama = "Obis tablo getir";
                    break;
                case CommandCode.CMD_SEND_LISTEN_PORT:
                    Aciklama = "Listen Port";
                    break;
                case CommandCode.CMD_SEND_GPRS_APN:
                    Aciklama = "Apn bilgileri";
                    break;
                case CommandCode.CMD_SEND_TUKETIM_SINIRI:
                    Aciklama = "Tüketim sınırı getir";
                    break;
                case CommandCode.CMD_SEND_ALL_OBIS_TABLE:
                    Aciklama = "Obis tablo listesi";
                    break;
                case CommandCode.CMD_SET_CUSTOMER_CODE:
                    Aciklama = "Zone kodu set";
                    break;
                case CommandCode.CMD_SET_METER_CREDIT:
                    Aciklama = "DSI Su Sayacı Kredi Yükle";
                    break;
                case CommandCode.CMD_SELF_TEST:
                    Aciklama = "Self Test";
                    break;
                case CommandCode.CMD_TRANSPARENT_MODE:
                    Aciklama = "Transparan mode";
                    break;
                case CommandCode.CMD_SET_PULSE:
                    Aciklama = "Pulse set";
                    break;
                case CommandCode.CMD_GET_PULSE:
                    Aciklama = "Get pulse";
                    break;
                case CommandCode.CMD_RF_ADD_CREDIT:
                    Aciklama = "RF Su Sayacı Kredi Yükle";
                    break;
                //case CommandCode.CMD_ILLUM_CONTROL:
                //    Aciklama = "Aydınlatma kontrol";
                //    break;
                //case CommandCode.CMD_ILLUM_FIFTY_HOURS:
                //    Aciklama = "Aydınlatma yuzde elli saat";
                //    break;
                case CommandCode.CMD_ILLUM_UPDATE_TABLE:
                    Aciklama = "Aydınlatma tablo güncelle";
                    break;
                case CommandCode.CMD_PRICE_LOG:
                    Aciklama = "Gaz birim fiyat tablosu";
                    break;
                case CommandCode.CMD_RF_SET_FIYAT:
                    Aciklama = "Su sayacı fiyatları";
                    break;
                case CommandCode.CMD_RF_SET_LIMIT:
                    Aciklama = "Su sayacı limitleri";
                    break;
                case CommandCode.CMD_RF_SET_BAYRAM:
                    Aciklama = "Su sayacı bayram değerleri";
                    break;
                case CommandCode.CMD_RF_SET_KALIBRE:
                    Aciklama = "Su sayacı kalibre değerleri";
                    break;
                case CommandCode.CMD_RF_SET_AYARLAR1:
                    Aciklama = "Su sayacı parametreleri";
                    break;
                case CommandCode.CMD_RF_SET_AYARLAR2:
                    Aciklama = "Su sayacı parametreleri";
                    break;
                case CommandCode.CMD_RF_READ_LOG:
                    Aciklama = "Log İsteme";
                    break;
                case CommandCode.CMD_RF_SEND_LOG:
                    Aciklama = "Log Alma";
                    break;
                case CommandCode.CMD_HIBRID_FATURA:
                    Aciklama = "Faturalı Moda Geç";
                    break;
                case CommandCode.CMD_HIBRID_ONODEME:
                    Aciklama = "Ön Ödemeli Moda Geç";
                    break;
                default:
                    Aciklama = "";
                    break;
            }

            return Aciklama;
        }

        public Config GetConfig(string cfg)
        {
            Config c = null;

            c = new Config(cfg);
            return c;
        }
    }
}
