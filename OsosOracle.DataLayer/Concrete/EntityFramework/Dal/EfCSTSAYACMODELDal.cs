using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.CSTSAYACMODELComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfCSTSAYACMODELDal : ICSTSAYACMODELDal
    {


        private IQueryable<CSTSAYACMODELDetay> DetayDoldur(IQueryable<CSTSAYACMODELEf> result)
        {
            return result.Select(x => new CSTSAYACMODELDetay()
            {
                KAYITNO = x.KAYITNO,
                AD = x.AD,
                ACIKLAMA = x.ACIKLAMA,
                YAZILIMVERSIYON = x.YAZILIMVERSIYON,
                FLAG = x.FLAG,
                CONTROLLER = x.CONTROLLER

            });
        }




        private IQueryable<CSTSAYACMODELEf> Filtrele(IQueryable<CSTSAYACMODELEf> result, CSTSAYACMODELAra filtre = null)
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
                    if (filtre.DURUM != null)
                    {
                        result = result.Where(x => x.DURUM == filtre.DURUM);
                    }
                    if (filtre.VERSIYON != null)
                    {
                        result = result.Where(x => x.VERSIYON == filtre.VERSIYON);
                    }
                    if (filtre.MARKAKAYITNO != null)
                    {
                        result = result.Where(x => x.MARKAKAYITNO == filtre.MARKAKAYITNO);
                    }
                    if (!string.IsNullOrEmpty(filtre.YAZILIMVERSIYON))
                    {
                        result = result.Where(x => x.YAZILIMVERSIYON.Contains(filtre.YAZILIMVERSIYON));
                    }
                    if (!string.IsNullOrEmpty(filtre.FLAG))
                    {
                        result = result.Where(x => x.FLAG.Contains(filtre.FLAG));
                    }
                    //if (!string.IsNullOrEmpty(filtre.CONTROLLER))
                    //{
                    //    result = result.Where(x => x.CONTROLLER.Contains(filtre.CONTROLLER));
                    //}
                }
            }
            return result;
        }

        public List<CSTSAYACMODEL> Getir(CSTSAYACMODELAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<CSTSAYACMODEL> GetirWithContext(AppContext context, CSTSAYACMODELAra filtre = null)
        {
            var filterHelper = new FilterHelper<CSTSAYACMODELEf>();
            return filterHelper.Sayfala(Filtrele(context.CSTSAYACMODELEf.AsQueryable(), filtre), filtre?.Ara).ToList<CSTSAYACMODEL>();
        }


        public List<CSTSAYACMODELDetay> DetayGetir(CSTSAYACMODELAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<CSTSAYACMODELDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.CSTSAYACMODELEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public CSTSAYACMODELDataTable Ara(CSTSAYACMODELAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<CSTSAYACMODELDetay>();

                return new CSTSAYACMODELDataTable
                {
                    CSTSAYACMODELDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.CSTSAYACMODELEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(CSTSAYACMODELAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<CSTSAYACMODELEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.CSTSAYACMODELEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public CSTSAYACMODEL Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.CSTSAYACMODELEf.Find(id);
            }
        }

        public CSTSAYACMODELDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.CSTSAYACMODELEf.AsQueryable(), new CSTSAYACMODELAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<CSTSAYACMODEL> Ekle(List<CSTSAYACMODELEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.CSTSAYACMODELEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<CSTSAYACMODEL> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.CSTSAYACMODELEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<CSTSAYACMODEL>();
                }

                return null;
            }
        }


        public void Guncelle(List<CSTSAYACMODELEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<CSTSAYACMODELEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.CSTSAYACMODELEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.CSTSAYACMODELEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<CSTSAYACMODELEf> mevcutDegerler, List<CSTSAYACMODELEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("CSTSAYACMODEL bulunamadı");

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
                    var entry = context.CSTSAYACMODELEf.Find(idler[0]);
                    if (entry != null)
                        context.CSTSAYACMODELEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.CSTSAYACMODELEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.CSTSAYACMODELEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
