using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSGOREVComplexTypes;
using OsosOracle.Entities.ComplexType.SYSROLComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfSYSROLDal : ISYSROLDal
    {


        private IQueryable<SYSROLDetay> DetayDoldur(IQueryable<SYSROLEf> result)
        {
            return result.Select(x => new SYSROLDetay()
            {
                KAYITNO=x.KAYITNO,
                AD = x.AD,
                ACIKLAMA = x.ACIKLAMA,
                VERSIYON = x.VERSIYON,
                KURUMKAYITNO = x.KURUMKAYITNO,
                SysGorevList = x.RolSysGorevRolEfCollection.Select(a => new SYSGOREVDetay() { KAYITNO = a.SysGorevEf.KAYITNO, AD = a.SysGorevEf.AD }).ToList(),
                Kurum=x.ConKurumEf.AD
            });
        }




        private IQueryable<SYSROLEf> Filtrele(IQueryable<SYSROLEf> result, SYSROLAra filtre = null)
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
                    if (filtre.KURUMKAYITNO != null)
                    {
                        result = result.Where(x => x.KURUMKAYITNO == filtre.KURUMKAYITNO);
                    }
                }
            }
            return result;
        }

        public List<SYSROL> Getir(SYSROLAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<SYSROL> GetirWithContext(AppContext context, SYSROLAra filtre = null)
        {
            var filterHelper = new FilterHelper<SYSROLEf>();
            return filterHelper.Sayfala(Filtrele(context.SYSROLEf.AsQueryable(), filtre), filtre?.Ara).ToList<SYSROL>();
        }


        public List<SYSROLDetay> DetayGetir(SYSROLAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSROLDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.SYSROLEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public SYSROLDataTable Ara(SYSROLAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSROLDetay>();

                return new SYSROLDataTable
                {
                    SYSROLDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.SYSROLEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(SYSROLAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSROLEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.SYSROLEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public SYSROL Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.SYSROLEf.Find(id);
            }
        }

        public SYSROLDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.SYSROLEf.AsQueryable(), new SYSROLAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<SYSROL> Ekle(List<SYSROLEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.SYSROLEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<SYSROL> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.SYSROLEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<SYSROL>();
                }

                return null;
            }
        }


        public void Guncelle(List<SYSROLEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<SYSROLEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.SYSROLEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.SYSROLEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<SYSROLEf> mevcutDegerler, List<SYSROLEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("SYSROL bulunamadı");

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
                    var entry = context.SYSROLEf.Find(idler[0]);
                    if (entry != null)
                        context.SYSROLEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.SYSROLEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.SYSROLEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
