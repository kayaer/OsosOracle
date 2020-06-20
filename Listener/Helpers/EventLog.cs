using System;
using System.Collections.Generic;
using System.Text;

namespace Listener.Helpers
{
    [Serializable]
    public class EventLog
    {
        public string _Log;
        public string Tarih;
        public string Id;
        public string Aciklama;
        public string KanalKod;
        public string Kanal;
        public string Params;

        public EventLog() { }

        public EventLog(string Log)
        {
            _Log = Log;
            string[] s = Log.Split(';');

            Tarih = s[0];
            Id = s[1];
            Params = s[2];
            Aciklama = GetAciklama(Id);

            if (s.Length == 4)
            {
                KanalKod = s[3];
                Kanal = GetKanal(KanalKod);
            }
        }


        public string GetAciklama(string Id)
        {
            #region log kodları
            /*
            EVENT_PROGRAMMING                1
            EVENT_SERVER_IP_ADD              2
            EVENT_SERVER_IP_DEL              3
            EVENT_LISTEN_IP_ADD              4
            EVENT_LISTEN_IP_DEL              5
            EVENT_LISTEN_PORT_CHANGE         6
            EVENT_CONNECTION_PERIOD_CHANGE   7
            EVENT_CLIENT_MODE_CHANGE         8
            EVENT_READOUT_PEROID_CHANGE      9
            EVENT_DELETE_LISTEN_IP_LIST      10
            EVENT_DELETE_METER_LIST          11
            EVENT_LOGIN_FAILED               12
            EVENT_RESET                      13
            EVENT_ROLE_PROCESS               14
            EVENT_PENALTY                    15
            EVENT_CONSOLE_LOGIN              16
            EVENT_FACTORY_DEFAULT            17
            EVENT_GPRS_APN_CHANGE            18
            EVENT_REJECTED_IP                19
            EVENT_GPRS_SIGNAL_ERR            20
            EVENT_POWER                      21
            EVET_CONSANTRATOR_ROLE           22
            EVENT_RTC_SET                    23
            EVENT_CONNECTION_ERROR           24
            EVENT_DELETE_LOG                 25
            */
            #endregion

            #region log kodları yorumlama

            string Aciklama = "Bilinmeyen EventLog Kodu";
            switch (Id)
            {
                case "1":
                    Aciklama = "Versiyon güncelleme";
                    break;
                case "2":
                    Aciklama = "Server Ip eklendi";
                    break;
                case "3":
                    Aciklama = "Server Ip silindi";
                    break;
                case "4":
                    Aciklama = "Listen Ip eklendi";
                    break;
                case "5":
                    Aciklama = "Listen Ip silindi";
                    break;
                case "6":
                    Aciklama = "Listen port değişti";
                    break;
                case "7":
                    Aciklama = "Servera bağlantı periyodu değişti";
                    break;
                case "8":
                    Aciklama = "Client mode değişti";
                    break;
                case "9":
                    Aciklama = "Readout periyodu değişti";
                    break;
                case "10":
                    Aciklama = "Listen Ip'ler silindi";
                    break;
                case "11":
                    Aciklama = "Tüm sayaçlar silindi";
                    break;
                case "12":
                    Aciklama = "Login başarısız";
                    break;
                case "13":
                    Aciklama = "Sistem Reset aldı";
                    break;
                case "14":
                    Aciklama = "Röle işlemi yapıldı";
                    break;
                case "15":
                    Aciklama = "Ceza durumu(DSI projesi için)";
                    break;
                case "16":
                    Aciklama = "Konsoldan login";
                    break;
                case "17":
                    Aciklama = "Fabrika yarlarına dönüldü";
                    break;
                case "18":
                    Aciklama = "APN değişti";
                    break;
                case "19":
                    Aciklama = "Rejected IP";
                    break;
                case "20":
                    Aciklama = "Gprs Sinyal Hatası";
                    break;
                case "21":
                    Aciklama = "Elektriksel olay";
                    break;
                case "22":
                    Aciklama = "Röle/Vana işlemi";
                    break;
                case "23":
                    Aciklama = "Saat ayarlama işlemi";
                    break;
                case "24":
                    Aciklama = "Servera bağlantı hatası";
                    break;
                case "25":
                    Aciklama = "Event Log silme işlemi";
                    break;
                case "26":
                    Aciklama = "Sayaç eklendi";
                    break;
                case "27":
                    Aciklama = "Sayaç silindi";
                    break;
                case "28":
                    Aciklama = "Obis kod eklendi";
                    break;
                case "29":
                    Aciklama = "Obis kod silindi";
                    break;
                case "30":
                    Aciklama = "Tüm obis tabloları silindi";
                    break;
                case "31":
                    Aciklama = "Obis tablosu silindi";
                    break;
                case "32":
                    Aciklama = "Tüm obis tabloları gönderildi";
                    break;
                case "33":
                    Aciklama = "Obis tablosu gönderildi";
                    break;
                case "34":
                    Aciklama = "Listen port gönderildi";
                    break;
                case "35":
                    Aciklama = "Sayaç röle işlemi";
                    break;
                case "36":
                    Aciklama = "Tüketim sınırı set edildi";
                    break;
                case "37":
                    Aciklama = "Tüketim sınırı gönderildi";
                    break;
                case "38":
                    Aciklama = "Sayaç ceza temizleme";
                    break;
                case "39":
                    Aciklama = "Listen Ip list gönderildi";
                    break;
                case "40":
                    Aciklama = "Readoutlar silindi";
                    break;
                case "41":
                    Aciklama = "Sayaç listesi gönderildi";
                    break;
                case "42":
                    Aciklama = "Event loglar gönderildi";
                    break;
                case "43":
                    Aciklama = "Server Ip listesi gönderildi";
                    break;
                case "44":
                    Aciklama = "Reset komutu alındı";
                    break;
                case "45":
                    Aciklama = "Apn gönderildi";
                    break;
                case "46":
                    Aciklama = "Konsantrator cezası temizlendi";
                    break;
                case "47":
                    Aciklama = "Zamanlanmış görev eklendi";
                    break;
                case "48":
                    Aciklama = "Zamanlanmış görev silindi";
                    break;
                case "49":
                    Aciklama = "Zamanlanmış görevler gönderildi";
                    break;
                case "50":
                    Aciklama = "Zone kodu set edildi";
                    break;
                default:
                    Aciklama = "Bilinmeyen EventLog Kodu";
                    break;
            }

            #endregion

            return Aciklama;
        }

        public string GetKanal(string KanalKod)
        {
            #region log kodları
            /*
            EVENT_PROGRAMMING                1
            EVENT_SERVER_IP_ADD              2
            EVENT_SERVER_IP_DEL              3
            EVENT_LISTEN_IP_ADD              4
            */
            #endregion

            #region kanal kodları yorumlama

            string Kanal = "Bilinmeyen Kanal Kodu";
            switch (KanalKod)
            {
                case "0":
                    Kanal = "NULL";
                    break;
                case "1":
                    Kanal = "GPRS/PSTN";
                    break;
                case "2":
                    Kanal = "RS485";
                    break;
                case "3":
                    Kanal = "ETHERNET";
                    break;
                default:
                    Kanal = "Bilinmeyen Kanal Kodu";
                    break;
            }

            #endregion

            return Kanal;
        }

        public override string ToString()
        {
            return "Tarih:" + Tarih + "," + "Id:" + Id + "," + "Açıklama:" + Aciklama + "," + "Parametre:" + Params + "," + "Kanal:" + Kanal;
        }
    }
}
