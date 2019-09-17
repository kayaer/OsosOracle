using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSROLKULLANICIComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfSYSROLKULLANICIDal : ISYSROLKULLANICIDal
    {


        private IQueryable<SYSROLKULLANICIDetay> DetayDoldur(IQueryable<SYSROLKULLANICIEf> result)
        {
            return result.Select(x => new SYSROLKULLANICIDetay()
            {
                KAYITNO=x.KAYITNO,
                KULLANICIKAYITNO = x.KULLANICIKAYITNO,
                ROLKAYITNO = x.ROLKAYITNO,
                VERSIYON = x.VERSIYON,
                RolAdi=x.SysRolEf.AD,
                KullaniciAdi=x.SysKullaniciEf.KULLANICIAD

            });
        }




        private IQueryable<SYSROLKULLANICIEf> Filtrele(IQueryable<SYSROLKULLANICIEf> result, SYSROLKULLANICIAra filtre = null)
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

                    if (filtre.KULLANICIKAYITNO != null)
                    {
                        result = result.Where(x => x.KULLANICIKAYITNO == filtre.KULLANICIKAYITNO);
                    }
                    if (filtre.ROLKAYITNO != null)
                    {
                        result = result.Where(x => x.ROLKAYITNO == filtre.ROLKAYITNO);
                    }
                    if (filtre.VERSIYON != null)
                    {
                        result = result.Where(x => x.VERSIYON == filtre.VERSIYON);
                    }
                }
            }
            return result;
        }

        public List<SYSROLKULLANICI> Getir(SYSROLKULLANICIAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<SYSROLKULLANICI> GetirWithContext(AppContext context, SYSROLKULLANICIAra filtre = null)
        {
            var filterHelper = new FilterHelper<SYSROLKULLANICIEf>();
            return filterHelper.Sayfala(Filtrele(context.SYSROLKULLANICIEf.AsQueryable(), filtre), filtre?.Ara).ToList<SYSROLKULLANICI>();
        }


        public List<SYSROLKULLANICIDetay> DetayGetir(SYSROLKULLANICIAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSROLKULLANICIDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.SYSROLKULLANICIEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public SYSROLKULLANICIDataTable Ara(SYSROLKULLANICIAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSROLKULLANICIDetay>();

                return new SYSROLKULLANICIDataTable
                {
                    SYSROLKULLANICIDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.SYSROLKULLANICIEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(SYSROLKULLANICIAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSROLKULLANICIEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.SYSROLKULLANICIEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public SYSROLKULLANICI Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.SYSROLKULLANICIEf.Find(id);
            }
        }

        public SYSROLKULLANICIDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.SYSROLKULLANICIEf.AsQueryable(), new SYSROLKULLANICIAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<SYSROLKULLANICI> Ekle(List<SYSROLKULLANICIEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.SYSROLKULLANICIEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<SYSROLKULLANICI> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.SYSROLKULLANICIEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<SYSROLKULLANICI>();
                }

                return null;
            }
        }


        public void Guncelle(List<SYSROLKULLANICIEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<SYSROLKULLANICIEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.SYSROLKULLANICIEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.SYSROLKULLANICIEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<SYSROLKULLANICIEf> mevcutDegerler, List<SYSROLKULLANICIEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("SYSROLKULLANICI bulunamadı");

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
                    var entry = context.SYSROLKULLANICIEf.Find(idler[0]);
                    if (entry != null)
                        context.SYSROLKULLANICIEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.SYSROLKULLANICIEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.SYSROLKULLANICIEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

        public bool RolKullaniciSil(int? rolid, int? kullaniciId)
        {
            if (rolid != null)
            {
                using (var context = new AppContext())
                {
                    var entry = context.SYSROLKULLANICIEf.Where(x => x.ROLKAYITNO == rolid).ToList();
                    if (entry.Any())
                        context.SYSROLKULLANICIEf.RemoveRange(entry);

                    context.SaveChanges();
                }
            }
            else if (kullaniciId != null)
            {
                using (var context = new AppContext())
                {
                    var entry = context.SYSROLKULLANICIEf.Where(x => x.KULLANICIKAYITNO == kullaniciId).ToList();
                    if (entry.Any())
                        context.SYSROLKULLANICIEf.RemoveRange(entry);

                    context.SaveChanges();
                }
            }
            
           
            return true;
        }
    }
}
