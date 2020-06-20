using System;
using System.Collections.Generic;
using System.Text;

namespace Listener.Helpers
{
    public class CommandFactory
    {

        byte[] _KomutPaket;

        public byte[] KomutPaket
        {
            get
            {
                return _KomutPaket;
            }
            set
            {
                _KomutPaket = value;
            }
        }

        #region Paket hazırlama fonksiyonları

        public byte[] KomutPaketHazirla(AmiCommand srv)
        {

            try
            {
                char[] sv = srv.ToString().ToCharArray();
                _KomutPaket = new byte[sv.Length];
                //_ServisPaket = new byte[sv.Length + 2];
                //_ServisPaket[0] = (byte)(sv.Length >> 8);
                //_ServisPaket[1] = (byte)(sv.Length);

                for (int i = 0; i < _KomutPaket.Length; i++)
                {
                    _KomutPaket[i] = (byte)sv[i];
                }

                return _KomutPaket;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public byte[] KomutPaketHazirla(AmiCommand[] srvlList)
        {

            try
            {
                string servisler = "";
                foreach (AmiCommand srv in srvlList)
                {
                    servisler += srv.ToString() + ":";
                }
                servisler = servisler.Remove(servisler.LastIndexOf(":"));

                char[] sv = servisler.ToCharArray();
                _KomutPaket = new byte[sv.Length];


                for (int i = 0; i < _KomutPaket.Length; i++)
                {
                    _KomutPaket[i] = (byte)sv[i];
                }

                return _KomutPaket;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public string KomutIdUret()
        {
            System.Threading.Thread.Sleep(100);
            try
            {
                UInt32 sId = Convert.ToUInt32((DateTime.Now.Ticks / 1000000) % 0xffffffff);
                return sId.ToString();
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        /// <summary>
        /// Elle Servis Hazırlama
        /// </summary>
        /// <param name="CommandId">CommandId</param>
        /// <param name="CommandCode">CommandCode</param>
        /// <param name="ServisParams">Servis Parametreleri</param>
        /// <returns></returns>
        public AmiCommand ServisHazirla(string CommandId, Int32 CommandCode, string ServisParams)
        {
            return new AmiCommand(CommandId, CommandCode, ServisParams);
        }

        #endregion

        #region Konsantrator Ayar servisleri

        /// <summary>
        /// Zone kodunu set eder. Müşterileri takip için eklendi.
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="Zone">String Zone kodu(4 karakter)</param>
        /// <returns></returns>
        public AmiCommand SetZone(string CommandId, string Zone)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SET_CUSTOMER_CODE, Zone);
        }

        /// <summary>
        /// Konsantratörü versiyon güncelleme moduna getirir
        /// </summary>
        /// <param name="CommandId">CommandId</param>
        /// <returns></returns>
        public AmiCommand ProgrammingMode(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_ERASE_FLASH, "FLASH");
        }

        /// <summary>
        /// Konsantratörün tarih ve saatini set eder
        /// </summary>
        /// <param name="CommandId">CommandId</param>
        /// <param name="dt">Tarih Saat</param>
        /// <returns></returns>
        public AmiCommand SetKonsDateTime(string CommandId, DateTime dt)
        {
            try
            {
                return new AmiCommand(CommandId, CommandCode.CMD_SET_DATE_TIME, dt.Day.ToString().PadLeft(2, '0') + dt.Month.ToString().PadLeft(2, '0') + dt.Year + dt.Hour.ToString().PadLeft(2, '0') + dt.Minute.ToString().PadLeft(2, '0') + dt.Second.ToString().PadLeft(2, '0') + (int)dt.DayOfWeek);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Konsantartor client modu aktif eder. Belirlenen periyotta Tcp Servera bağlanmaya başlar.
        /// </summary>
        /// <param name="CommandId">CommandId</param>
        /// <returns></returns>
        public AmiCommand ActivateClientMode(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_CLIENT_MODE, "ACTIVE");
        }

        /// <summary>
        /// Konsantartor server moda geçer. Bağlantı beklemeye başlar.
        /// </summary>
        /// <param name="CommandId">CommandId</param>
        /// <returns></returns>
        public AmiCommand ActivateServerMode(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_CLIENT_MODE, "PASIVE");
        }

        /// <summary>
        /// Konsantratörün ayarlarını döndürür. 
        /// </summary>
        /// <param name="CommandId">CommandId</param>
        /// <returns></returns>
        public AmiCommand GetConfig(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SEND_CONFIG, "CONFIG");
        }

        /// <summary>
        /// Olay kayıtlarını getirir
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand GetEventLogs(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SEND_EVENT_LOGS, "EVENT_LOGS");
        }

        /// <summary>
        /// Olay kayıtlarını siler
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand DelEventLogs(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_DELETE_EVENT_LOG, "DEL_LOG");
        }

        /// <summary>
        /// Konsantratöre soft reset atar
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand Reset(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_RESET, "RESET");
        }

        /// <summary>
        /// Fabrika ayarlarına getirir
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand FactoryDefault(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_FACTORY_DEFAULT, "DEFAULT");
        }

        /// <summary>
        /// APN(Access Point Name) parametresini set eder. Mini Rsc modeli için geçerli bir servistir.
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="Apn">Apn adı</param>
        /// <returns></returns>
        public AmiCommand SetAPN(string CommandId, string ApnAd)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SET_APN, ApnAd);
        }

        /// <summary>
        /// APN(Access Point Name) parametrelerini set eder. Rsc Ultra ve M3 modelleri için geçerli bir servistir
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="ApnAd">Apn adı</param>
        /// <param name="User">Varsa Apn kullanıcı adı, yoksa * girilir</param>
        /// <param name="Pass">Varsa Apn şifresi, yoksa * girilir</param>
        /// <returns></returns>
        public AmiCommand SetAPN(string CommandId, string ApnAd, string User, string Pass)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SET_APN, ApnAd + "," + User + "," + Pass);
        }

        /// <summary>
        /// Apn parametrelerini getir
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand GetAPNUltra(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SEND_GPRS_APN, "GET_APN");
        }

        /// <summary>
        /// Konsantratör röle aç
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="RoleNum">1 veya 2</param>
        /// <param name="TimeOut">ms cinsinden geciktirme süresi</param>
        /// <returns></returns>
        public AmiCommand KonsRoleOn(string CommandId, string RoleNum, string TimeOut)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_CONSANTRATOR_ROLE, "ROLE_ON," + RoleNum + "," + TimeOut);
        }

        /// <summary>
        /// Konsantratör röle kapat
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="RoleNum">1 veya 2</param>
        /// <param name="TimeOut">ms cinsinden geciktirme süresi</param>
        /// <returns></returns>
        public AmiCommand KonsRoleOff(string CommandId, string RoleNum)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_CONSANTRATOR_ROLE, "ROLE_OFF," + RoleNum);
        }

        /// <summary>
        /// Kons. Ceza ve Pano Kapak ceza bitlerini temizler
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="KonsPenalty">Konsantratör kapak cezasını temizle</param>
        /// <param name="CoverPenalty">Pano kapak cezasını temizle</param>
        /// <returns></returns>
        public AmiCommand ClearKonsPenalty(string CommandId, bool KonsPenalty, bool CoverPenalty)
        {
            int ceza = 0;
            if (KonsPenalty) ceza |= 1;
            if (CoverPenalty) ceza |= 2;

            return new AmiCommand(CommandId, CommandCode.CMD_CLEAR_PENALTY, ceza.ToString());
        }

        /// <summary>
        /// Zamanlanmış görev ekle
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="GorevKod">Görev kodu (Örn : 9999999988051000)</param>
        /// <returns></returns>
        public AmiCommand AddJob(string CommandId, string GorevKod)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_ADD_SCHEDULE_TASK, GorevKod);
        }

        /// <summary>
        /// Zamanlanmış görev sil
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="Param">Görev silme parametreleri (Örn : T00 Tüm bağlantı tipi görevleri siler)</param>
        /// <returns></returns>
        public AmiCommand DelJob(string CommandId, string Param)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_DEL_SCHEDULE_TASK, Param);
        }

        /// <summary>
        /// Tüm zamanlanmış görevleri sil
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand DelAllJob(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_DEL_SCHEDULE_TASK, "ALL_TASK");
        }

        /// <summary>
        /// Tüm zamanlanmış görevleri getir
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand ReadJobs(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_GET_SCHEDULE_TASK, "GET_SCH");
        }

        /// <summary>
        /// Soket bağlantısını kapatır
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand CloseSocket(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_CLOSE_SOCKET, "CLOSE");
        }

        /// <summary>
        /// Ethernet konsantratöre Ip ayarlarını set eder
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="Ip">Network Ip</param>
        /// <param name="Mask">Ağ maskesi</param>
        /// <param name="Gateway">Ağ geçidi</param>
        /// <returns></returns>
        public AmiCommand SetEthernetConfig(string CommandId, string Ip, string Mask, string Gateway)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SET_ETHERNET_CONFIG, Ip + "," + Mask + "," + Gateway);
        }

        /// <summary>
        /// Listen port numarasını getir
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand GetListenPort(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SEND_LISTEN_PORT, "GET_PORT");
        }

        /// <summary>
        /// Konsantratör self test
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand SelfTest(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SELF_TEST, "SELF_TEST");
        }


        /// <summary>
        /// Konsantratör Transparan Moda Geçirme
        /// </summary>      
        public AmiCommand TransparanMode(string ServisId, string Kanal, string Baudrate, string StopBit, string Parity, string Bitsize, int timeout)
        {
            return new AmiCommand(ServisId, CommandCode.CMD_TRANSPARENT_MODE, Kanal + "-" + Baudrate + "-" + StopBit + "-" + Parity + "-" + Bitsize + "-" + timeout);
        }

        #endregion

        #region Sayaç Servisleri

        /// <summary>
        /// Sayaç ekle
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="MeterId">Sayaç Flag + Sayaç Seri numarası(ELM20122021)</param>
        /// <returns></returns>
        public AmiCommand AddMeter(string CommandId, string MeterId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_ADD_METER, MeterId);
        }

        /// <summary>
        /// Sayaç ekleme yeni hali
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="MeterId">Sayaç Flag + Sayaç Seri numarası(ELM20122021)</param>
        /// <param name="FlagSend">FlagSend : Sayaç okuma esnasında Flag karakterinin gönderilip gönderilmeyeceğini tanımlar. '0' gönderilmeyecek, '1' gönderilecek demektir.</param>
        /// <param name="Kanal">Sayaç okuma kanalını tanımlar, '0' RS485, '1' RS232</param>
        /// <param name="StartBaudrate">StartBaud : Sayaç okumaya başlama baudrate' ini tanımlar. 
		///'*' : default demektir. (300baudrate)
		///'0' : 300 baudrate
		///'1' : 600 baudrate
		///'2' : 1200 baudrate
		///'3' : 2400 baudrate
		///'4' : 4800 baudrate
		///'5' : 9600 baudrate
        ///'6' : 19200 baudrate</param>
        /// <param name="SwitchBaudrate">SwitchBaud : Sayaç okuma esnasında değiştirilen baudrate 'i tanımlar. 
		///'*' : default demektir. Sayaç tarafından desteklenen baudrate kullanılır.
        ///Diğer baudrate tanımlamaları StartBaudrate gibidir.</param>
        /// <param name="ReadMode">ReadMode : Sayaç okuma modunu tanımlar. '0' Readout mode, '1' Partial readout mod</param>
        /// <returns></returns>
        public AmiCommand AddMeterElk(string CommandId, string MeterId, string FlagSend, string Kanal, string StartBaudrate, string SwitchBaudrate, string ReadMode)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_ADD_METER, MeterId + "-" + FlagSend + "-" + Kanal + "-" + StartBaudrate + "-" + SwitchBaudrate + "-" + ReadMode);
        }

        /// <summary>
        /// Sayaç sil
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="MeterId">Sayaç Flag + Sayaç Seri numarası(ELM20122021)</param>
        /// <returns></returns>
        public AmiCommand DelMeter(string CommandId, string MeterId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_DEL_METER, MeterId);
        }

        /// <summary>
        /// Tüm sayaçları sil
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand DelAllMeters(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_DEL_ALL_METERS, "METERS");
        }

        /// <summary>
        /// Kayıtlı sayaç listesini getir
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand GetMeterIdList(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SEND_METER_IDS, "GET_IDS");
        }

        /// <summary>
        /// Çoklu sayaç ekle
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="MeterIdList">Sayaç Flag + Sayaç Seri numarası(ELM20122021)</param>
        /// <returns></returns>
        public AmiCommand AddMeterByMeterIds(string CommandId, string[] MeterIdList)
        {
            string param = "";
            foreach (string s in MeterIdList)
            {
                param += s.Trim() + ",";
            }
            param = param.Remove(param.LastIndexOf(","));

            return new AmiCommand(CommandId, CommandCode.CMD_ADD_METER, param);
        }

        /// <summary>
        /// Çoklu sayaç sil
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="MeterIdList">Sayaç Flag + Sayaç Seri numarası(ELM20122021)</param>
        /// <returns></returns>
        public AmiCommand DelMeterByMeterIds(string CommandId, string[] MeterIdList)
        {
            string param = "";
            foreach (string s in MeterIdList)
            {
                param += s.Trim() + ",";
            }
            param = param.Remove(param.LastIndexOf(","));

            return new AmiCommand(CommandId, CommandCode.CMD_DEL_METER, param);
        }

        /// <summary>
        /// EC25 sayaç röle on
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="MeterId">Sayaç Flag + Sayaç Seri numarası(ELM20122021)</param>
        /// <returns></returns>
        public AmiCommand MeterRoleOn(string CommandId, string MeterId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_METER_ROLE_CONTROL, "ROLE_ON," + MeterId);
        }

        /// <summary>
        /// EC25 sayaç röle off
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="MeterId"></param>
        /// <returns></returns>
        public AmiCommand MeterRoleOff(string CommandId, string MeterId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_METER_ROLE_CONTROL, "ROLE_OFF," + MeterId);
        }

        /// <summary>
        /// EC25 sayaç röle on
        /// </summary>
        /// <param name="ServisId"></param>
        /// <param name="MeterId"></param>
        /// <returns></returns>
        public AmiCommand MeterRoleOn(string ServisId, string MeterId, string FlagSend, string Kanal, string StartBaudrate, string SwitchBaudrate, string ReadMode)
        {
            return new AmiCommand(ServisId, CommandCode.CMD_METER_ROLE_CONTROL, "ROLE_ON," + MeterId + "-" + FlagSend + "-" + Kanal + "-" + StartBaudrate + "-" + SwitchBaudrate + "-" + ReadMode);
        }

        /// <summary>
        /// EC25 sayaç röle off
        /// </summary>
        /// <param name="ServisId"></param>
        /// <param name="MeterId"></param>
        /// <returns></returns>
        public AmiCommand MeterRoleOff(string ServisId, string MeterId, string FlagSend, string Kanal, string StartBaudrate, string SwitchBaudrate, string ReadMode)
        {
            return new AmiCommand(ServisId, CommandCode.CMD_METER_ROLE_CONTROL, "ROLE_OFF," + MeterId + "-" + FlagSend + "-" + Kanal + "-" + StartBaudrate + "-" + SwitchBaudrate + "-" + ReadMode);
        }


        /// <summary>
        /// EC25 sayaç tarih saat set
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="MeterId">Sayaç Flag + Sayaç Seri numarası(ELM20122021)</param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public AmiCommand SetMeterDateTime(string CommandId, string MeterId, DateTime dt)
        {
            try
            {
                return new AmiCommand(CommandId, CommandCode.CMD_SET_METER_DATETIME, MeterId + "," + dt.Day.ToString().PadLeft(2, '0') + dt.Month.ToString().PadLeft(2, '0') + dt.Year + dt.Hour.ToString().PadLeft(2, '0') + dt.Minute.ToString().PadLeft(2, '0') + dt.Second.ToString().PadLeft(2, '0') + (int)dt.DayOfWeek);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// İl Özel idaresi için su sayacı ceza temizleme servisi
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand ClearWaterMeterPenalty(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_CLEAR_METER_PENALTY, "CLEAR");
        }

        /// <summary>
        ///  EC25 sayaç tarife saatlerini set 
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="MeterId">Sayaç Flag + Sayaç Seri numarası(ELM20122021)</param>
        /// <param name="Gun">'0' haftaici, '1' cumartesi, '2' pazar</param>
        /// <param name="TarifeSaatleri"></param>
        /// <returns></returns>
        public AmiCommand SetMeterTarifHours(string CommandId, string MeterId, string Day, string TarifHours)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SET_METER_TARIFF_HOUR, MeterId + "," + Day + "," + TarifHours);
        }

        /// <summary>
        ///  EC25 sayaç tarife dilimlerini set
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="SayacId">Sayac Id</param>
        /// <param name="Gun">'0' haftaici, '1' cumartesi, '2' pazar</param>
        /// <param name="TarifeDilimleri"></param>
        /// <returns></returns>
        public AmiCommand SetMeterTarifSegments(string CommandId, string MeterId, string Day, string TarifSegments)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SET_METER_TARIFF_SEGMENT, MeterId + "," + Day + "," + TarifSegments);
        }

        #endregion

        #region Obis Servisleri

        /// <summary>
        /// Obis kod ekle
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="ObisCode">Obis kod (Örn : 0.0.0)</param>
        /// <returns></returns>
        public AmiCommand AddObisCode(string CommandId, string ObisCode)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_ADD_OBIS, ObisCode);
        }

        /// <summary>
        /// Obis kod sil
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="ObisCode">Obis kod (Örn : 0.0.0)</param>
        /// <returns></returns>
        public AmiCommand DelObisCode(string CommandId, string ObisCode)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_DEL_OBIS, ObisCode);
        }

        /// <summary>
        /// Tüm obis kodları sil
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand DelAllObisCodes(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_DEL_ALL_OBIS, "OBIS");
        }

        /// <summary>
        /// Obis kod listesini getir
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand GetObisCodeList(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SEND_OBIS_LIST, "OBIS_LIST");
        }

        /// <summary>
        /// Çoklu obis kod ekle
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="ObisCodeList">Obis kod (Örn : 0.0.0)</param>
        /// <returns></returns>
        public AmiCommand AddObisCodes(string CommandId, string[] ObisCodeList)
        {
            string param = "";
            foreach (string kod in ObisCodeList)
            {
                param += kod.Trim() + ",";
            }
            param = param.Remove(param.LastIndexOf(","));

            return new AmiCommand(CommandId, CommandCode.CMD_ADD_OBIS, param);
        }

        /// <summary>
        /// Çoklu obis kod sil
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="ObisCodeList">Obis kod (Örn : 0.0.0)</param>
        /// <returns></returns>
        public AmiCommand DelObisCodes(string CommandId, string[] ObisCodeList)
        {
            string param = "";
            foreach (string kod in ObisCodeList)
            {
                param += kod.Trim() + ",";
            }
            param = param.Remove(param.LastIndexOf(","));

            return new AmiCommand(CommandId, CommandCode.CMD_DEL_OBIS, param);
        }

        //rsc ultrada eklendiler 07.06.2012
        /// <summary>
        /// Tüm Obis tablo listesini kodları ile birlikte getir
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand GetObisTableList(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SEND_ALL_OBIS_TABLE, "OBIS_LIST");
        }

        /// <summary>
        /// Obis tablo listesini sil
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="TableNum">Tablo numarası</param>
        /// <returns></returns>
        public AmiCommand DelObisTable(string CommandId, string TableNum)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_DEL_OBIS_TABLE, TableNum);
        }

        /// <summary>
        /// Belirtilen Obis tablosundaki kodları getir
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="TableNum">Tablo numarası</param>
        /// <returns></returns>
        public AmiCommand GetObisTable(string CommandId, string TableNum)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SEND_OBIS_TABLE, TableNum);
        }

        /// <summary>
        /// Belirtilen Obis tablosuna kod ekle
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="TableNum">Tablo numarası</param>
        /// <param name="ObisCode">Obis kodu</param>
        /// <returns></returns>
        public AmiCommand AddObisCodeByTableNum(string CommandId, string TableNum, string ObisCode)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_ADD_OBIS, TableNum + "," + ObisCode);
        }

        /// <summary>
        /// Belirtilen Obis tablosuna çoklu kod ekle
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="TableNum">Tablo numarası</param>
        /// <param name="ObisCodeList">Obis kod listesi</param>
        /// <returns></returns>
        public AmiCommand AddObisCodesByTableNum(string CommandId, string TableNum, string[] ObisCodeList)
        {
            string param = "";
            foreach (string kod in ObisCodeList)
            {
                param += kod.Trim() + ",";
            }
            param = param.Remove(param.LastIndexOf(","));

            return new AmiCommand(CommandId, CommandCode.CMD_ADD_OBIS, TableNum + "," + param);
        }

        /// <summary>
        /// Belirtilen Obis tablosundan kod sil
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="TableNum">Tablo numarası</param>
        /// <param name="ObisCode">Obis kodu</param>
        /// <returns></returns>
        public AmiCommand DelObisCodeByTableNum(string CommandId, string TableNum, string ObisCode)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_DEL_OBIS, TableNum + "," + ObisCode);
        }

        /// <summary>
        /// Belirtilen Obis tablosundan çoklu kod sil
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="TableNum">Tablo numarası</param>
        /// <param name="ObisCodeList">Obis kod listesi</param>
        /// <returns></returns>
        public AmiCommand DelObisCodesByTableNum(string CommandId, string TableNum, string[] ObisCodeList)
        {
            string param = "";
            foreach (string kod in ObisCodeList)
            {
                param += kod.Trim() + ",";
            }
            param = param.Remove(param.LastIndexOf(","));

            return new AmiCommand(CommandId, CommandCode.CMD_DEL_OBIS, TableNum + "," + param);
        }



        #endregion

        #region Network Ayar Servisleri

        #region listen mod fonksiyonları

        /// <summary>
        /// Listen modda: konsantratorün dinlediği port numarasını değiştirir.
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="Port"></param>
        /// <returns></returns>
        public AmiCommand SetListenPort(string CommandId, string Port)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SET_LISTEN_PORT, Port);
        }

        /// <summary>
        ///  Listen modda : konsantratöre bağlanabilecek belirtilen Listen Ip'yi ekler
        /// </summary>
        /// <param name="CommandId">CommandId</param>
        /// <param name="Ip">Listen Ip</param>
        /// <returns></returns>
        public AmiCommand AddListenIp(string CommandId, string Ip)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_ADD_LISTEN_IP, Ip);
        }

        /// <summary>
        /// Listen modda : konsantratöre bağlanabilecek belirtilen Listen Ip'yi siler
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="Ip">Listen Ip</param>
        /// <returns></returns>
        public AmiCommand DelListenIp(string CommandId, string Ip)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_DEL_LISTEN_IP, Ip);
        }

        /// <summary>
        /// Listen modda : konsantratöre bağlanabilecek tüm Ip'leri siler
        /// </summary>
        /// <param name="CommandId">CommandId</param>
        /// <returns></returns>
        public AmiCommand DelAllListenIp(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_DEL_LISTEN_IP_LIST, "DEL_IP");
        }

        /// <summary>
        /// Listen modda : konsantratöre bağlanabilecek Ip'lerin listesini döndürür
        /// </summary>
        /// <param name="CommandId">CommandId</param>
        /// <returns></returns>
        public AmiCommand GetListenIpList(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SEND_LISTEN_IP_LIST, "GET_IP");
        }

        /// <summary>
        /// Listen modda : konsantratöre bağlanabilecek belirtilen Listen Ip'leri ekler
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="Iplist"></param>
        /// <returns></returns>
        public AmiCommand AddListenIps(string CommandId, string[] Iplist)
        {
            string param = "";
            foreach (string s in Iplist)
            {
                param += s.Trim() + ",";
            }
            param = param.Remove(param.LastIndexOf(","));

            return new AmiCommand(CommandId, CommandCode.CMD_ADD_LISTEN_IP, param);
        }

        /// <summary>
        /// Listen modda : konsantratöre bağlanabilecek tüm Ip'leri siler
        /// </summary>
        /// <param name="CommandId">CommandId</param>
        /// <returns></returns>
        public AmiCommand DelListenIps(string CommandId, string[] Iplist)
        {
            string param = "";
            foreach (string s in Iplist)
            {
                param += s.Trim() + ",";
            }
            param = param.Remove(param.LastIndexOf(","));

            return new AmiCommand(CommandId, CommandCode.CMD_DEL_LISTEN_IP, param);
        }

        #endregion

        #region client mod fonksiyonları

        /// <summary>
        /// Konsantratorun sıra ile bağlanacağı Tcp Server makinanın Ip ve portunu konsantratöre ekler.
        /// </summary>
        /// <param name="CommandId">CommandId</param>
        /// <param name="Index">Bağlanacak Ip indexi(1,2,3)</param>
        /// <param name="TCPServerIp">Eklenecek Server Ip</param>
        /// <param name="TCPServerPort">Eklenecek Server Port</param>
        /// <returns></returns>
        public AmiCommand AddServerIpPort(string CommandId, string index, string TCPServerIp, string TCPServerPort)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_ADD_SERVER_IP, index + "-" + TCPServerIp + "-" + TCPServerPort);
        }

        /// <summary>
        /// Client modda : konsantratörün bağlanacağı Server Ip'lerinden belirtilen Ip yi siler
        /// </summary>
        /// <param name="CommandId">CommandId</param>
        /// <param name="TCPServerIp">Silinecek Server Ip</param>
        /// <returns></returns>
        public AmiCommand DelServerIpPort(string CommandId, string TCPServerIp)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_DEL_SERVER_IP, TCPServerIp);
        }

        /// <summary>
        /// Client modda : konsantratöre kayıtlı olan tüm Server Iplerinin listesini döndürür
        /// </summary>
        /// <param name="CommandId">CommandId</param>
        /// <returns></returns>
        public AmiCommand GetServerIpList(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SEND_SERVER_IP_LIST, "GET_IP");
        }

        /// <summary>
        /// Konsantratorun sıra ile bağlanacağı Tcp Server makinanın Ip ve portunu konsantratöre ekler.
        /// </summary>
        /// <param name="CommandId">CommandId</param>
        /// <param name="Index">Bağlanacak Ip indexi(1,2,3)</param>
        /// <param name="TCPServerIp">Eklenecek Server Ip</param>
        /// <param name="TCPServerPort">Eklenecek Server Port</param>
        /// <returns></returns>
        public AmiCommand AddServerIps(string CommandId, string[] Iplist)
        {
            string param = "";
            foreach (string s in Iplist)
            {
                param += s.Trim() + ",";
            }
            param = param.Remove(param.LastIndexOf(","));

            return new AmiCommand(CommandId, CommandCode.CMD_ADD_SERVER_IP, param);
        }

        /// <summary>
        /// Client modda : konsantratörün bağlanacağı Server Ip'lerinden belirtilen Ip yi siler
        /// </summary>
        /// <param name="CommandId">CommandId</param>
        /// <param name="TCPServerIp">Silinecek Server Ip</param>
        /// <returns></returns>
        public AmiCommand DelServerIps(string CommandId, string[] Iplist)
        {
            string param = "";
            foreach (string s in Iplist)
            {
                string[] t = s.Trim().Split('-');
                param += t[1] + ",";
            }
            param = param.Remove(param.LastIndexOf(","));

            return new AmiCommand(CommandId, CommandCode.CMD_DEL_SERVER_IP, param);
        }

        #endregion

        #endregion

        #region Periyot Servisleri

        /// <summary>
        /// Tcp Server'a kaç dakikada bir bağlanılacak
        /// </summary>
        /// <param name="Periyot">1:15 dk'da bir, 2:yarım saatte bir, 3:saatte bir, 4:günde bir, 14 den sonra gönderilen dakika cinsinden</param>
        /// <returns></returns>
        public AmiCommand SetConnectionPeriodToTCPServer(string CommandId, string Periyot)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_CONNECTION_PERIOD, Periyot);
        }
        /// <summary>
        /// Sayaca kaç dakikada bir bağlanılacak
        /// </summary>
        /// <param name="Periyot">1:15 dk'da bir, 2:yarım saatte bir, 3:saatte bir, 4:günde bir, 14 den sonra gönderilen dakika cinsinden</param>
        /// <returns></returns>
        public AmiCommand SetReadOutPeriyotToMeter(string CommandId, string Periyot)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_READOUT_PERIOD, Periyot);
        }

        #endregion

        #region Özel Servisler

        /// <summary>
        /// Dsi projesinde tüketim sınırını geçince sms gönderme işlemini pasifleştirir
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="TelNo"></param>
        /// <returns></returns>
        public AmiCommand SmsPasif(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SMS_CONTROL, "PASIVE");
        }

        /// <summary>
        /// Dsi projesinde tüketim sınırını geçince sms gönderilir belirtilen numaraya
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="TelNo"></param>
        /// <returns></returns>
        public AmiCommand SmsAktif(string CommandId, string TelNo) //0
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SMS_CONTROL, TelNo);
        }

        #region DSI projesine özel servisler

        /// <summary>
        /// Tüketim sınırını set eder
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="Limit">Wh olarak tüketim sınırı</param>
        /// <returns></returns>
        public AmiCommand SetConsumptionLimit(string CommandId, string Limit)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_TUKETIM_SINIRI, Limit);
        }

        /// <summary>
        /// Tüketim sınırını getir
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand GetConsumptionLimit(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SEND_TUKETIM_SINIRI, "GET_TUKETIM");
        }

        /// <summary>
        /// DSI su sayacına kredi yükler
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="MeterId">Sayaç Flag + Sayaç Seri numarası(ELM20122021)</param>
        /// <param name="Kredi">Metre küp olarak kredi miktarı</param>
        /// <returns></returns>
        public AmiCommand DsiSuKrediYukle(string CommandId, string MeterId, string KrediM3)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SET_METER_CREDIT, MeterId + "," + KrediM3);
        }

        /// <summary>
        /// DSI su sayacına kredi yükler
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="MeterId">Sayaç Flag + Sayaç Seri numarası(ELM20122021)</param>
        /// <param name="Kredi">Metre küp olarak kredi miktarı</param>
        /// <param name="KapanmaTarihi">KapanmaTarihi</param>
        /// <returns></returns>
        public AmiCommand DsiSuKrediYukle(string CommandId, string MeterId, string KrediM3, DateTime KapanmaTarihi)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SET_METER_CREDIT, MeterId + "," + KrediM3 + "," + KapanmaTarihi.Day.ToString().PadLeft(2, '0') + KapanmaTarihi.Month.ToString().PadLeft(2, '0') + KapanmaTarihi.Year + KapanmaTarihi.Hour.ToString().PadLeft(2, '0') +
                                  KapanmaTarihi.Minute.ToString().PadLeft(2, '0') + KapanmaTarihi.Second.ToString().PadLeft(2, '0') + (int)KapanmaTarihi.DayOfWeek);
        }

        #endregion

        #region Carrefour SA

        /// <summary>
        /// Modemin okuduğu pulse değerlerini getirir
        /// </summary>
        /// <param name="ServisId"></param>
        /// <returns></returns>
        public AmiCommand GetPulse(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_GET_PULSE, "GET_VALUE");
        }

        /// <summary>
        /// Sayacın pulse değerlerini set eder
        /// </summary>
        /// <param name="ServisId"></param>
        /// <param name="Pulse">Pulse değeri. Set edilmeyecekse X gönderilir.</param>
        /// <param name="NegatifPulse">Negatif Pulse değeri. Set edilmeyecekse X gönderilir.</param>
        /// <returns></returns>
        public AmiCommand SetPulse(string CommandId, string Pulse, string NegatifPulse)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SET_PULSE, Pulse + "," + NegatifPulse);
        }

        #endregion

        #region RF SU Servisleri

        /// <summary>
        /// RF su sayacına kredi yükler
        /// </summary>
        /// <param name="ServisId"></param>
        /// <param name="MeterId">Sayaç Flag + Sayaç Seri numarası(ELM20122021)</param>
        /// <param name="FlagSend">FlagSend : Sayaç okuma esnasında Flag karakterinin gönderilip gönderilmeyeceğini tanımlar. '0' gönderilmeyecek, '1' gönderilecek demektir.</param>
        /// <param name="Kanal">Sayaç okuma kanalını tanımlar, '0:RS485', '1:RS232', '2:PLC', '3:RF'</param>
        /// <param name="StartBaudrate">StartBaud : Sayaç okumaya başlama baudrate' ini tanımlar. 
        ///'*' : default demektir. (300baudrate)
        ///'0' : 300 baudrate
        ///'1' : 600 baudrate
        ///'2' : 1200 baudrate
        ///'3' : 2400 baudrate
        ///'4' : 4800 baudrate
        ///'5' : 9600 baudrate
        ///'6' : 19200 baudrate</param>
        /// <param name="SwitchBaudrate">SwitchBaud : Sayaç okuma esnasında değiştirilen baudrate 'i tanımlar. 
        ///'*' : default demektir. Sayaç tarafından desteklenen baudrate kullanılır.
        ///Diğer baudrate tanımlamaları StartBaudrate gibidir.</param>
        /// <param name="ReadMode">ReadMode : Sayaç okuma modunu tanımlar. '0' Readout mode, '1' Partial readout mod</param>
        /// <param name="Kredi">Metre küp olarak kredi miktarı</param>
        /// <returns></returns>
        public AmiCommand RfSuKrediYukle(string CommandId, string MeterId,
                                          string FlagSend, string Kanal, string StartBaudrate, string SwitchBaudrate, string ReadMode,
                                          int Kredi)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_RF_ADD_CREDIT,
                                 MeterId + "-" + FlagSend + "-" + Kanal + "-" + StartBaudrate + "-" + SwitchBaudrate + "-" + ReadMode + "," +
                                 Kredi);
        }


        #endregion

        #endregion


        #region Sayaç Okuma servisleri

        /// <summary>
        /// Kaydedilen readoutları getir
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand GetReadouts(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SEND_READOUTS, "READOUTS");
        }

        /// <summary>
        /// Kaydedilen readoutları tablo numarasına göre filtreleyerek getir
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="TableNum">Tablo numarası</param>
        /// <returns></returns>
        public AmiCommand GetReadoutsByTableNum(string CommandId, string TableNum)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_SEND_READOUTS, "READOUTS" + "," + TableNum);
        }

        /// <summary>
        /// Kaydedilen readoutları tablo numaralarına göre filtreleyerek getir
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="TableNums">Tablo numaraları</param>
        /// <returns></returns>
        public AmiCommand GetReadoutsByTableNums(string CommandId, string[] TableNums)
        {
            if (TableNums != null && TableNums.Length > 0)
            {
                string t = "";
                foreach (var s in TableNums)
                {
                    t += s + ",";
                }
                t = t.Remove(t.LastIndexOf(","));

                return new AmiCommand(CommandId, CommandCode.CMD_SEND_READOUTS, "READOUTS," + t);
            }
            else return new AmiCommand(CommandId, CommandCode.CMD_SEND_READOUTS, "READOUTS");

        }

        /// <summary>
        /// Kaydedilen readoutları sil
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        public AmiCommand DelReadouts(string CommandId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_DEL_READOUTS, "READOUTS");
        }

        /// <summary>
        /// Yük profili oku
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="MeterId">Sayaç Flag + Sayaç Seri numarası(ELM20122021)</param>
        /// <param name="BasTar">Başlangıç tarihi</param>
        /// <param name="BitTar">Bitiş tarihi</param>
        /// <returns></returns>
        public AmiCommand LoadProfileMini(string CommandId, string MeterId,
                                    DateTime BasTar, DateTime BitTar)
        {
            string basTarih = (BasTar.Year - 2000) + BasTar.Month.ToString().PadLeft(2, '0') + BasTar.Day.ToString().PadLeft(2, '0') + BasTar.Hour.ToString().PadLeft(2, '0') + BasTar.Minute.ToString().PadLeft(2, '0');
            string bitTarih = (BitTar.Year - 2000) + BitTar.Month.ToString().PadLeft(2, '0') + BitTar.Day.ToString().PadLeft(2, '0') + BitTar.Hour.ToString().PadLeft(2, '0') + BitTar.Minute.ToString().PadLeft(2, '0');
            return new AmiCommand(CommandId, CommandCode.CMD_LOAD_PROFILE, MeterId + "," + basTarih + "-" + bitTarih);

        }

        /// <summary>
        /// Yük profili oku
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="MeterId">Sayaç Flag + Sayaç Seri numarası(ELM20122021)</param>
        /// <param name="FlagSend">FlagSend : Sayaç okuma esnasında Flag karakterinin gönderilip gönderilmeyeceğini tanımlar. '0' gönderilmeyecek, '1' gönderilecek demektir.</param>
        /// <param name="Kanal">Sayaç okuma kanalını tanımlar, '0' RS485, '1' RS232, '2' PLC, '3' RF</param>
        /// <param name="StartBaudrate">StartBaud : Sayaç okumaya başlama baudrate' ini tanımlar. 
        ///'*' : default demektir. (300baudrate)
        ///'0' : 300 baudrate
        ///'1' : 600 baudrate
        ///'2' : 1200 baudrate
        ///'3' : 2400 baudrate
        ///'4' : 4800 baudrate
        ///'5' : 9600 baudrate
        ///'6' : 19200 baudrate</param>
        /// <param name="SwitchBaudrate">SwitchBaud : Sayaç okuma esnasında değiştirilen baudrate 'i tanımlar. 
        ///'*' : default demektir. Sayaç tarafından desteklenen baudrate kullanılır.
        ///Diğer baudrate tanımlamaları StartBaudrate gibidir.</param>
        /// <param name="ReadMode">ReadMode : Sayaç okuma modunu tanımlar. '0' Readout mode, '1' Partial readout mod</param>
        /// <param name="BasTar">Başlangıç tarihi</param>
        /// <param name="BitTar">Bitiş tarihi</param>
        /// <returns></returns>
        public AmiCommand LoadProfileUltra(string CommandId, string MeterId,
                                           string FlagSend, string Kanal, string StartBaudrate, string SwitchBaudrate, string ReadMode,
                                           DateTime BasTar, DateTime BitTar)
        {
            string basTarih = (BasTar.Year - 2000) + BasTar.Month.ToString().PadLeft(2, '0') + BasTar.Day.ToString().PadLeft(2, '0') + BasTar.Hour.ToString().PadLeft(2, '0') + BasTar.Minute.ToString().PadLeft(2, '0');
            string bitTarih = (BitTar.Year - 2000) + BitTar.Month.ToString().PadLeft(2, '0') + BitTar.Day.ToString().PadLeft(2, '0') + BitTar.Hour.ToString().PadLeft(2, '0') + BitTar.Minute.ToString().PadLeft(2, '0');
            return new AmiCommand(CommandId, CommandCode.CMD_LOAD_PROFILE, MeterId + "-" + FlagSend + "-" + Kanal + "-" + StartBaudrate + "-" + SwitchBaudrate + "-" + ReadMode + "," + basTarih + "-" + bitTarih);

        }

        /// <summary>
        /// Tüm Yük profilini oku
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="MeterId">Sayaç Flag + Sayaç Seri numarası(ELM20122021)</param>
        /// <param name="BasTar">Başlangıç tarihi</param>
        /// <param name="BitTar">Bitiş tarihi</param>
        /// <returns></returns>
        public AmiCommand LoadProfileAll(string CommandId, string MeterId,
                                         string FlagSend, string Kanal, string StartBaudrate, string SwitchBaudrate, string ReadMode)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_LOAD_PROFILE, MeterId + "-" + FlagSend + "-" + Kanal + "-" + StartBaudrate + "-" + SwitchBaudrate + "-" + ReadMode + ",ALL_PROFILE");

        }

        /// <summary>
        /// Anlık olarak sayaca bağlanır ve full readout yapar.
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="MeterId">Sayaç Flag + Sayaç Seri numarası(ELM20122021)</param>
        /// <returns></returns>
        public AmiCommand InstantReadout(string CommandId, string MeterId)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_INSTANT_READOUT, MeterId);
        }

        /// <summary>
        /// Anlık olarak sayaca bağlanır ve full readout yapar.
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="MeterId">Sayaç Flag + Sayaç Seri numarası(ELM20122021)</param>
        /// <param name="FlagSend">FlagSend : Sayaç okuma esnasında Flag karakterinin gönderilip gönderilmeyeceğini tanımlar. '0' gönderilmeyecek, '1' gönderilecek demektir.</param>
        /// <param name="Kanal">Sayaç okuma kanalını tanımlar, '0' RS485, '1' RS232</param>
        /// <param name="StartBaudrate">StartBaud : Sayaç okumaya başlama baudrate' ini tanımlar. 
        ///'*' : default demektir. (300baudrate)
        ///'0' : 300 baudrate
        ///'1' : 600 baudrate
        ///'2' : 1200 baudrate
        ///'3' : 2400 baudrate
        ///'4' : 4800 baudrate
        ///'5' : 9600 baudrate
        ///'6' : 19200 baudrate</param>
        /// <param name="SwitchBaudrate">SwitchBaud : Sayaç okuma esnasında değiştirilen baudrate 'i tanımlar. 
        ///'*' : default demektir. Sayaç tarafından desteklenen baudrate kullanılır.
        ///Diğer baudrate tanımlamaları StartBaudrate gibidir.</param>
        /// <param name="ReadMode">ReadMode : Sayaç okuma modunu tanımlar. '0' Readout mode, '1' Partial readout mod</param>
        /// <returns></returns>
        public AmiCommand InstantReadoutElk(string CommandId, string MeterId, string FlagSend, string Kanal, string StartBaudrate, string SwitchBaudrate, string ReadMode)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_INSTANT_READOUT, MeterId + "-" + FlagSend + "-" + Kanal + "-" + StartBaudrate + "-" + SwitchBaudrate + "-" + ReadMode);
        }


        #endregion

        #region Aydınlatma modülü

        /// <summary>
        /// Aydınlatma modülü kontrol
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="BolgeKodu">Bölge kodu</param>
        /// <returns></returns>
        //public AmiCommand AydinlatmaModulOn(string CommandId, string BolgeKodu)
        //{
        //    return new AmiCommand(CommandId, CommandCode.CMD_ILLUM_CONTROL, BolgeKodu);
        //}

        /// <summary>
        /// Aydınlatma modülü kontrol
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        //public AmiCommand AydinlatmaModulOff(string CommandId)
        //{
        //    return new AmiCommand(CommandId, CommandCode.CMD_ILLUM_CONTROL, "ILLUM_OFF");
        //}

        /// <summary>
        /// Aydınlatma modülü Yuzde Elli Saat
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="saat">Yuzde Elli Saat</param>
        /// <returns></returns>
        //public AmiCommand AydinlatmaModulYuzdeElliSaatSet(string CommandId, string saat)
        //{
        //    return new AmiCommand(CommandId, CommandCode.CMD_ILLUM_FIFTY_HOURS, saat);
        //}

        /// <summary>
        /// Aydınlatma modülü tablo güncelle
        /// </summary>
        /// <param name="CommandId"></param>
        /// <param name="TableSize">Tablo büyüklüğü</param>
        /// <returns></returns>
        public AmiCommand AydinlatmaModulTableUpdate(string CommandId, int TableSize)
        {
            return new AmiCommand(CommandId, CommandCode.CMD_ILLUM_UPDATE_TABLE, TableSize.ToString());
        }

        #endregion

    }
}
