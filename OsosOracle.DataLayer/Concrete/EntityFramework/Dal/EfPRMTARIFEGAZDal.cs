using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.PRMTARIFEGAZComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfPRMTARIFEGAZDal : IPRMTARIFEGAZDal
    {


        private IQueryable<PRMTARIFEGAZDetay> DetayDoldur(IQueryable<PRMTARIFEGAZEf> result)
        {
            return result.Select(x => new PRMTARIFEGAZDetay()
            {
                KAYITNO = x.KAYITNO,
                TUKETIMLIMIT = x.TUKETIMLIMIT,
                SABAHSAAT = x.SABAHSAAT,
                AKSAMSAAT = x.AKSAMSAAT,
                PULSE = x.PULSE,
                BAYRAM1GUN = x.BAYRAM1GUN,
                BAYRAM1AY = x.BAYRAM1AY,
                BAYRAM1SURE = x.BAYRAM1SURE,
                BAYRAM2GUN = x.BAYRAM2GUN,
                BAYRAM2SURE = x.BAYRAM2SURE,
                BAYRAM2AY = x.BAYRAM2AY,
                KRITIKKREDI = x.KRITIKKREDI,
                SAYACTUR = x.SAYACTUR,
                SAYACCAP = x.SAYACCAP,
                AD = x.AD,
                FIYAT1 = x.FIYAT1,
                YEDEKKREDI = x.YEDEKKREDI,
                KURUMKAYITNO = x.KURUMKAYITNO,
                BIRIMFIYAT = x.BIRIMFIYAT,
                Kurum=x.ConKurumEf.AD

            });
        }




        private IQueryable<PRMTARIFEGAZEf> Filtrele(IQueryable<PRMTARIFEGAZEf> result, PRMTARIFEGAZAra filtre = null)
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

                    if (filtre.TUKETIMLIMIT != null)
                    {
                        result = result.Where(x => x.TUKETIMLIMIT == filtre.TUKETIMLIMIT);
                    }
                    if (!string.IsNullOrEmpty(filtre.SABAHSAAT))
                    {
                        result = result.Where(x => x.SABAHSAAT.Contains(filtre.SABAHSAAT));
                    }
                    if (!string.IsNullOrEmpty(filtre.AKSAMSAAT))
                    {
                        result = result.Where(x => x.AKSAMSAAT.Contains(filtre.AKSAMSAAT));
                    }
                    if (filtre.PULSE != null)
                    {
                        result = result.Where(x => x.PULSE == filtre.PULSE);
                    }
                    if (filtre.BAYRAM1GUN != null)
                    {
                        result = result.Where(x => x.BAYRAM1GUN == filtre.BAYRAM1GUN);
                    }
                    if (filtre.BAYRAM1AY != null)
                    {
                        result = result.Where(x => x.BAYRAM1AY == filtre.BAYRAM1AY);
                    }
                    if (filtre.BAYRAM1SURE != null)
                    {
                        result = result.Where(x => x.BAYRAM1SURE == filtre.BAYRAM1SURE);
                    }
                    if (filtre.BAYRAM2GUN != null)
                    {
                        result = result.Where(x => x.BAYRAM2GUN == filtre.BAYRAM2GUN);
                    }
                    if (filtre.BAYRAM2SURE != null)
                    {
                        result = result.Where(x => x.BAYRAM2SURE == filtre.BAYRAM2SURE);
                    }
                    if (filtre.BAYRAM2AY != null)
                    {
                        result = result.Where(x => x.BAYRAM2AY == filtre.BAYRAM2AY);
                    }
                    if (filtre.KRITIKKREDI != null)
                    {
                        result = result.Where(x => x.KRITIKKREDI == filtre.KRITIKKREDI);
                    }
                    if (filtre.SAYACTUR != null)
                    {
                        result = result.Where(x => x.SAYACTUR == filtre.SAYACTUR);
                    }
                    if (filtre.SAYACCAP != null)
                    {
                        result = result.Where(x => x.SAYACCAP == filtre.SAYACCAP);
                    }
                    if (!string.IsNullOrEmpty(filtre.AD))
                    {
                        result = result.Where(x => x.AD.Contains(filtre.AD));
                    }
                    if (filtre.FIYAT1 != null)
                    {
                        result = result.Where(x => x.FIYAT1 == filtre.FIYAT1);
                    }
                    if (filtre.YEDEKKREDI != null)
                    {
                        result = result.Where(x => x.YEDEKKREDI == filtre.YEDEKKREDI);
                    }
                    if (filtre.KURUMKAYITNO != null)
                    {
                        result = result.Where(x => x.KURUMKAYITNO == filtre.KURUMKAYITNO);
                    }
                    if (filtre.BIRIMFIYAT != null)
                    {
                        result = result.Where(x => x.BIRIMFIYAT == filtre.BIRIMFIYAT);
                    }
                }
            }
            return result;
        }

        public List<PRMTARIFEGAZ> Getir(PRMTARIFEGAZAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<PRMTARIFEGAZ> GetirWithContext(AppContext context, PRMTARIFEGAZAra filtre = null)
        {
            var filterHelper = new FilterHelper<PRMTARIFEGAZEf>();
            return filterHelper.Sayfala(Filtrele(context.PRMTARIFEGAZEf.AsQueryable(), filtre), filtre?.Ara).ToList<PRMTARIFEGAZ>();
        }


        public List<PRMTARIFEGAZDetay> DetayGetir(PRMTARIFEGAZAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<PRMTARIFEGAZDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.PRMTARIFEGAZEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public PRMTARIFEGAZDataTable Ara(PRMTARIFEGAZAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<PRMTARIFEGAZDetay>();

                return new PRMTARIFEGAZDataTable
                {
                    PRMTARIFEGAZDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.PRMTARIFEGAZEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(PRMTARIFEGAZAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<PRMTARIFEGAZEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.PRMTARIFEGAZEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public PRMTARIFEGAZ Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.PRMTARIFEGAZEf.Find(id);
            }
        }

        public PRMTARIFEGAZDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.PRMTARIFEGAZEf.AsQueryable(), new PRMTARIFEGAZAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<PRMTARIFEGAZ> Ekle(List<PRMTARIFEGAZEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.PRMTARIFEGAZEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<PRMTARIFEGAZ> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.PRMTARIFEGAZEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<PRMTARIFEGAZ>();
                }

                return null;
            }
        }


        public void Guncelle(List<PRMTARIFEGAZEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<PRMTARIFEGAZEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.PRMTARIFEGAZEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.PRMTARIFEGAZEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<PRMTARIFEGAZEf> mevcutDegerler, List<PRMTARIFEGAZEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("PRMTARIFEGAZ bulunamadı");

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
                    var entry = context.PRMTARIFEGAZEf.Find(idler[0]);
                    if (entry != null)
                        context.PRMTARIFEGAZEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.PRMTARIFEGAZEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.PRMTARIFEGAZEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
