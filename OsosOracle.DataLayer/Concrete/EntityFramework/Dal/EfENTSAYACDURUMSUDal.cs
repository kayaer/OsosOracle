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
    public class EfENTSAYACDURUMSUDal : IENTSAYACDURUMSUDal
    {


        private IQueryable<ENTSAYACDURUMSUDetay> DetayDoldur(IQueryable<ENTSAYACDURUMSUEf> result)
        {
            return result.Select(x => new ENTSAYACDURUMSUDetay()
            {
                //	ENTSAYACDURUMSU = x,
                KAYITNO = x.KAYITNO,
                SAYACID = x.SayacId


            });
        }




        private IQueryable<ENTSAYACDURUMSUEf> Filtrele(IQueryable<ENTSAYACDURUMSUEf> result, ENTSAYACDURUMSUAra filtre = null)
        {
            //silindi kolonu varsa silinenler gelmesin

            //TODO: filtereyi özelleştir
            if (filtre != null)
            {
                //id ve idler
                if (filtre.KAYITNO != null) { result = result.Where(x => x.KAYITNO == filtre.KAYITNO); }
                else
                {

                    //if (!string.IsNullOrEmpty(filtre.Idler))
                    //{
                    //	var idList = filtre.Idler.ToList<int>();
                    //	result = result.Where(x => idList.Contains(x.KAYITNO));
                    //}

                    if (!string.IsNullOrEmpty(filtre.SAYACID))
                    {
                        result = result.Where(x => x.SayacId.Contains(filtre.SAYACID));
                    }
                    if (!string.IsNullOrEmpty(filtre.ACIKLAMA))
                    {
                        result = result.Where(x => x.ACIKLAMA.Contains(filtre.ACIKLAMA));
                    }
                    if (filtre.DURUM != null)
                    {
                        result = result.Where(x => x.DURUM == filtre.DURUM);
                    }
                    if (!string.IsNullOrEmpty(filtre.KREDIBITTI))
                    {
                        result = result.Where(x => x.KREDIBITTI.Contains(filtre.KREDIBITTI));
                    }
                    if (!string.IsNullOrEmpty(filtre.KREDIAZ))
                    {
                        result = result.Where(x => x.KREDIAZ.Contains(filtre.KREDIAZ));
                    }
                    if (!string.IsNullOrEmpty(filtre.ANAPILBITTI))
                    {
                        result = result.Where(x => x.ANAPILBITTI.Contains(filtre.ANAPILBITTI));
                    }
                    if (!string.IsNullOrEmpty(filtre.PULSEHATA))
                    {
                        result = result.Where(x => x.PULSEHATA.Contains(filtre.PULSEHATA));
                    }
                    if (!string.IsNullOrEmpty(filtre.RTCHATA))
                    {
                        result = result.Where(x => x.RTCHATA.Contains(filtre.RTCHATA));
                    }
                    if (!string.IsNullOrEmpty(filtre.VANA))
                    {
                        result = result.Where(x => x.VANA.Contains(filtre.VANA));
                    }
                    if (!string.IsNullOrEmpty(filtre.CEZA1))
                    {
                        result = result.Where(x => x.CEZA1.Contains(filtre.CEZA1));
                    }
                    if (!string.IsNullOrEmpty(filtre.CEZA2))
                    {
                        result = result.Where(x => x.CEZA2.Contains(filtre.CEZA2));
                    }
                    if (!string.IsNullOrEmpty(filtre.CEZA3))
                    {
                        result = result.Where(x => x.CEZA3.Contains(filtre.CEZA3));
                    }
                    if (!string.IsNullOrEmpty(filtre.CEZA4))
                    {
                        result = result.Where(x => x.CEZA4.Contains(filtre.CEZA4));
                    }
                    if (!string.IsNullOrEmpty(filtre.MAGNET))
                    {
                        result = result.Where(x => x.MAGNET.Contains(filtre.MAGNET));
                    }
                    if (filtre.VERSIYON != null)
                    {
                        result = result.Where(x => x.VERSIYON == filtre.VERSIYON);
                    }
                   
                    if (filtre.OKUMATARIH != null)
                    {
                        result = result.Where(x => x.OKUMATARIH == filtre.OKUMATARIH);
                    }
                    if (!string.IsNullOrEmpty(filtre.ARIZA))
                    {
                        result = result.Where(x => x.ARIZA.Contains(filtre.ARIZA));
                    }
                    if (!string.IsNullOrEmpty(filtre.ANAPILZAYIF))
                    {
                        result = result.Where(x => x.ANAPILZAYIF.Contains(filtre.ANAPILZAYIF));
                    }
                    if (!string.IsNullOrEmpty(filtre.MOTORPILZAYIF))
                    {
                        result = result.Where(x => x.MOTORPILZAYIF.Contains(filtre.MOTORPILZAYIF));
                    }
                    if (!string.IsNullOrEmpty(filtre.ASIRITUKETIM))
                    {
                        result = result.Where(x => x.ASIRITUKETIM.Contains(filtre.ASIRITUKETIM));
                    }
                    if (!string.IsNullOrEmpty(filtre.CEZA4IPTAL))
                    {
                        result = result.Where(x => x.CEZA4IPTAL.Contains(filtre.CEZA4IPTAL));
                    }
                    if (filtre.CAP != null)
                    {
                        result = result.Where(x => x.CAP == filtre.CAP);
                    }
                    if (filtre.BAGLANTISAYISI != null)
                    {
                        result = result.Where(x => x.BAGLANTISAYISI == filtre.BAGLANTISAYISI);
                    }
                    if (filtre.LIMIT1 != null)
                    {
                        result = result.Where(x => x.LIMIT1 == filtre.LIMIT1);
                    }
                    if (filtre.LIMIT2 != null)
                    {
                        result = result.Where(x => x.LIMIT2 == filtre.LIMIT2);
                    }
                    if (filtre.LIMIT3 != null)
                    {
                        result = result.Where(x => x.LIMIT3 == filtre.LIMIT3);
                    }
                    if (filtre.LIMIT4 != null)
                    {
                        result = result.Where(x => x.LIMIT4 == filtre.LIMIT4);
                    }
                    if (filtre.KRITIKKREDI != null)
                    {
                        result = result.Where(x => x.KRITIKKREDI == filtre.KRITIKKREDI);
                    }
                    if (filtre.FIYAT1 != null)
                    {
                        result = result.Where(x => x.FIYAT1 == filtre.FIYAT1);
                    }
                    if (filtre.FIYAT2 != null)
                    {
                        result = result.Where(x => x.FIYAT2 == filtre.FIYAT2);
                    }
                    if (filtre.FIYAT3 != null)
                    {
                        result = result.Where(x => x.FIYAT3 == filtre.FIYAT3);
                    }
                    if (filtre.FIYAT4 != null)
                    {
                        result = result.Where(x => x.FIYAT4 == filtre.FIYAT4);
                    }
                    if (filtre.FIYAT5 != null)
                    {
                        result = result.Where(x => x.FIYAT5 == filtre.FIYAT5);
                    }
                    if (!string.IsNullOrEmpty(filtre.ARIZAA))
                    {
                        result = result.Where(x => x.ARIZAA.Contains(filtre.ARIZAA));
                    }
                    if (!string.IsNullOrEmpty(filtre.ARIZAK))
                    {
                        result = result.Where(x => x.ARIZAK.Contains(filtre.ARIZAK));
                    }
                    if (!string.IsNullOrEmpty(filtre.ARIZAP))
                    {
                        result = result.Where(x => x.ARIZAP.Contains(filtre.ARIZAP));
                    }
                    if (!string.IsNullOrEmpty(filtre.ARIZAPIL))
                    {
                        result = result.Where(x => x.ARIZAPIL.Contains(filtre.ARIZAPIL));
                    }
                    if (!string.IsNullOrEmpty(filtre.BORC))
                    {
                        result = result.Where(x => x.BORC.Contains(filtre.BORC));
                    }
                    if (!string.IsNullOrEmpty(filtre.ITERASYON))
                    {
                        result = result.Where(x => x.ITERASYON.Contains(filtre.ITERASYON));
                    }
                    if (!string.IsNullOrEmpty(filtre.PILAKIM))
                    {
                        result = result.Where(x => x.PILAKIM.Contains(filtre.PILAKIM));
                    }
                    if (!string.IsNullOrEmpty(filtre.PILVOLTAJ))
                    {
                        result = result.Where(x => x.PILVOLTAJ.Contains(filtre.PILVOLTAJ));
                    }
                    if (!string.IsNullOrEmpty(filtre.ABONENO))
                    {
                        result = result.Where(x => x.ABONENO.Contains(filtre.ABONENO));
                    }
                    if (!string.IsNullOrEmpty(filtre.ABONETIP))
                    {
                        result = result.Where(x => x.ABONETIP.Contains(filtre.ABONETIP));
                    }
                    if (!string.IsNullOrEmpty(filtre.ILKPULSETARIH))
                    {
                        result = result.Where(x => x.ILKPULSETARIH.Contains(filtre.ILKPULSETARIH));
                    }
                    if (!string.IsNullOrEmpty(filtre.SONPULSETARIH))
                    {
                        result = result.Where(x => x.SONPULSETARIH.Contains(filtre.SONPULSETARIH));
                    }
                    if (!string.IsNullOrEmpty(filtre.BORCTARIH))
                    {
                        result = result.Where(x => x.BORCTARIH.Contains(filtre.BORCTARIH));
                    }
                    if (filtre.MAXDEBI != null)
                    {
                        result = result.Where(x => x.MAXDEBI == filtre.MAXDEBI);
                    }
                    if (filtre.MAXDEBISINIR != null)
                    {
                        result = result.Where(x => x.MAXDEBISINIR == filtre.MAXDEBISINIR);
                    }
                    if (filtre.DONEMGUN != null)
                    {
                        result = result.Where(x => x.DONEMGUN == filtre.DONEMGUN);
                    }
                    if (filtre.DONEM != null)
                    {
                        result = result.Where(x => x.DONEM == filtre.DONEM);
                    }
                    if (!string.IsNullOrEmpty(filtre.VANAACMATARIH))
                    {
                        result = result.Where(x => x.VANAACMATARIH.Contains(filtre.VANAACMATARIH));
                    }
                    if (!string.IsNullOrEmpty(filtre.VANAKAPAMATARIH))
                    {
                        result = result.Where(x => x.VANAKAPAMATARIH.Contains(filtre.VANAKAPAMATARIH));
                    }
                    if (filtre.SICAKLIK != null)
                    {
                        result = result.Where(x => x.SICAKLIK == filtre.SICAKLIK);
                    }
                    if (filtre.MINSICAKLIK != null)
                    {
                        result = result.Where(x => x.MINSICAKLIK == filtre.MINSICAKLIK);
                    }
                    if (filtre.MAXSICAKLIK != null)
                    {
                        result = result.Where(x => x.MAXSICAKLIK == filtre.MAXSICAKLIK);
                    }
                    if (filtre.YANGINMODU != null)
                    {
                        result = result.Where(x => x.YANGINMODU == filtre.YANGINMODU);
                    }
                    if (!string.IsNullOrEmpty(filtre.SONYUKLENENKREDITARIH))
                    {
                        result = result.Where(x => x.SONYUKLENENKREDITARIH.Contains(filtre.SONYUKLENENKREDITARIH));
                    }
                }
            }
            return result;
        }

        public List<ENTSAYACDURUMSU> Getir(ENTSAYACDURUMSUAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<ENTSAYACDURUMSU> GetirWithContext(AppContext context, ENTSAYACDURUMSUAra filtre = null)
        {
            var filterHelper = new FilterHelper<ENTSAYACDURUMSUEf>();
            return filterHelper.Sayfala(Filtrele(context.EntSayacDurumSuEf.AsQueryable(), filtre), filtre?.Ara).ToList<ENTSAYACDURUMSU>();
        }


        public List<ENTSAYACDURUMSUDetay> DetayGetir(ENTSAYACDURUMSUAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTSAYACDURUMSUDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.EntSayacDurumSuEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public ENTSAYACDURUMSUDataTable Ara(ENTSAYACDURUMSUAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTSAYACDURUMSUDetay>();

                return new ENTSAYACDURUMSUDataTable
                {
                    ENTSAYACDURUMSUDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.EntSayacDurumSuEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(ENTSAYACDURUMSUAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTSAYACDURUMSUEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.EntSayacDurumSuEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public ENTSAYACDURUMSU Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.EntSayacDurumSuEf.Find(id);
            }
        }

        public ENTSAYACDURUMSUDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.EntSayacDurumSuEf.AsQueryable(), new ENTSAYACDURUMSUAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<ENTSAYACDURUMSU> Ekle(List<ENTSAYACDURUMSUEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.EntSayacDurumSuEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<ENTSAYACDURUMSU> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.EntSayacDurumSuEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<ENTSAYACDURUMSU>();
                }

                return null;
            }
        }


        public void Guncelle(List<ENTSAYACDURUMSUEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<ENTSAYACDURUMSUEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.EntSayacDurumSuEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.EntSayacDurumSuEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<ENTSAYACDURUMSUEf> mevcutDegerler, List<ENTSAYACDURUMSUEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("ENTSAYACDURUMSU bulunamadı");

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
                    var entry = context.EntSayacDurumSuEf.Find(idler[0]);
                    if (entry != null)
                        context.EntSayacDurumSuEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.EntSayacDurumSuEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.EntSayacDurumSuEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
