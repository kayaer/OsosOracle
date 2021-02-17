using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSKULLANICIComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfSYSKULLANICIDal : ISYSKULLANICIDal
    {


        private IQueryable<SYSKULLANICIDetay> DetayDoldur(IQueryable<SYSKULLANICIEf> result)
        {
            return result.Select(x => new SYSKULLANICIDetay()
            {
                KAYITNO=x.KAYITNO,
                KULLANICIAD = x.KULLANICIAD,
                SIFRE = x.SIFRE,
                VERSIYON = x.VERSIYON,
                AD = x.AD,
                SOYAD = x.SOYAD,
                DURUM = x.DURUM,
                DIL = x.DIL,
                KURUMKAYITNO = x.KURUMKAYITNO,
                KurumAdi=x.ConKurumEf.AD,
                KurumDllAdi=x.ConKurumEf.DllAdi
            
            });
        }




        private IQueryable<SYSKULLANICIEf> Filtrele(IQueryable<SYSKULLANICIEf> result, SYSKULLANICIAra filtre = null)
        {
            //silindi kolonu varsa silinenler gelmesin
            result = result.Where(x => x.DURUM == 1);
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

                    if (!string.IsNullOrEmpty(filtre.KULLANICIAD))
                    {
                        result = result.Where(x => x.KULLANICIAD==filtre.KULLANICIAD);
                    }
                    if (!string.IsNullOrEmpty(filtre.SIFRE))
                    {
                        result = result.Where(x => x.SIFRE==filtre.SIFRE);
                    }
                    
                    if (filtre.VERSIYON != null)
                    {
                        result = result.Where(x => x.VERSIYON == filtre.VERSIYON);
                    }
                    if (!string.IsNullOrEmpty(filtre.AD))
                    {
                        result = result.Where(x => x.AD.Contains(filtre.AD));
                    }
                    if (!string.IsNullOrEmpty(filtre.SOYAD))
                    {
                        result = result.Where(x => x.SOYAD.Contains(filtre.SOYAD));
                    }
                    
                    if (filtre.DURUM != null)
                    {
                        result = result.Where(x => x.DURUM == filtre.DURUM);
                    }
                   
                    if (filtre.DIL != null)
                    {
                        result = result.Where(x => x.DIL == filtre.DIL);
                    }
                    
                    if (filtre.KURUMKAYITNO != null)
                    {
                        result = result.Where(x => x.KURUMKAYITNO == filtre.KURUMKAYITNO);
                    }
                }
            }
            return result;
        }

        public List<SYSKULLANICI> Getir(SYSKULLANICIAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<SYSKULLANICI> GetirWithContext(AppContext context, SYSKULLANICIAra filtre = null)
        {
            var filterHelper = new FilterHelper<SYSKULLANICIEf>();
            return filterHelper.Sayfala(Filtrele(context.SYSKULLANICIEf.AsQueryable(), filtre), filtre?.Ara).ToList<SYSKULLANICI>();
        }


        public List<SYSKULLANICIDetay> DetayGetir(SYSKULLANICIAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSKULLANICIDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.SYSKULLANICIEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public SYSKULLANICIDataTable Ara(SYSKULLANICIAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSKULLANICIDetay>();

                return new SYSKULLANICIDataTable
                {
                    SYSKULLANICIDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.SYSKULLANICIEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(SYSKULLANICIAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSKULLANICIEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.SYSKULLANICIEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public SYSKULLANICI Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.SYSKULLANICIEf.Find(id);
            }
        }

        public SYSKULLANICIDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.SYSKULLANICIEf.AsQueryable(), new SYSKULLANICIAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<SYSKULLANICI> Ekle(List<SYSKULLANICIEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.SYSKULLANICIEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<SYSKULLANICI> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.SYSKULLANICIEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<SYSKULLANICI>();
                }

                return null;
            }
        }


        public void Guncelle(List<SYSKULLANICIEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<SYSKULLANICIEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.SYSKULLANICIEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.SYSKULLANICIEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<SYSKULLANICIEf> mevcutDegerler, List<SYSKULLANICIEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("SYSKULLANICI bulunamadı");

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
                    var entry = context.SYSKULLANICIEf.Find(idler[0]);
                    if (entry != null)
                        context.SYSKULLANICIEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.SYSKULLANICIEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.SYSKULLANICIEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
