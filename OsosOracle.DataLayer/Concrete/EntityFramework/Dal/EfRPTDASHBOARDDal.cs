using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.RPTDASHBOARDComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfRPTDASHBOARDDal : IRPTDASHBOARDDal
    {


        private IQueryable<RPTDASHBOARDDetay> DetayDoldur(IQueryable<RPTDASHBOARDEf> result)
        {
            return result.Select(x => new RPTDASHBOARDDetay()
            {
                //	RPTDASHBOARD = x,
                KAYITNO = x.KAYITNO,
                TARIH = x.TARIH,
                ADET = x.ADET,
                KURUMKAYITNO = x.KURUMKAYITNO,

                //TODO: Ek detayları buraya ekleyiniz
                //örnek: RPTDASHBOARDDurumu = x.NesneDegerDurumEf.Adi

            });
        }




        private IQueryable<RPTDASHBOARDEf> Filtrele(IQueryable<RPTDASHBOARDEf> result, RPTDASHBOARDAra filtre = null)
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

                    if (filtre.TARIH != null)
                    {
                        result = result.Where(x => x.TARIH == filtre.TARIH);
                    }
                    if (filtre.ADET != null)
                    {
                        result = result.Where(x => x.ADET == filtre.ADET);
                    }
                    if (filtre.KURUMKAYITNO != null)
                    {
                        result = result.Where(x => x.KURUMKAYITNO == filtre.KURUMKAYITNO);
                    }
                }
            }
            return result;
        }

        public List<RPTDASHBOARD> Getir(RPTDASHBOARDAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<RPTDASHBOARD> GetirWithContext(AppContext context, RPTDASHBOARDAra filtre = null)
        {
            var filterHelper = new FilterHelper<RPTDASHBOARDEf>();
            return filterHelper.Sayfala(Filtrele(context.RPTDASHBOARDEf.AsQueryable(), filtre), filtre?.Ara).ToList<RPTDASHBOARD>();
        }


        public List<RPTDASHBOARDDetay> DetayGetir(RPTDASHBOARDAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<RPTDASHBOARDDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.RPTDASHBOARDEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public RPTDASHBOARDDataTable Ara(RPTDASHBOARDAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<RPTDASHBOARDDetay>();

                return new RPTDASHBOARDDataTable
                {
                    RPTDASHBOARDDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.RPTDASHBOARDEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(RPTDASHBOARDAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<RPTDASHBOARDEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.RPTDASHBOARDEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public RPTDASHBOARD Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.RPTDASHBOARDEf.Find(id);
            }
        }

        public RPTDASHBOARDDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.RPTDASHBOARDEf.AsQueryable(), new RPTDASHBOARDAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<RPTDASHBOARD> Ekle(List<RPTDASHBOARDEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.RPTDASHBOARDEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<RPTDASHBOARD> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.RPTDASHBOARDEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<RPTDASHBOARD>();
                }

                return null;
            }
        }


        public void Guncelle(List<RPTDASHBOARDEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<RPTDASHBOARDEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.RPTDASHBOARDEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.RPTDASHBOARDEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<RPTDASHBOARDEf> mevcutDegerler, List<RPTDASHBOARDEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("RPTDASHBOARD bulunamadı");

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
                    var entry = context.RPTDASHBOARDEf.Find(idler[0]);
                    if (entry != null)
                        context.RPTDASHBOARDEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.RPTDASHBOARDEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.RPTDASHBOARDEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
