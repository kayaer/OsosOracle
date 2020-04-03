using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSCSTOPERASYONComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfSYSCSTOPERASYONDal : ISYSCSTOPERASYONDal
    {


        private IQueryable<SYSCSTOPERASYONDetay> DetayDoldur(IQueryable<SYSCSTOPERASYONEf> result)
        {
            return result.Select(x => new SYSCSTOPERASYONDetay()
            {
                KAYITNO = x.KAYITNO,
                AD = x.AD,
                ACIKLAMA = x.ACIKLAMA,
                VERSIYON = x.VERSIYON,
                MENUKAYITNO = x.MENUKAYITNO

            });
        }




        private IQueryable<SYSCSTOPERASYONEf> Filtrele(IQueryable<SYSCSTOPERASYONEf> result, SYSCSTOPERASYONAra filtre = null)
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

                    if (!string.IsNullOrEmpty(filtre.AD))
                    {
                        result = result.Where(x => x.AD.Contains(filtre.AD));
                    }
                    if (!string.IsNullOrEmpty(filtre.ACIKLAMA))
                    {
                        result = result.Where(x => x.ACIKLAMA.Contains(filtre.ACIKLAMA));
                    }
                    
                    if (filtre.VERSIYON != null)
                    {
                        result = result.Where(x => x.VERSIYON == filtre.VERSIYON);
                    }
                    if (filtre.MENUKAYITNO != null)
                    {
                        result = result.Where(x => x.MENUKAYITNO == filtre.MENUKAYITNO);
                    }
                }
            }
            return result;
        }

        public List<SYSCSTOPERASYON> Getir(SYSCSTOPERASYONAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<SYSCSTOPERASYON> GetirWithContext(AppContext context, SYSCSTOPERASYONAra filtre = null)
        {
            var filterHelper = new FilterHelper<SYSCSTOPERASYONEf>();
            return filterHelper.Sayfala(Filtrele(context.SYSCSTOPERASYONEf.AsQueryable(), filtre), filtre?.Ara).ToList<SYSCSTOPERASYON>();
        }


        public List<SYSCSTOPERASYONDetay> DetayGetir(SYSCSTOPERASYONAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSCSTOPERASYONDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.SYSCSTOPERASYONEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public SYSCSTOPERASYONDataTable Ara(SYSCSTOPERASYONAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSCSTOPERASYONDetay>();

                return new SYSCSTOPERASYONDataTable
                {
                    SYSCSTOPERASYONDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.SYSCSTOPERASYONEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(SYSCSTOPERASYONAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSCSTOPERASYONEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.SYSCSTOPERASYONEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public SYSCSTOPERASYON Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.SYSCSTOPERASYONEf.Find(id);
            }
        }

        public SYSCSTOPERASYONDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.SYSCSTOPERASYONEf.AsQueryable(), new SYSCSTOPERASYONAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<SYSCSTOPERASYON> Ekle(List<SYSCSTOPERASYONEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.SYSCSTOPERASYONEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<SYSCSTOPERASYON> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.SYSCSTOPERASYONEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<SYSCSTOPERASYON>();
                }

                return null;
            }
        }


        public void Guncelle(List<SYSCSTOPERASYONEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<SYSCSTOPERASYONEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.SYSCSTOPERASYONEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.SYSCSTOPERASYONEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<SYSCSTOPERASYONEf> mevcutDegerler, List<SYSCSTOPERASYONEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("SYSCSTOPERASYON bulunamadı");

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
                    var entry = context.SYSCSTOPERASYONEf.Find(idler[0]);
                    if (entry != null)
                        context.SYSCSTOPERASYONEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.SYSCSTOPERASYONEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.SYSCSTOPERASYONEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
