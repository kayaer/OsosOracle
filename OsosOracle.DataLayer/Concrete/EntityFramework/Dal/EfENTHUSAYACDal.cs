using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTHUSAYACComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfENTHUSAYACDal : IENTHUSAYACDal
    {


        private IQueryable<ENTHUSAYACDetay> DetayDoldur(IQueryable<ENTHUSAYACEf> result)
        {
            return result.Select(x => new ENTHUSAYACDetay()
            {
                //	ENTHUSAYAC = x,
                // Id = x.Id,

                HUKAYITNO = x.HUKAYITNO,
                SAYACKAYITNO = x.SAYACKAYITNO,
                VERSIYON = x.VERSIYON,
                SAYACID=x.ENTSAYACSAYACKAYITNOEf.SAYACID
                //TODO: Ek detayları buraya ekleyiniz
                //örnek: ENTHUSAYACDurumu = x.NesneDegerDurumEf.Adi

            });
        }




        private IQueryable<ENTHUSAYACEf> Filtrele(IQueryable<ENTHUSAYACEf> result, ENTHUSAYACAra filtre = null)
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

                    if (filtre.HUKAYITNO != null)
                    {
                        result = result.Where(x => x.HUKAYITNO == filtre.HUKAYITNO);
                    }
                    if (filtre.SAYACKAYITNO != null)
                    {
                        result = result.Where(x => x.SAYACKAYITNO == filtre.SAYACKAYITNO);
                    }
                    if (filtre.VERSIYON != null)
                    {
                        result = result.Where(x => x.VERSIYON == filtre.VERSIYON);
                    }
                }
            }
            return result;
        }

        public List<ENTHUSAYAC> Getir(ENTHUSAYACAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<ENTHUSAYAC> GetirWithContext(AppContext context, ENTHUSAYACAra filtre = null)
        {
            var filterHelper = new FilterHelper<ENTHUSAYACEf>();
            return filterHelper.Sayfala(Filtrele(context.ENTHUSAYACEf.AsQueryable(), filtre), filtre?.Ara).ToList<ENTHUSAYAC>();
        }


        public List<ENTHUSAYACDetay> DetayGetir(ENTHUSAYACAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTHUSAYACDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTHUSAYACEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public ENTHUSAYACDataTable Ara(ENTHUSAYACAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTHUSAYACDetay>();

                return new ENTHUSAYACDataTable
                {
                    ENTHUSAYACDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTHUSAYACEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(ENTHUSAYACAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTHUSAYACEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.ENTHUSAYACEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public ENTHUSAYAC Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.ENTHUSAYACEf.Find(id);
            }
        }

        public ENTHUSAYACDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.ENTHUSAYACEf.AsQueryable(), new ENTHUSAYACAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<ENTHUSAYAC> Ekle(List<ENTHUSAYACEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.ENTHUSAYACEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<ENTHUSAYAC> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.ENTHUSAYACEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<ENTHUSAYAC>();
                }

                return null;
            }
        }


        public void Guncelle(List<ENTHUSAYACEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<ENTHUSAYACEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");

                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.ENTHUSAYACEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.ENTHUSAYACEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<ENTHUSAYACEf> mevcutDegerler, List<ENTHUSAYACEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("ENTHUSAYAC bulunamadı");


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
                    var entry = context.ENTHUSAYACEf.Find(idler[0]);
                    if (entry != null)
                        context.ENTHUSAYACEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.ENTHUSAYACEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.ENTHUSAYACEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
