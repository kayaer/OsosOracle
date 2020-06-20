using Newtonsoft.Json;
using OsosOracle.DataLayer.Concrete.EntityFramework.Dal;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitmqParser
{
    public class RabbitmqService
    {
        // localhost üzerinde kurulu olduğu için host adresi olarak bunu kullanıyorum.
        private readonly string _hostName = "localhost";
        private readonly string _queueName = "HamData";

        public IConnection GetRabbitMQConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory() { HostName = _hostName };
            return connectionFactory.CreateConnection();
        }

        public void Consume()
        {
            using (var connection = GetRabbitMQConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                        var hamdata = JsonConvert.DeserializeObject<EntHamData>(message);
                        var data = Encoding.UTF8.GetString(hamdata.Data);
                        Console.WriteLine(message);
                        StartParse(hamdata);

                    };

                    channel.BasicConsume(_queueName, true, consumer);
                    Console.ReadLine();
                }
            }
        }

        private void StartParse(EntHamData hamdata)
        {
            var encoding = new UTF8Encoding();

            string HamData = encoding.GetString(hamdata.Data);

            var splitPaket = HamData.Split('|');

            string[] k = splitPaket[0].Split(':');//Header Paket

            List<ENTSAYACDURUMSUEf> sayacDurumList = new List<ENTSAYACDURUMSUEf>();

            if (splitPaket.Count() > 1)
            {
                for (int i = 1; i < splitPaket.Count(); i++)
                {
                    string veriKontrol = VeriKontrol(splitPaket[i]);
                    try
                    {

                        if (string.IsNullOrEmpty(veriKontrol))
                        {
                            var sayacDurum = SayacDurumParse(splitPaket[i]);
                            sayacDurum.KonsSeriNo = k[1];
                            // sayacDurum.Ip= Ip yazılacak
                            sayacDurumList.Add(sayacDurum);
                        }
                        else
                        {
                            //Hatalı Data Log la
                        }
                    }
                    catch (Exception ex)
                    {
                        //Hata oluştu Logla
                    }



                }

            }

            EfENTSAYACDURUMSUDal dal = new EfENTSAYACDURUMSUDal();
            dal.Ekle(sayacDurumList);

        }

        private ENTSAYACDURUMSUEf SayacDurumParse(string hamData)
        {
            ENTSAYACDURUMSUEf sayacDurum = new ENTSAYACDURUMSUEf();
            string[] okunanData = hamData.Split('\n');

            string[] al = okunanData[0].Split('\n');
            sayacDurum.SayacId = al[0].Substring(0, al[0].Length).Trim();
            try
            {
                sayacDurum.OKUMATARIH = Convert.ToDateTime(okunanData[1]);
            }
            catch (Exception)
            {
                //Okuma Tarih pars edilemedi

            }
            foreach (string gelenData in okunanData)
            {
                if (gelenData.Contains("ERROR"))
                    continue;

                int pStart = gelenData.IndexOf('(');
                int pStop = gelenData.IndexOf(')');

                if (pStart != -1 && pStop != -1)
                {
                    if (gelenData.Contains("8.1.1.0"))
                    {
                        try
                        {
                            char[] data = gelenData.Substring(pStart + 1, pStop - (pStart + 1)).ToCharArray();
                            sayacDurum.CEZA1 = data[0].ToString();
                            sayacDurum.PULSEHATA = data[1].ToString();
                            sayacDurum.CEZA3 = data[2].ToString();
                            sayacDurum.ARIZA = data[3].ToString();
                            sayacDurum.CEZA2 = data[4].ToString();
                            sayacDurum.MAGNET = data[5].ToString();
                            sayacDurum.VANA = data[6].ToString();
                            sayacDurum.CEZA4 = data[7].ToString();
                        }
                        catch (Exception)
                        {
                        }


                    }
                    else if (gelenData.Contains("8.1.1.1"))
                    {
                        try
                        {
                            char[] data = gelenData.Substring(pStart + 1, pStop - (pStart + 1)).ToCharArray();
                            sayacDurum.ANAPILZAYIF = data[0].ToString();
                            sayacDurum.ANAPILBITTI = data[1].ToString();
                            sayacDurum.MOTORPILZAYIF = data[2].ToString();
                            sayacDurum.KREDIAZ = data[3].ToString();
                            sayacDurum.KREDIBITTI = data[4].ToString();
                            sayacDurum.RTCHATA = data[5].ToString();
                            sayacDurum.ASIRITUKETIM = data[6].ToString();
                            sayacDurum.CEZA4IPTAL = data[7].ToString();
                        }
                        catch (Exception)
                        {
                        }


                    }
                    else if (gelenData.Contains("9.0.0.0"))
                    {
                        try
                        {
                            char[] data = gelenData.Substring(pStart + 1, pStop - (pStart + 1)).ToCharArray();
                            sayacDurum.ARIZA = data[0].ToString();
                            sayacDurum.ARIZAK = data[1].ToString();
                            sayacDurum.ARIZAP = data[2].ToString();
                            sayacDurum.ARIZAPIL = data[3].ToString();
                            sayacDurum.BORC = data[4].ToString();

                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    else if (gelenData.Contains("9.0.0.1"))
                    {
                        try
                        {
                            string dataString = gelenData.Substring(pStart + 1, pStop - (pStart + 1));

                            string[] data = dataString.Split(',');
                            // sayacDurum.AnaPilZayif = Convert.ToInt32(data[0].ToString());
                            // sayacDurum.AnaPilBitti = Convert.ToInt32(data[1].ToString());
                            sayacDurum.CAP = Convert.ToInt32(data[2].ToString());
                            sayacDurum.BAGLANTISAYISI = Convert.ToInt32(data[3].ToString());
                            sayacDurum.KRITIKKREDI = Convert.ToInt32(data[4].ToString());
                            sayacDurum.LIMIT1 = Convert.ToInt32(data[5].ToString());
                            sayacDurum.LIMIT2 = Convert.ToInt32(data[6].ToString());
                            sayacDurum.LIMIT3 = Convert.ToInt32(data[7].ToString());
                            sayacDurum.LIMIT4 = Convert.ToInt32(data[8].ToString());
                            sayacDurum.FIYAT1 = Convert.ToInt64(data[9].ToString());
                            sayacDurum.FIYAT2 = Convert.ToInt64(data[10].ToString());
                            sayacDurum.FIYAT3 = Convert.ToInt64(data[11].ToString());
                            sayacDurum.FIYAT4 = Convert.ToInt64(data[12].ToString());
                            sayacDurum.FIYAT5 = Convert.ToInt64(data[13].ToString());



                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else if (gelenData.Contains("9.0.0.2"))
                    {
                        sayacDurum.ITERASYON = gelenData.Substring(pStart + 1, pStop - (pStart + 1));
                    }
                    else if (gelenData.Contains("9.0.0.3"))
                    {
                        string dataString = gelenData.Substring(pStart + 1, pStop - (pStart + 1));
                        string[] data = dataString.Split(',');
                        sayacDurum.PILAKIM = data[0];
                        sayacDurum.PILVOLTAJ = data[1];
                    }
                    else if (gelenData.Contains("9.0.0.4"))
                    {
                        string dataString = gelenData.Substring(pStart + 1, pStop - (pStart + 1));
                        string[] data = dataString.Split(',');
                        sayacDurum.ABONENO = data[0];
                        sayacDurum.ABONETIP = data[1];
                    }
                    else if (gelenData.Contains("9.0.0.5"))
                    {
                        string dataString = gelenData.Substring(pStart + 1, pStop - (pStart + 1));
                        string[] data = dataString.Split(',');

                        Integer2Byte IlkPulseTarih = new Integer2Byte(Convert.ToUInt16(data[0]));
                        Integer2Byte SonPulseTarih = new Integer2Byte(Convert.ToUInt16(data[1]));
                        Integer2Byte BorcTarih = new Integer2Byte(Convert.ToUInt16(data[2]));
                        sayacDurum.ILKPULSETARIH = TarihDuzenle(IlkPulseTarih.bir, IlkPulseTarih.iki);
                        sayacDurum.SONPULSETARIH = TarihDuzenle(SonPulseTarih.bir, SonPulseTarih.iki);
                        sayacDurum.BORCTARIH = TarihDuzenle(BorcTarih.bir, BorcTarih.iki);
                        sayacDurum.MAXDEBI = Convert.ToInt16(data[3]);
                        sayacDurum.MAXDEBISINIR = Convert.ToInt16(data[4]);
                    }
                    else if (gelenData.Contains("9.0.0.6"))
                    {
                        string dataString = gelenData.Substring(pStart + 1, pStop - (pStart + 1));
                        string[] data = dataString.Split(',');
                        sayacDurum.DONEMGUN = Convert.ToInt16(data[0]);
                        sayacDurum.DONEM = Convert.ToInt16(data[1]);
                        Integer2Byte VanaAcmaTarih = new Integer2Byte(Convert.ToUInt16(data[2]));
                        sayacDurum.VANAACMATARIH = TarihDuzenle(VanaAcmaTarih.bir, VanaAcmaTarih.iki);
                        Integer2Byte VanaKapamaTarih = new Integer2Byte(Convert.ToUInt16(data[2]));
                        sayacDurum.VANAKAPAMATARIH = TarihDuzenle(VanaKapamaTarih.bir, VanaKapamaTarih.iki);

                    }
                    else if (gelenData.Contains("9.0.0.7"))
                    {
                        string dataString = gelenData.Substring(pStart + 1, pStop - (pStart + 1));
                        string[] data = dataString.Split(',');
                        sayacDurum.SICAKLIK = Convert.ToInt16(data[0]);
                        sayacDurum.MINSICAKLIK = Convert.ToInt16(data[1]);
                        sayacDurum.MAXSICAKLIK = Convert.ToInt16(data[2]);
                        sayacDurum.YANGINMODU = Convert.ToInt16(data[3]);

                        Integer2Byte SonYuklenenKrediTarih = new Integer2Byte(Convert.ToUInt16(data[4]));
                        sayacDurum.SONYUKLENENKREDITARIH = TarihDuzenle(SonYuklenenKrediTarih.bir, SonYuklenenKrediTarih.iki);
                    }
                }
            }

            return sayacDurum;

        }
        private bool PaketKontrolAscii(string paket)
        {
            var sonuc = true;

            if (!string.IsNullOrEmpty(paket))
            {
                paket = paket.Replace('@', ' ');
                char[] pc = paket.ToCharArray();



                if (pc.Any(c => Convert.ToByte(c) > 127))
                {
                    sonuc = false;
                }

                if (pc.Any(c => Convert.ToByte(c) == 63))
                {
                    sonuc = false;
                }
                if (pc.Any(c => Convert.ToByte(c) == 64))
                {
                    sonuc = false;
                }
                if (pc.Any(c => Convert.ToByte(c) == 96))
                {
                    sonuc = false;
                }
            }
            return sonuc;
        }
        private string VeriKontrol(string okunanData)
        {
            string hataKontrol = string.Empty;


            if (string.IsNullOrEmpty(okunanData))
            {
                hataKontrol = "Optik Veri Null.";
            }
            else
            {
                if (!PaketKontrolAscii(okunanData))
                {
                    hataKontrol = "Paket Karakter Hatası";
                }
                if (okunanData.Contains("OPTIK ERROR") || okunanData.Contains("PLC ERROR") || okunanData.Contains("READOUT ERROR"))
                {
                    hataKontrol += "Paket ERROR içeriyor";
                }
                if (okunanData.Contains("METER"))
                {
                    hataKontrol += "Paket METER içeriyor";
                }



                if (hataKontrol == string.Empty)
                {

                    if (!okunanData.Contains("0.0.0")) //|| !okunanData.Contains("8.0.0.0"))
                    {
                        if (!okunanData.Contains("C.1.0"))
                            hataKontrol += "Optik Okuma Hatası( Cihaz No Yok )";
                    }
                }

            }
            return hataKontrol;

        }
        public string TarihDuzenle(int Byte1, int Byte2)
        {
            int Gun = ((Byte1 & 240) >> 4) + ((Byte2 & 1) << 4);

            int Ay = Byte1 & 15;

            int Yil = (Byte2 & 254) >> 1;
            Yil += 2000;

            if ((Gun > 31) || (Ay > 12) || (Yil > 2099) || (Gun == 0) || (Ay == 0))
            {
                return "00/00/2000";
            }
            else
            {
                return StringDuzenleBastan(Gun.ToString(), 2) + "/" +
                       StringDuzenleBastan(Ay.ToString(), 2) + "/" +
                       Yil.ToString();
            }

        }

        public string StringDuzenleBastan(string DuzenlenecekString, int IstenilenUzunluk)
        {
            int SifirSayisi = IstenilenUzunluk - DuzenlenecekString.Length;

            if (SifirSayisi > 0)
            {
                for (int i = 0; i < SifirSayisi; i++)
                {
                    DuzenlenecekString = "0" + DuzenlenecekString;
                }

            }
            return DuzenlenecekString;
        }
        public struct Integer2Byte
        {
            public byte bir, iki;
            public UInt16 value;
            public Integer2Byte(UInt16 Alinacak)
            {
                bir = Convert.ToByte(Alinacak & 0xFF);
                iki = Convert.ToByte((Alinacak & 0xFF00) >> 8);
                value = Alinacak;
            }

            public Integer2Byte(byte Veri1, byte Veri2)
            {
                bir = Veri1;
                iki = Veri2;
                value = Convert.ToUInt16(Veri2 * 256 + Veri1);
            }

            public Integer2Byte(byte Veri1, byte Veri2, byte Exor)
            {
                bir = Convert.ToByte(Veri1 ^ Exor);
                iki = Convert.ToByte(Veri2 ^ Exor);
                value = Convert.ToUInt16(iki * 256 + bir);
            }

        }
    }
}
