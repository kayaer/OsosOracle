using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSCSTOPERASYONComplexTypes;
using OsosOracle.Entities.ComplexType.SYSGOREVComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfSYSGOREVDal : ISYSGOREVDal
    {


        private IQueryable<SYSGOREVDetay> DetayDoldur(IQueryable<SYSGOREVEf> result)
        {
            return result.Select(x => new SYSGOREVDetay()
            {
                //	SYSGOREV = x,
                KAYITNO = x.KAYITNO,

                AD = x.AD,
                ACIKLAMA = x.ACIKLAMA,
                VERSIYON = x.VERSIYON,
                KURUMKAYITNO = x.KURUMKAYITNO,
                SysOperasyonList = x.GOREVKAYITNOSYSOPERASYONGOREVEfCollection.Select(a => new SYSCSTOPERASYONDetay() { KAYITNO = a.SysCstOperasyonEf.KAYITNO, AD = a.SysCstOperasyonEf.AD }).ToList(),
                KurumAdi=x.ConKurumEf.AD

            });
        }




        private IQueryable<SYSGOREVEf> Filtrele(IQueryable<SYSGOREVEf> result, SYSGOREVAra filtre = null)
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

        public List<SYSGOREV> Getir(SYSGOREVAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<SYSGOREV> GetirWithContext(AppContext context, SYSGOREVAra filtre = null)
        {
            var filterHelper = new FilterHelper<SYSGOREVEf>();
            return filterHelper.Sayfala(Filtrele(context.SYSGOREVEf.AsQueryable(), filtre), filtre?.Ara).ToList<SYSGOREV>();
        }


        public List<SYSGOREVDetay> DetayGetir(SYSGOREVAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSGOREVDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.SYSGOREVEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public SYSGOREVDataTable Ara(SYSGOREVAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSGOREVDetay>();

                return new SYSGOREVDataTable
                {
                    SYSGOREVDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.SYSGOREVEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(SYSGOREVAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSGOREVEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.SYSGOREVEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public SYSGOREV Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.SYSGOREVEf.Find(id);
            }
        }

        public SYSGOREVDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.SYSGOREVEf.AsQueryable(), new SYSGOREVAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<SYSGOREV> Ekle(List<SYSGOREVEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.SYSGOREVEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<SYSGOREV> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.SYSGOREVEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<SYSGOREV>();
                }

                return null;
            }
        }


        public void Guncelle(List<SYSGOREVEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<SYSGOREVEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.SYSGOREVEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.SYSGOREVEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<SYSGOREVEf> mevcutDegerler, List<SYSGOREVEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("SYSGOREV bulunamadı");

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
                    var entry = context.SYSGOREVEf.Find(idler[0]);
                    if (entry != null)
                        context.SYSGOREVEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.SYSGOREVEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.SYSGOREVEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
