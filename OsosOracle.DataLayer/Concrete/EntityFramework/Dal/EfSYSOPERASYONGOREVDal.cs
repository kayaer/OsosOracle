using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSOPERASYONGOREVComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfSYSOPERASYONGOREVDal : ISYSOPERASYONGOREVDal
    {


        private IQueryable<SYSOPERASYONGOREVDetay> DetayDoldur(IQueryable<SYSOPERASYONGOREVEf> result)
        {
            return result.Select(x => new SYSOPERASYONGOREVDetay()
            {
                KAYITNO=x.KAYITNO,
                OPERASYONKAYITNO = x.OPERASYONKAYITNO,
                GOREVKAYITNO = x.GOREVKAYITNO,
                VERSIYON = x.VERSIYON

            });
        }




        private IQueryable<SYSOPERASYONGOREVEf> Filtrele(IQueryable<SYSOPERASYONGOREVEf> result, SYSOPERASYONGOREVAra filtre = null)
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

                    if (filtre.OPERASYONKAYITNO != null)
                    {
                        result = result.Where(x => x.OPERASYONKAYITNO == filtre.OPERASYONKAYITNO);
                    }
                    if (filtre.GOREVKAYITNO != null)
                    {
                        result = result.Where(x => x.GOREVKAYITNO == filtre.GOREVKAYITNO);
                    }
                    if (filtre.VERSIYON != null)
                    {
                        result = result.Where(x => x.VERSIYON == filtre.VERSIYON);
                    }
                }
            }
            return result;
        }

        public List<SYSOPERASYONGOREV> Getir(SYSOPERASYONGOREVAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<SYSOPERASYONGOREV> GetirWithContext(AppContext context, SYSOPERASYONGOREVAra filtre = null)
        {
            var filterHelper = new FilterHelper<SYSOPERASYONGOREVEf>();
            return filterHelper.Sayfala(Filtrele(context.SYSOPERASYONGOREVEf.AsQueryable(), filtre), filtre?.Ara).ToList<SYSOPERASYONGOREV>();
        }


        public List<SYSOPERASYONGOREVDetay> DetayGetir(SYSOPERASYONGOREVAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSOPERASYONGOREVDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.SYSOPERASYONGOREVEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public SYSOPERASYONGOREVDataTable Ara(SYSOPERASYONGOREVAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSOPERASYONGOREVDetay>();

                return new SYSOPERASYONGOREVDataTable
                {
                    SYSOPERASYONGOREVDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.SYSOPERASYONGOREVEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(SYSOPERASYONGOREVAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSOPERASYONGOREVEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.SYSOPERASYONGOREVEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public SYSOPERASYONGOREV Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.SYSOPERASYONGOREVEf.Find(id);
            }
        }

        public SYSOPERASYONGOREVDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.SYSOPERASYONGOREVEf.AsQueryable(), new SYSOPERASYONGOREVAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<SYSOPERASYONGOREV> Ekle(List<SYSOPERASYONGOREVEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.SYSOPERASYONGOREVEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<SYSOPERASYONGOREV> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.SYSOPERASYONGOREVEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<SYSOPERASYONGOREV>();
                }

                return null;
            }
        }


        public void Guncelle(List<SYSOPERASYONGOREVEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<SYSOPERASYONGOREVEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.SYSOPERASYONGOREVEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.SYSOPERASYONGOREVEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<SYSOPERASYONGOREVEf> mevcutDegerler, List<SYSOPERASYONGOREVEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("SYSOPERASYONGOREV bulunamadı");

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
                    var entry = context.SYSOPERASYONGOREVEf.Find(idler[0]);
                    if (entry != null)
                        context.SYSOPERASYONGOREVEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.SYSOPERASYONGOREVEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.SYSOPERASYONGOREVEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }
        public bool OperasyonGorevSil(int gorevid)
        {
            using (var context = new AppContext())
            {
                var entry = context.SYSOPERASYONGOREVEf.Where(x => x.GOREVKAYITNO == gorevid).ToList();
                if (entry.Any())
                    context.SYSOPERASYONGOREVEf.RemoveRange(entry);

                context.SaveChanges();
            }
            return true;
        }
    }
}
