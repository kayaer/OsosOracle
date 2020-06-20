using System;
using System.Collections.Generic;
using System.Text;

namespace Listener.Helpers
{
    public class CommandCode
    {
        #region servis kodları

        public const int CMD_ADD_METER = 100; // sayaclar tipi ile eklenir, ELM75635510, 

        public const int CMD_DEL_OBIS = 101; // obis kod silme

        public const int CMD_DEL_ALL_OBIS = 102; // tam komut "102,OBIS" ...

        public const int CMD_DEL_METER = 103; // sayaç silme

        public const int CMD_DEL_ALL_METERS = 104; // tam komut "104,METERS" ...

        public const int CMD_ERASE_FLASH = 105; // storage flash silinir ve programla islemi baslatilir, tam komut "105,FLASH"...

        public const int CMD_DEL_LISTEN_IP = 106; // ip adresi silme, "G,ip_addr", "G,88.255.31.98" ...

        public const int CMD_TUKETIM_SINIRI = 107; // endeksin ulabilecegi ust tuketim siniri, "107,123456"...

        public const int CMD_ADD_LISTEN_IP = 108; // ip adresi ekleme, "108,ip_addr", "108,88.255.31.98" ...

        public const int CMD_SEND_LISTEN_IP_LIST = 109; // ip list okuma, "109,GET_IP" ...

        public const int CMD_DEL_LISTEN_IP_LIST = 110; // tum ip leri siler, "110,DEL_IP" ...

        public const int CMD_METER_ROLE_CONTROL = 111; // role kontrolu icin, "111,ROLE_ON/ROLE_OFF"...

        public const int CMD_SMS_CONTROL = 112; // sms gonderme controlu, "112,PASIVE" pasif, "112,tel_no" aktif...

        public const int CMD_SET_DATE_TIME = 113; // saat guncelleme tarih saat h_gun, "113,08.10.2011 14:55:04 3" ...

        public const int CMD_ADD_OBIS = 114; // obis kod ekleme

        public const int CMD_READOUT_PERIOD = 115; // sayaca bağlantı periyodu 1:15 dakikada bir, 2:yarim saatte bir, 3: saatte bir, 4: gunde bir, 14 den sonrasi dakika cinsinden...

        public const int CMD_SEND_READOUTS = 116; // "116,READOUTS" ...

        public const int CMD_ADD_SERVER_IP = 118; // Tcp server ip

        public const int CMD_CONNECTION_PERIOD = 119; // Tcp servera bağlantı periyodu 1:15 dakikada bir, 2:yarim saatte bir, 3: saatte bir, 4: gunde bir, 14 den sonrasi dakika cinsinden...

        public const int CMD_DEL_SERVER_IP = 120; //Tcp server Ip sil

        public const int CMD_CLIENT_MODE = 121; // client mode aktif/pasif yapma, "121,ACTIVE/121,PASIVE"

        public const int CMD_SEND_METER_IDS = 122; // kayitli olan id leri gonderir, "122,GET_IDS", cevap "122,id1,id2,id3..." 

        public const int CMD_DEL_READOUTS = 123; // "123,READOUTS", tum readoutleri siler...

        public const int CMD_LOAD_PROFILE = 124; // "124,id,tarih1-tarih2", "124,ELM1234567,1111121230-1111122300"

        public const int CMD_INSTANT_READOUT = 125; // "125,id", servis geldigi anda optik okuma yapar ve okunan verileri gonderir.

        public const int CMD_SEND_CONFIG = 126; // konsantrator ayarlarını döndürür. Tcp Server ıp, Port ve client mode bilgileri döndürülüyor

        public const int CMD_SET_LISTEN_PORT = 117; // konsantratorün dinlediği port numarasını değiştirir.

        public const int CMD_SEND_OBIS_LIST = 127; // Filtrelenen Obis kod listesini döndürür

        public const int CMD_SEND_EVENT_LOGS = 128; // Log kayıtlarını döndürür. format: 13-12-2011-10-47;EventLogKodu;parametreler

        public const int CMD_SEND_SERVER_IP_LIST = 129;  // Tim server ip listesini getirir. indexleri ile beraber

        public const int CMD_RESET = 130; // reset komutu;

        public const int CMD_SET_APN = 131;  // "131,internet/statikip/..." ...

        public const int CMD_FACTORY_DEFAULT = 132; // "132,DEFAULT", tüm sistemi fabrika ayarlarina döndürür.

        public const int CMD_CLEAR_METER_PENALTY = 133; //  il ozel idaresi projesinde su sayacinin cezalarini temizler...

        public const int CMD_DELETE_EVENT_LOG = 134; // "134,DEL_LOG", event loglari silmek icin... 

        public const int CMD_CONSANTRATOR_ROLE = 135; // "135,ROLE_ON/ROLE_OFF,role_no(1/2),timeout", "135,ROLE_ON,1,0", timeout 0 ise surekli, sifir disinda(16bit sayi) ise ms cinsinden bekleme... 

        public const int CMD_CLEAR_PENALTY = 136; // "136,sayi", bit0 konsantrator kapak temizleme, bit1 pano kapak ceza temizleme... 

        public const int CMD_SET_METER_DATETIME = 137; // "137,id,datetime", 

        public const int CMD_SET_METER_TARIFF_SEGMENT = 138; // "138,id,gun,tarife", gun : '0' haftaici, '1' cumartesi, '2' pazar...

        public const int CMD_SET_METER_TARIFF_HOUR = 139; // "139,id,gun,tarife", gun : '0' haftaici, '1' cumartesi, '2' pazar...

        public const int CMD_ADD_SCHEDULE_TASK = 140; // "140,task1,task2...", task sirasi |ay|hgun|gun|saat|dakika|mod|task|...

        public const int CMD_DEL_SCHEDULE_TASK = 141; // "141,ALL_TASK:Tümünü sil, T00:Bağlantı türü görevleri sil, T10: Readout türü görevleri sil, T20: Tüm röle türü görevleri sil, T21 : 1 numaralı röle görevini sil, 
                                                      //   141,index/task_no,index/task_no..." Görev nosuna göre sil

        public const int CMD_GET_SCHEDULE_TASK = 142; // "142,GET_SCH" ...

        public const int CMD_CLOSE_SOCKET = 143; // "143,CLOSE" ...

        public const int CMD_SET_ETHERNET_CONFIG = 144; // "144,ip,mask,gateway"...

        public const int CMD_DEL_OBIS_TABLE = 145; // Format : "Servis_id,145,Tablo_no" Örnek : "12345,145,8"  Haberleşme ünitesinde tanımlanmış olan obis tablosunu silmek için kullanılır.

        public const int CMD_SEND_OBIS_TABLE = 146; // Format : "Servis_id,146,Tablo_no" Örnek : "12345,146,6"  Haberleşme ünitesinde tanımlanmış olan obis tablosunu silmek için kullanılır.
                                                    //"12345,146,0,6,EMPTY"  "12345,146,0,6,0.0.0,1.8.0 ..."

        public const int CMD_SEND_LISTEN_PORT = 147; // Örnek : "12345,147,GET_PORT" cevap : "12345,147,0,80"

        public const int CMD_SEND_GPRS_APN = 148; //Örnek : "12345,148,GET_APN"  cevap  Örnek2 : "12345,148,0,statikip,user,*"

        public const int CMD_SEND_TUKETIM_SINIRI = 149; //  Örnek : "12345,149,GET_TUKETIM"  cevap Örnek1 : "12345,149,0,123456"  DSİ projesinde kullanılan tüketim limit değerini okumak için kullanılır.

        public const int CMD_SEND_ALL_OBIS_TABLE = 150; // Obis kod tablo listesini döndürür

        public const int CMD_SET_CUSTOMER_CODE = 151; // Müşteri kodu set edilir

        public const int CMD_SET_METER_CREDIT = 152; // "152,id,kredi"  dsi_kuyu_su projesine ozel... kredi = m3 olarak 

        public const int CMD_SELF_TEST = 153; // Servis_id,153,SELF_TEST  rsc ultraya ozel... 

        public const int CMD_TRANSPARENT_MODE = 154; // 154,Ch(0-1)-baud(0-6)-stop(1-2)-party(N/E/O)-bitsize(7-8)-idletimeout(10-300)sec 

        public const int CMD_SET_PULSE = 155;  // "155,value/X,neg_value/X" ... CARREFOUR PROJESINE OZEL KOMUTLAR

        public const int CMD_GET_PULSE = 156;  // "156,GET_VALUE" ...  CARREFOUR PROJESINE OZEL KOMUTLAR

        public const int CMD_RF_ADD_CREDIT = 159;  // "159,SayacId,Kedi" ...  RF SU SAYACINA ÖZEL KOMUT

        public const int CMD_SET_AUTHORIZE = 161;  // "161,SayacId" ...  MCM SU SAYACINA ÖZEL KOMUT

        public const int CMD_RESET_CREDIT = 162;  // "162,0" ...  MCM SU SAYACINA ÖZEL KOMUT

        // public const int CMD_ILLUM_CONTROL = 163;  // ""163,ILLUM_OFF/ZONE" ... AYDINLATMA KONTROL
        public const int CMD_GET_PERIOD = 163;  // ""163,ILLUM_OFF/ZONE" ... AYDINLATMA KONTROL

        public const int CMD_DEBUG = 164;  // "164,OFF_HOFF_M" 164,0130 ... AYDINLATMA YÜZDE ELLİ SAATİ

        // public const int CMD_ILLUM_FIFTY_HOURS = 164;  // "164,OFF_HOFF_M" 164,0130 ... AYDINLATMA YÜZDE ELLİ SAATİ

        public const int CMD_ILLUM_UPDATE_TABLE = 165;  // "165,TABLE_SIZE" ... AYDINLATMA TABLO GÜNCELLEME

        public const int CMD_PRICE_LOG = 301;  // "GAZ BİRİM FİYAT LOGLARI

        public const int CMD_RF_SET_FIYAT = 170; // "fiyat1, fiyat2, fiyat3, fiyat4, fiyat5"

        public const int CMD_RF_SET_LIMIT = 171;  // "limit1, limit2, limit3, limit4"

        public const int CMD_RF_SET_BAYRAM = 172;  // "bayram1Gun, bayram1Ay, bayram1Sure, bayram2Gun, bayram2Ay, bayram2Sure"

        public const int CMD_RF_SET_KALIBRE = 173;  // "kalibre1, kalibre2, kalibre3, kalibre4, kalibre5"

        public const int CMD_RF_SET_AYARLAR1 = 174;  // ""

        public const int CMD_RF_SET_AYARLAR2 = 175;  // ""

        public const int CMD_RF_READ_LOG = 176;  // ""

        public const int CMD_RF_SEND_LOG = 177;  // ""

        public const int CMD_HIBRID_FATURA = 166;

        public const int CMD_HIBRID_ONODEME = 167;
        #endregion
    }
}
