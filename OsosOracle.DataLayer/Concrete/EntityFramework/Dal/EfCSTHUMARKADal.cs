using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.CSTHUMARKAComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfCSTHUMARKADal : ICSTHUMARKADal
    {


        private IQueryable<CSTHUMARKADetay> DetayDoldur(IQueryable<CSTHUMARKAEf> result)
        {
            return result.Select(x => new CSTHUMARKADetay()
            {
                //	CSTHUMARKA = x,
                KAYITNO = x.KAYITNO,
                AD = x.AD,
                ACIKLAMA = x.ACIKLAMA,
                VERSIYON = x.VERSIYON,

                //TODO: Ek detayları buraya ekleyiniz
                //örnek: CSTHUMARKADurumu = x.NesneDegerDurumEf.Adi

            });
        }




        private IQueryable<CSTHUMARKAEf> Filtrele(IQueryable<CSTHUMARKAEf> result, CSTHUMARKAAra filtre = null)
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
                }
            }
            return result;
        }

        public List<CSTHUMARKA> Getir(CSTHUMARKAAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<CSTHUMARKA> GetirWithContext(AppContext context, CSTHUMARKAAra filtre = null)
        {
            var filterHelper = new FilterHelper<CSTHUMARKAEf>();
            return filterHelper.Sayfala(Filtrele(context.CstHuMarkaEf.AsQueryable(), filtre), filtre?.Ara).ToList<CSTHUMARKA>();
        }


        public List<CSTHUMARKADetay> DetayGetir(CSTHUMARKAAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<CSTHUMARKADetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.CstHuMarkaEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public CSTHUMARKADataTable Ara(CSTHUMARKAAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<CSTHUMARKADetay>();

                return new CSTHUMARKADataTable
                {
                    CSTHUMARKADetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.CstHuMarkaEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(CSTHUMARKAAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<CSTHUMARKAEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.CstHuMarkaEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public CSTHUMARKA Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.CstHuMarkaEf.Find(id);
            }
        }

        public CSTHUMARKADetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.CstHuMarkaEf.AsQueryable(), new CSTHUMARKAAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<CSTHUMARKA> Ekle(List<CSTHUMARKAEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.CstHuMarkaEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<CSTHUMARKA> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.CstHuMarkaEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<CSTHUMARKA>();
                }

                return null;
            }
        }


        public void Guncelle(List<CSTHUMARKAEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<CSTHUMARKAEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.CstHuMarkaEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.CstHuMarkaEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<CSTHUMARKAEf> mevcutDegerler, List<CSTHUMARKAEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("CSTHUMARKA bulunamadı");

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
                    var entry = context.CstHuMarkaEf.Find(idler[0]);
                    if (entry != null)
                        context.CstHuMarkaEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.CstHuMarkaEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.CstHuMarkaEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
