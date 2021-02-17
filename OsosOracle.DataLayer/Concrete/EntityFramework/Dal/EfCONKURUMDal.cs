using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.CONKURUMComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfCONKURUMDal : ICONKURUMDal
    {


        private IQueryable<CONKURUMDetay> DetayDoldur(IQueryable<CONKURUMEf> result)
        {
            return result.Select(x => new CONKURUMDetay()
            {
                KAYITNO=x.KAYITNO,
                AD = x.AD,
                ACIKLAMA = x.ACIKLAMA,
                DllAdi=x.DllAdi,
                VERSIYON = x.VERSIYON

            });
        }




        private IQueryable<CONKURUMEf> Filtrele(IQueryable<CONKURUMEf> result, CONKURUMAra filtre = null)
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
                   
                }
            }
            return result;
        }

        public List<CONKURUM> Getir(CONKURUMAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<CONKURUM> GetirWithContext(AppContext context, CONKURUMAra filtre = null)
        {
            var filterHelper = new FilterHelper<CONKURUMEf>();
            return filterHelper.Sayfala(Filtrele(context.CONKURUMEf.AsQueryable(), filtre), filtre?.Ara).ToList<CONKURUM>();
        }


        public List<CONKURUMDetay> DetayGetir(CONKURUMAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<CONKURUMDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.CONKURUMEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public CONKURUMDataTable Ara(CONKURUMAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<CONKURUMDetay>();

                return new CONKURUMDataTable
                {
                    CONKURUMDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.CONKURUMEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(CONKURUMAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<CONKURUMEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.CONKURUMEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public CONKURUM Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.CONKURUMEf.Find(id);
            }
        }

        public CONKURUMDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.CONKURUMEf.AsQueryable(), new CONKURUMAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<CONKURUM> Ekle(List<CONKURUMEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.CONKURUMEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<CONKURUM> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.CONKURUMEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<CONKURUM>();
                }

                return null;
            }
        }


        public void Guncelle(List<CONKURUMEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<CONKURUMEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.CONKURUMEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.CONKURUMEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<CONKURUMEf> mevcutDegerler, List<CONKURUMEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("CONKURUM bulunamadı");

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
                    var entry = context.CONKURUMEf.Find(idler[0]);
                    if (entry != null)
                        context.CONKURUMEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.CONKURUMEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.CONKURUMEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
