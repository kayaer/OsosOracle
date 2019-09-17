using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSKULLANICIDETAYComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfSYSKULLANICIDETAYDal : ISYSKULLANICIDETAYDal
    {


        private IQueryable<SYSKULLANICIDETAYDetay> DetayDoldur(IQueryable<SYSKULLANICIDETAYEf> result)
        {
            return result.Select(x => new SYSKULLANICIDETAYDetay()
            {
                //	SYSKULLANICIDETAY = x,
                KAYITNO = x.KAYITNO,

                EPOSTA = x.EPOSTA,
                GSM = x.GSM,
                KULLANICIKAYITNO = x.KULLANICIKAYITNO,

                //TODO: Ek detayları buraya ekleyiniz
                //örnek: SYSKULLANICIDETAYDurumu = x.NesneDegerDurumEf.Adi

            });
        }




        private IQueryable<SYSKULLANICIDETAYEf> Filtrele(IQueryable<SYSKULLANICIDETAYEf> result, SYSKULLANICIDETAYAra filtre = null)
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

                    if (!string.IsNullOrEmpty(filtre.EPOSTA))
                    {
                        result = result.Where(x => x.EPOSTA.Contains(filtre.EPOSTA));
                    }
                    if (filtre.GSM != null)
                    {
                        result = result.Where(x => x.GSM == filtre.GSM);
                    }
                    if (filtre.KULLANICIKAYITNO != null)
                    {
                        result = result.Where(x => x.KULLANICIKAYITNO == filtre.KULLANICIKAYITNO);
                    }
                }
            }
            return result;
        }

        public List<SYSKULLANICIDETAY> Getir(SYSKULLANICIDETAYAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<SYSKULLANICIDETAY> GetirWithContext(AppContext context, SYSKULLANICIDETAYAra filtre = null)
        {
            var filterHelper = new FilterHelper<SYSKULLANICIDETAYEf>();
            return filterHelper.Sayfala(Filtrele(context.SYSKULLANICIDETAYEf.AsQueryable(), filtre), filtre?.Ara).ToList<SYSKULLANICIDETAY>();
        }


        public List<SYSKULLANICIDETAYDetay> DetayGetir(SYSKULLANICIDETAYAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSKULLANICIDETAYDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.SYSKULLANICIDETAYEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public SYSKULLANICIDETAYDataTable Ara(SYSKULLANICIDETAYAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSKULLANICIDETAYDetay>();

                return new SYSKULLANICIDETAYDataTable
                {
                    SYSKULLANICIDETAYDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.SYSKULLANICIDETAYEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(SYSKULLANICIDETAYAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSKULLANICIDETAYEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.SYSKULLANICIDETAYEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public SYSKULLANICIDETAY Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.SYSKULLANICIDETAYEf.Find(id);
            }
        }

        public SYSKULLANICIDETAYDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.SYSKULLANICIDETAYEf.AsQueryable(), new SYSKULLANICIDETAYAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<SYSKULLANICIDETAY> Ekle(List<SYSKULLANICIDETAYEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.SYSKULLANICIDETAYEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<SYSKULLANICIDETAY> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.SYSKULLANICIDETAYEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<SYSKULLANICIDETAY>();
                }

                return null;
            }
        }


        public void Guncelle(List<SYSKULLANICIDETAYEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<SYSKULLANICIDETAYEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.SYSKULLANICIDETAYEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.SYSKULLANICIDETAYEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<SYSKULLANICIDETAYEf> mevcutDegerler, List<SYSKULLANICIDETAYEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("SYSKULLANICIDETAY bulunamadı");

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
                    var entry = context.SYSKULLANICIDETAYEf.Find(idler[0]);
                    if (entry != null)
                        context.SYSKULLANICIDETAYEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.SYSKULLANICIDETAYEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.SYSKULLANICIDETAYEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
