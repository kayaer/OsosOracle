using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.CSTHUMODELComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfCSTHUMODELDal : ICSTHUMODELDal
    {


        private IQueryable<CSTHUMODELDetay> DetayDoldur(IQueryable<CSTHUMODELEf> result)
        {
            return result.Select(x => new CSTHUMODELDetay()
            {
                KAYITNO = x.KAYITNO,
                ACIKLAMA = x.ACIKLAMA,
                VERSIYON = x.VERSIYON,
                AD = x.AD,
                MARKAKAYITNO=x.MARKAKAYITNO

            });
        }




        private IQueryable<CSTHUMODELEf> Filtrele(IQueryable<CSTHUMODELEf> result, CSTHUMODELAra filtre = null)
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
                  
                    if (!string.IsNullOrEmpty(filtre.AD))
                    {
                        result = result.Where(x => x.AD.Contains(filtre.AD));
                    }
                }
            }
            return result;
        }

        public List<CSTHUMODEL> Getir(CSTHUMODELAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<CSTHUMODEL> GetirWithContext(AppContext context, CSTHUMODELAra filtre = null)
        {
            var filterHelper = new FilterHelper<CSTHUMODELEf>();
            return filterHelper.Sayfala(Filtrele(context.CstHuModelEf.AsQueryable(), filtre), filtre?.Ara).ToList<CSTHUMODEL>();
        }


        public List<CSTHUMODELDetay> DetayGetir(CSTHUMODELAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<CSTHUMODELDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.CstHuModelEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public CSTHUMODELDataTable Ara(CSTHUMODELAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<CSTHUMODELDetay>();

                return new CSTHUMODELDataTable
                {
                    CSTHUMODELDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.CstHuModelEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(CSTHUMODELAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<CSTHUMODELEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.CstHuModelEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public CSTHUMODEL Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.CstHuModelEf.Find(id);
            }
        }

        public CSTHUMODELDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.CstHuModelEf.AsQueryable(), new CSTHUMODELAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<CSTHUMODEL> Ekle(List<CSTHUMODELEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.CstHuModelEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<CSTHUMODEL> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.CstHuModelEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<CSTHUMODEL>();
                }

                return null;
            }
        }


        public void Guncelle(List<CSTHUMODELEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<CSTHUMODELEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.CstHuModelEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.CstHuModelEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<CSTHUMODELEf> mevcutDegerler, List<CSTHUMODELEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("CSTHUMODEL bulunamadı");

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
                    var entry = context.CstHuModelEf.Find(idler[0]);
                    if (entry != null)
                        context.CstHuModelEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.CstHuModelEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.CstHuModelEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
