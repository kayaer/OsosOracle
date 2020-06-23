using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTSAYACDURUMSUComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfEntSayacOkumaVeriDal : IEntSayacOkumaVeriDal
    {


        private IQueryable<EntSayacOkumaVeriDetay> DetayDoldur(IQueryable<EntSayacOkumaVeriEf> result)
        {
            return result.Select(x => new EntSayacOkumaVeriDetay()
            {
                KayitNo = x.KayitNo,
                SayacId = x.SayacId,
                OkumaTarih=x.OkumaTarih,
                Ceza1=x.Ceza1,
                Ceza2=x.Ceza2,
                Ceza3=x.Ceza3,
                Ceza4=x.Ceza4,
                Magnet=x.Magnet,
                Ariza=x.Ariza,
                Vana=x.Vana,
                PulseHata=x.PulseHata,
                AnaPilZayif=x.AnaPilZayif,
                AnaPilBitti=x.AnaPilBitti,
                MotorPilZayif=x.MotorPilZayif,
                KrediAz=x.KrediAz,
                KrediBitti=x.KrediBitti,
                RtcHata=x.RtcHata,
                AsiriTuketim=x.AsiriTuketim,
                Ceza4Iptal=x.Ceza4Iptal,
                ArizaK=x.ArizaK,
                ArizaP=x.ArizaP,
                ArizaPil=x.ArizaPil,
                Borc=x.Borc,
                Cap=x.Cap,
                BaglantiSayisi=x.BaglantiSayisi,
                KritikKredi=x.KritikKredi,
                Limit1=x.Limit1,
                Limit2=x.Limit2,
                Limit3=x.Limit3,
                Limit4=x.Limit4,
                Fiyat1=x.Fiyat1,
                Fiyat2=x.Fiyat2,
                Fiyat3=x.Fiyat3,
                Fiyat4=x.Fiyat4,
                Fiyat5=x.Fiyat5,
                Iterasyon=x.Iterasyon,
                PilAkim=x.PilAkim,
                PilVoltaj=x.PilVoltaj,
                AboneNo=x.AboneNo,
                AboneTip=x.AboneTip,
                IlkPulseTarih=x.IlkPulseTarih,
                SonPulseTarih=x.SonPulseTarih,
                BorcTarih=x.BorcTarih,
                MaxDebi=x.MaxDebi,
                MaxDebiSinir=x.MaxDebiSinir,
                DonemGun = x.DonemGun,
                Donem=x.Donem,
                VanaAcmaTarih=x.VanaAcmaTarih,
                VanaKapamaTarih=x.VanaKapamaTarih,
                Sicaklik=x.Sicaklik,
                MinSicaklik=x.MinSicaklik,
                MaxSicaklik=x.MaxSicaklik,
                YanginModu=x.YanginModu,
                SonYuklenenKrediTarih=x.SonYuklenenKrediTarih,
                SayacTarih=x.SayacTarih,
                HaftaninGunu=x.HaftaninGunu,
                Tuketim=x.Tuketim,
                Tuketim1=x.Tuketim1,
                Tuketim2=x.Tuketim2,
                Tuketim3=x.Tuketim3,
                Tuketim4=x.Tuketim4,
                HarcananKredi=x.HarcananKredi,
                KalanKredi=x.KalanKredi


            });
        }

        private IQueryable<EntSayacOkumaVeriEf> Filtrele(IQueryable<EntSayacOkumaVeriEf> result, EntSayacOkumaVeriAra filtre = null)
        {
            //silindi kolonu varsa silinenler gelmesin

            //TODO: filtereyi özelleştir
            if (filtre != null)
            {
                //id ve idler
                if (filtre.KayitNo != null) { result = result.Where(x => x.KayitNo == filtre.KayitNo); }
                else
                {
                    if (!string.IsNullOrEmpty(filtre.SayacId))
                    {
                        result = result.Where(x => x.SayacId.Contains(filtre.SayacId));
                    }
                    if (filtre.OkumaTarihiBaslangic != null)
                    {
                        result = result.Where(x => x.OkumaTarih >= filtre.OkumaTarihiBaslangic);
                    }
                    if (filtre.OkumaTarihiBitis != null)
                    {
                        result = result.Where(x => x.OkumaTarih <= filtre.OkumaTarihiBitis);
                    }
                   
                  
                }
            }
            return result;
        }

        public List<EntSayacOkumaVeri> Getir(EntSayacOkumaVeriAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<EntSayacOkumaVeri> GetirWithContext(AppContext context, EntSayacOkumaVeriAra filtre = null)
        {
            var filterHelper = new FilterHelper<EntSayacOkumaVeriEf>();
            return filterHelper.Sayfala(Filtrele(context.EntSayacOkumaVeriEf.AsQueryable(), filtre), filtre?.Ara).ToList<EntSayacOkumaVeri>();
        }


        public List<EntSayacOkumaVeriDetay> DetayGetir(EntSayacOkumaVeriAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<EntSayacOkumaVeriDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.EntSayacOkumaVeriEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public EntSayacOkumaVeriDataTable Ara(EntSayacOkumaVeriAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<EntSayacOkumaVeriDetay>();

                return new EntSayacOkumaVeriDataTable
                {
                    EntSayacOkumaVeriDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.EntSayacOkumaVeriEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(EntSayacOkumaVeriAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<EntSayacOkumaVeriEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.EntSayacOkumaVeriEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public EntSayacOkumaVeri Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.EntSayacOkumaVeriEf.Find(id);
            }
        }

        public EntSayacOkumaVeriDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.EntSayacOkumaVeriEf.AsQueryable(), new EntSayacOkumaVeriAra() { KayitNo = id }))
                  .FirstOrDefault();
            }
        }

        public List<EntSayacOkumaVeri> Ekle(List<EntSayacOkumaVeriEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.EntSayacOkumaVeriEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<EntSayacOkumaVeri> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.EntSayacOkumaVeriEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<EntSayacOkumaVeri>();
                }

                return null;
            }
        }


        public void Guncelle(List<EntSayacOkumaVeriEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<EntSayacOkumaVeriEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.EntSayacOkumaVeriEf.Find(yeniDegerler[0].KayitNo));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KayitNo).ToList();
                    mevcutDegerler = context.EntSayacOkumaVeriEf.Where(x => idler.Contains(x.KayitNo)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<EntSayacOkumaVeriEf> mevcutDegerler, List<EntSayacOkumaVeriEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KayitNo == yeniDeger.KayitNo);
                if (mevcutDeger == null)
                    throw new NotificationException("EntSayacOkumaVeri bulunamadı");

                var entry = context.Entry(mevcutDeger);
                entry.CurrentValues.SetValues(yeniDeger);

                ////Değişmemesi gereken kolonlar buraya yazılacak.
                //entry.Property(u => u.Id).IsModified = false;
                //entry.Property(u => u.EklemeTarihi).IsModified = false;
                //entry.Property(u => u.EkleyenId).IsModified = false;
            }
        }

        public void Sil(List<int> idler)
        {
            using (var context = new AppContext())
            {
                if (idler.Count == 1)
                {
                    var entry = context.EntSayacOkumaVeriEf.Find(idler[0]);
                    if (entry != null)
                        context.EntSayacOkumaVeriEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.EntSayacOkumaVeriEf.Where(x => idler.Contains(x.KayitNo)).ToList();
                    context.EntSayacOkumaVeriEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
