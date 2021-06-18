using log4net;
using Newtonsoft.Json;
using OsosOracle.Business.Concrete;
using OsosOracle.DataLayer.Concrete.EntityFramework.Dal;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.EntIsEmriComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Entities.Enums;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitmqParser.Helpers;
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
        private static readonly ILog _log = LogManager.GetLogger(typeof(ParserWindowsService));
        public IConnection GetRabbitMQConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory() { HostName = _hostName };
            //ConnectionFactory connectionFactory = new ConnectionFactory() { HostName = "192.168.1.152", UserName = "admin", Password = "admin" };
            return connectionFactory.CreateConnection();
        }

        public void Consume()
        {
            //Console.WriteLine("Ent İş Emri Sorgulanıyor");
            //EfEntIsEmriDal entIsEmriDal = new EfEntIsEmriDal();
            //var list = entIsEmriDal.DetayGetir(new EntIsEmriAra { KonsSeriNo = "359855074432786", IsEmriDurumKayitNo = enumIsEmirleriDurum.Bekliyor.GetHashCode() });
            //if (list.Count > 0)
            //{
            //    Console.WriteLine("Komut Bulundu");
            //}
            //else
            //{
            //    Console.WriteLine("Komut Bulunamadı");
            //}


            // test hamdata
            // string testData = @"{'KonsSeriNo':'','Ip':'178.241.123.72','Data':'fEVMTTExMQ0KMjIuNi4yMCA3OjIwDQo4LjAuMC4wKDExMSkNCjguOTYuNTEuMCgxMDI2MSprKQ0KOC45Ni41MS4xKDAqaykNCjguMS44LjAoMCptMykNCjguMS44LjEoMCptMykNCjguMC45LjIoMjAtNi0yMikNCjguMC45LjEoNzoyMDozNCkNCjguMC45LjUoMSkNCjguMS4xLjAoMDAxMDExMDApDQo4LjEuMS4xKDAwMDAwMDAxKQ0KOC4xLjEuMig2NDQ5MDA1LDAsMTAyNjEsMCw4NDIxNTA0NDYsOTQ4OSwwLDAsMCwwLDAsMCwwLDAsMCwwLDAsMCwwLDAsMCwwLDAsLTEsMTAsOTQ4OSw3OSkNCjkuMC4wLjAoMDAwMDApDQo5LjAuMC4xKDE4NDksMjk4NywyMCw5OSw1MDAwLDk5OTk5OSw5OTk5OTksOTk5OTk5LDk5OTk5OSwxMDAwLDEwMDAsMTAwMCwxMDAwLDEwMDApDQo5LjAuMC4yKEwwMDM2KQ0KOS4wLjAuMygzLjU5ODAwMCw0LjAyMzAwMCkNCjkuMC4wLjQoMjIyLDEpDQo5LjAuMC41KDAsMCwwLDAsMCkNCjkuMC4wLjYoNjAsMSw5MDUyLDkwNTIpDQo5LjAuMC43KDI3LDExLDM0LDAsOTQ4OSwwKQ0K'}";
            //string testData = @"{'KonsSeriNo':'359855072144409','Ip':'178.244.61.189','Data':'RFNQMDowMDA2OjM1OT
            //g1NTA3MjE0NDQwOTpWIDIuMDAuNzALOjU6MToyMDEyOjgtMTAtMjAyMC0yMy00Ny0xMzoyMDowfEVMTT
            //IwMjAxMDA2DQo4LjEwLjIwIDIzOjQ3DQo4LjAuMC4wKDIwMjAxMDA2KQ0KOC45Ni41MS4wKDUwMDAqay
            //kNCjguOTYuNTEuMSgwKmspDQo4LjEuOC4wKDAqbTMpDQo4LjEuOC4xKDAqbTMpDQo4LjAuOS4yKDIwLT
            //EwLTgpDQo4LjAuOS4xKDIzOjQ3OjI0KQ0KOC4wLjkuNSg0KQ0KOC4xLjEuMCgwMDEwMDAwMSkNCjguMS
            //4xLjEoMDAwMDAwMDApDQo4LjEuMS4yKDk5MTM3MzQ1MCw1MDAwLDg0MjE1MDQ0NiwxMDM3ODkuMC4wLj
            //AoMDAwMDApDQo5LjAuMC4xKDAsMCwyMCwyMCwzMDAwLDk5OTk5OSw5OTk5OTksOTk5OTk5LDk5OTk5OS
            //wxMDAwLDEwMDAsMTAwMCwxMDAwLDEwMDApDQo5LjAuMC4yKFYgMi4wMC43MBcpDQo5LjAuMC4zKCVmLC
            //VmKQ0KOS4wLjAuNCglbHUsJWQpDQo5LjAuMC41KCVkLCVkLCVkLCVkLCVkKQ0KOS4wLjAuNiglZCwlZC
            //wlZCwlZCkNCjkuMC4wLjcoJWQsJWQsJWQsJWQsJWQsJWQpDQo = '}";
            //var ss = JsonConvert.DeserializeObject<EntHamData>(testData);
            //var ffdata = Encoding.UTF8.GetString(ss.Data);
            //StartParse(ss);



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

//                        string HamData = @"DSP0:0006:359855072117272:V 2.00.16:5:1:2012:29-10-2020-11-30-21:11:0|ELM111
//            29.10.20 11:30
//8.0.0.0(111)
//8.96.51.0(90647 * k)
//8.96.51.1(9363 * k)
//8.1.8.0(9363 * m3)
//8.1.8.1(9363 * m3)
//8.0.9.2(20 - 10 - 29)
//8.0.9.1(11:30:28)
//8.0.9.5(4)
//8.1.1.0(00010011)
//8.1.1.1(00000000)
//8.1.1.2(0, 100010, 0, 76058)
//9.0.0.0(00010)
//9.0.0.1(67385, 68523, 20, 9999990000000000internet, 30, 2000, 4000, 8000, 16000, 1000, 1000, 1000, 1000, 1000)
//9.0.0.2(V 2.00.16)
//9.0.0.3(3.511, 4.412)
//9.0.0.4(222, 1)
//9.0.0.5(10458, 10522, 0, 0, 10)
//9.0.0.6(30, 28, 10522, 10522)
//9.0.0.7(0, 0, 24, 24, 0, 10522)
//";

            string HamData = encoding.GetString(hamdata.Data);
            Console.WriteLine(HamData);

            var splitPaket = HamData.Split('|');

            //  string[] k = splitPaket[0].Split(':');//Header Paket
            Header hd = new Header(splitPaket[0]);

            List<EntSayacOkumaVeriEf> sayacOkumaList = new List<EntSayacOkumaVeriEf>();

            if (splitPaket.Count() > 1)
            {
                for (int i = 1; i < splitPaket.Count(); i++)
                {
                    string veriKontrol = VeriKontrol(splitPaket[i]);
                    try
                    {

                        if (string.IsNullOrEmpty(veriKontrol))
                        {
                            var sayacDurum = SayacVeriParse(splitPaket[i]);
                            sayacDurum.KonsSeriNo = hamdata.KonsSeriNo;
                            sayacDurum.Ip = hamdata.Ip;
                            sayacDurum.Rssi = hd.RSSI;
                            sayacOkumaList.Add(sayacDurum);
                        }
                        else
                        {
                            //Hatalı Data Log la
                            _log.Error("Hatalı Data : " + HamData);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Hata oluştu Logla
                        _log.Error(ex.Message + " : " + HamData);
                    }



                }

            }


            EfEntSayacOkumaVeriDal dal = new EfEntSayacOkumaVeriDal();
            var data = dal.Ekle(sayacOkumaList);
            if (sayacOkumaList.Count > 0)
            {


                Console.WriteLine("Veri Kaydedildi");
            }
            else
            {
                Console.WriteLine("Veri Kaydedilemedi");
            }



        }

        private EntSayacOkumaVeriEf SayacVeriParse(string hamData)
        {
            byte krediNoktaYeri = 1;
            byte tuketimNoktaYeri = 1;
            EfENTSAYACDal sayacDal = new EfENTSAYACDal();
            EntSayacOkumaVeriEf sayacVeri = new EntSayacOkumaVeriEf();
            string[] okunanData = hamData.Split('\n');

            string[] al = okunanData[0].Split('\n');
            string flag = al[0].Substring(0, 3);
            sayacVeri.SayacId = al[0].Substring(0, al[0].Length).Trim();
            try
            {
                sayacVeri.OkumaTarih = Convert.ToDateTime(okunanData[1]);
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
                            sayacVeri.Ceza1 = data[0].ToString();
                            sayacVeri.PulseHata = data[1].ToString();
                            sayacVeri.Ceza3 = data[2].ToString();
                            sayacVeri.Ariza = data[3].ToString();
                            sayacVeri.Ceza2 = data[4].ToString();
                            sayacVeri.Magnet = data[5].ToString();
                            sayacVeri.Vana = data[6].ToString();
                            sayacVeri.Ceza4 = data[7].ToString();
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
                            sayacVeri.AnaPilZayif = data[0].ToString();
                            sayacVeri.AnaPilBitti = data[1].ToString();
                            sayacVeri.MotorPilZayif = data[2].ToString();
                            sayacVeri.KrediAz = data[3].ToString();
                            sayacVeri.KrediBitti = data[4].ToString();
                            sayacVeri.RtcHata = data[5].ToString();
                            sayacVeri.AsiriTuketim = data[6].ToString();
                            sayacVeri.Ceza4Iptal = data[7].ToString();
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
                            sayacVeri.ArizaA = data[0].ToString(); //Bunu sor iki tane arıza var
                            sayacVeri.ArizaK = data[1].ToString();
                            sayacVeri.ArizaP = data[2].ToString();
                            sayacVeri.ArizaPil = data[3].ToString();
                            sayacVeri.Borc = data[4].ToString();

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
                            //sayacVeri.AnaPilZayif = data[0].ToString();//iki tane var
                            //sayacVeri.AnaPilBitti = data[1].ToString();//iki tane var 
                            sayacVeri.Cap = data[2].ToString();
                            sayacVeri.BaglantiSayisi = data[3].ToString();
                            sayacVeri.KritikKredi = data[4].ToString();
                            sayacVeri.Limit1 = data[5].ToString();
                            sayacVeri.Limit2 = data[6].ToString();
                            sayacVeri.Limit3 = data[7].ToString();
                            sayacVeri.Limit4 = data[8].ToString();
                            sayacVeri.Fiyat1 = data[9].ToString();
                            sayacVeri.Fiyat2 = data[10].ToString();
                            sayacVeri.Fiyat3 = data[11].ToString();
                            sayacVeri.Fiyat4 = data[12].ToString();
                            sayacVeri.Fiyat5 = data[13].ToString();



                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else if (gelenData.Contains("9.0.0.2"))
                    {
                        sayacVeri.Iterasyon = gelenData.Substring(pStart + 1, pStop - (pStart + 1));
                    }
                    else if (gelenData.Contains("9.0.0.3"))
                    {
                        string dataString = gelenData.Substring(pStart + 1, pStop - (pStart + 1));
                        string[] data = dataString.Split(',');
                        sayacVeri.PilAkim = data[0];
                        sayacVeri.PilVoltaj = data[1];
                    }
                    else if (gelenData.Contains("9.0.0.4"))
                    {
                        string dataString = gelenData.Substring(pStart + 1, pStop - (pStart + 1));
                        string[] data = dataString.Split(',');
                        sayacVeri.AboneNo = data[0];
                        sayacVeri.AboneTip = data[1];
                    }
                    else if (gelenData.Contains("9.0.0.5"))
                    {
                        string dataString = gelenData.Substring(pStart + 1, pStop - (pStart + 1));
                        string[] data = dataString.Split(',');
                        //Geçiçi kapatıldı hata veren okuma var
                        Integer2Byte IlkPulseTarih = new Integer2Byte(Convert.ToUInt16(data[0]));
                        Integer2Byte SonPulseTarih = new Integer2Byte(Convert.ToUInt16(data[1]));
                        Integer2Byte BorcTarih = new Integer2Byte(Convert.ToUInt16(data[2]));
                        sayacVeri.IlkPulseTarih = TarihDuzenle(IlkPulseTarih.bir, IlkPulseTarih.iki);
                        sayacVeri.SonPulseTarih = TarihDuzenle(SonPulseTarih.bir, SonPulseTarih.iki);
                        sayacVeri.BorcTarih = TarihDuzenle(BorcTarih.bir, BorcTarih.iki);
                        sayacVeri.MaxDebi = data[3];
                        sayacVeri.MaxDebiSinir = data[4];
                    }
                    else if (gelenData.Contains("9.0.0.6"))
                    {
                        string dataString = gelenData.Substring(pStart + 1, pStop - (pStart + 1));
                        string[] data = dataString.Split(',');
                        //Geçiçi olarak kapatıldı
                        sayacVeri.DonemGun = data[0];
                        sayacVeri.Donem = data[1];
                        Integer2Byte VanaAcmaTarih = new Integer2Byte(Convert.ToUInt16(data[2]));
                        sayacVeri.VanaAcmaTarih = TarihDuzenle(VanaAcmaTarih.bir, VanaAcmaTarih.iki);
                        Integer2Byte VanaKapamaTarih = new Integer2Byte(Convert.ToUInt16(data[2]));
                        sayacVeri.VanaKapamaTarih = TarihDuzenle(VanaKapamaTarih.bir, VanaKapamaTarih.iki);

                    }
                    else if (gelenData.Contains("9.0.0.7"))
                    {
                        string dataString = gelenData.Substring(pStart + 1, pStop - (pStart + 1));
                        string[] data = dataString.Split(',');
                        //Geçiçi olarak kapatıldı
                        sayacVeri.Sicaklik = data[0];
                        sayacVeri.MinSicaklik = data[1];
                        sayacVeri.MaxSicaklik = data[2];
                        sayacVeri.YanginModu = data[3];

                        //Integer2Byte SonYuklenenKrediTarih = new Integer2Byte(Convert.ToUInt16(data[4]));
                        //sayacVeri.SonYuklenenKrediTarih = TarihDuzenle(SonYuklenenKrediTarih.bir, SonYuklenenKrediTarih.iki);
                    }
                    else if (gelenData.Contains("8.0.0.0"))
                    {
                        sayacVeri.SayacId = flag + gelenData.Substring(pStart + 1, pStop - (pStart + 1));

                        var meter = sayacDal.DetayGetir(new OsosOracle.Entities.ComplexType.ENTSAYACComplexTypes.ENTSAYACAra { SERINO = gelenData.Substring(pStart + 1, pStop - (pStart + 1)) });

                        if (meter.Any())
                        {

                            switch (Convert.ToInt32(meter.First().Cap))
                            {
                                case 15:
                                    krediNoktaYeri = 10;
                                    tuketimNoktaYeri = 100;
                                    break;
                                case 20:
                                    krediNoktaYeri = 10;
                                    tuketimNoktaYeri = 100;
                                    break;
                                case 25:
                                    krediNoktaYeri = 10;
                                    tuketimNoktaYeri = 100;
                                    break;
                                case 40:
                                    krediNoktaYeri = 10;
                                    tuketimNoktaYeri = 10;
                                    break;
                                case 50:
                                case 65:
                                case 80:
                                case 100:
                                case 125:
                                case 150:
                                case 200:
                                case 250:
                                    krediNoktaYeri = 1;
                                    tuketimNoktaYeri = 1;
                                    break;
                            }

                        }
                        else
                        {
                            krediNoktaYeri = 1;
                            tuketimNoktaYeri = 1;
                        }
                    }
                    else if (gelenData.Contains("0.9.1"))//Sayaç Saati
                    {
                        try
                        {
                            string saat = gelenData.Substring(pStart + 1, pStop - (pStart + 1));

                            string t = Convert.ToDateTime(sayacVeri.SayacTarih).ToShortDateString();
                            sayacVeri.SayacTarih = Convert.ToDateTime(t + " " + saat);
                        }
                        catch
                        {

                        }

                    }
                    else if (gelenData.Contains("0.9.2"))//Sayaç Tarihi
                    {
                        try
                        {
                            string tarih = gelenData.Substring(pStart + 1, pStop - (pStart + 1));

                            if (tarih.Contains("-"))
                            {
                                string[] tt = tarih.Split('-');
                                tarih = tt[2] + "." + tt[1] + "." + tt[0];

                            }

                            sayacVeri.SayacTarih = Convert.ToDateTime(tarih);



                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    else if (gelenData.Contains("0.9.5"))
                    {
                        try
                        {
                            if (Convert.ToInt32(gelenData.Substring(pStart + 1, pStop - (pStart + 1))) == 2)
                            {
                                // tuketim.FaturaMod = 1;
                            }
                            else if (Convert.ToInt32(gelenData.Substring(pStart + 1, pStop - (pStart + 1))) == 3)
                            {
                                //tuketim.FaturaMod = 0;
                            }
                            else
                            {
                                sayacVeri.HaftaninGunu = gelenData.Substring(pStart + 1, pStop - (pStart + 1));
                            }

                        }
                        catch (Exception)
                        {


                        }

                    }
                    else if (gelenData.Contains("1.8.0("))//Kumulatif Aktif Enerji
                    {
                        try
                        {
                            sayacVeri.Tuketim = decimal.Parse(GetTuketim(gelenData)) / tuketimNoktaYeri;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Hata alındı ve tüketim kayıt edilmedi");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Hata alındı ve tüketim kayıt edilmedi");
                        }

                    }
                    else if (gelenData.Contains("1.8.1("))
                    {
                        try
                        {
                            sayacVeri.Tuketim1 = decimal.Parse(GetTuketim(gelenData)) / tuketimNoktaYeri;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Hata alındı ve tüketim kayıt edilmedi");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Hata alındı ve tüketim kayıt edilmedi");
                        }

                    }
                    else if (gelenData.Contains("1.8.2("))
                    {
                        try
                        {
                            sayacVeri.Tuketim2 = decimal.Parse(GetTuketim(gelenData)) / tuketimNoktaYeri;
                        }
                        catch (FormatException)
                        {

                            Console.WriteLine("Hata alındı ve tüketim kayıt edilmedi");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Hata alındı ve tüketim kayıt edilmedi");
                        }


                    }
                    else if (gelenData.Contains("1.8.3("))
                    {
                        try
                        {
                            sayacVeri.Tuketim3 = decimal.Parse(GetTuketim(gelenData)) / tuketimNoktaYeri;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Hata alındı ve tüketim kayıt edilmedi");

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Hata alındı ve tüketim kayıt edilmedi");
                        }

                    }
                    else if (gelenData.Contains("1.8.4("))
                    {
                        try
                        {
                            sayacVeri.Tuketim4 = decimal.Parse(GetTuketim(gelenData)) / tuketimNoktaYeri;
                        }
                        catch (FormatException)
                        {

                            Console.WriteLine("Hata alındı ve tüketim kayıt edilmedi");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Hata alındı ve tüketim kayıt edilmedi");
                        }

                    }
                    else if (gelenData.Contains("96.51.1"))
                    {
                        try
                        {
                            sayacVeri.HarcananKredi = decimal.Parse(GetTuketim(gelenData)) / krediNoktaYeri;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Hata alındı ve tüketim kayıt edilmedi");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Hata alındı ve tüketim kayıt edilmedi");
                        }
                        //RF pilli SU

                    }
                    else if (gelenData.Contains("96.51.0"))
                    {
                        try
                        {
                            sayacVeri.KalanKredi = decimal.Parse(GetTuketim(gelenData)) / krediNoktaYeri;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Hata alındı ve tüketim kayıt edilmedi");

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Hata alındı ve tüketim kayıt edilmedi");
                        }
                        //RF pilli SU

                    }
                }
            }

            return sayacVeri;

        }
        private string GetTuketim(string gelenData)
        {

            string sonuc = "";
            string sp = "*";
            try
            {
                int pStart = gelenData.IndexOf('(');
                int pStop = gelenData.IndexOf(')');

                sonuc = gelenData.Substring(pStart + 1, pStop - (pStart + 1));
                if (sonuc.Contains(sp))
                    sonuc = sonuc.Remove(sonuc.IndexOf(sp));
                sonuc = sonuc.Replace(".", ",");
            }
            catch (Exception ex)
            {
                sonuc = "";
            }

            return sonuc;
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
