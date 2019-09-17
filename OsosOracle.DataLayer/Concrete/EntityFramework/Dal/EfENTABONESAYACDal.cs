using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfENTABONESAYACDal : IENTABONESAYACDal
    {


        private IQueryable<ENTABONESAYACDetay> DetayDoldur(IQueryable<ENTABONESAYACEf> result)
        {
            return result.Select(x => new ENTABONESAYACDetay()
            {
                KAYITNO = x.KAYITNO,
                TARIFE = x.TARIFE,
                TARIFEKAYITNO = x.TARIFEKAYITNO,
                ABONEKAYITNO = x.ABONEKAYITNO,
                SAYACKAYITNO = x.SAYACKAYITNO,


                //TODO: Ek detayları buraya ekleyiniz
                //örnek: ENTABONESAYACDurumu = x.NesneDegerDurumEf.Adi

            });
        }




        private IQueryable<ENTABONESAYACEf> Filtrele(IQueryable<ENTABONESAYACEf> result, ENTABONESAYACAra filtre = null)
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

                    if (filtre.ABONEKAYITNO != null)
                    {
                        result = result.Where(x => x.ABONEKAYITNO == filtre.ABONEKAYITNO);
                    }
                    if (filtre.SAYACKAYITNO != null)
                    {
                        result = result.Where(x => x.SAYACKAYITNO == filtre.SAYACKAYITNO);
                    }
                    if (filtre.SOKULMENEDEN != null)
                    {
                        result = result.Where(x => x.SOKULMENEDEN == filtre.SOKULMENEDEN);
                    }
                    if (filtre.SONENDEKS != null)
                    {
                        result = result.Where(x => x.SONENDEKS == filtre.SONENDEKS);
                    }
                    if (filtre.SOKULMETARIH != null)
                    {
                        result = result.Where(x => x.SOKULMETARIH == filtre.SOKULMETARIH);
                    }
                    if (filtre.KARTNO != null)
                    {
                        result = result.Where(x => x.KARTNO == filtre.KARTNO);
                    }
                    if (filtre.VERSIYON != null)
                    {
                        result = result.Where(x => x.VERSIYON == filtre.VERSIYON);
                    }
                    if (filtre.TARIFEKAYITNO != null)
                    {
                        result = result.Where(x => x.TARIFEKAYITNO == filtre.TARIFEKAYITNO);
                    }
                    if (filtre.TARIFE != null)
                    {
                        result = result.Where(x => x.TARIFE == filtre.TARIFE);
                    }
                    if (filtre.TAKILMATARIH != null)
                    {
                        result = result.Where(x => x.TAKILMATARIH == filtre.TAKILMATARIH);
                    }
                    if (filtre.SONSATISKAYITNO != null)
                    {
                        result = result.Where(x => x.SONSATISKAYITNO == filtre.SONSATISKAYITNO);
                    }
                    if (filtre.SONSATISTARIH != null)
                    {
                        result = result.Where(x => x.SONSATISTARIH == filtre.SONSATISTARIH);
                    }
                }
            }
            return result;
        }

        public List<ENTABONESAYAC> Getir(ENTABONESAYACAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<ENTABONESAYAC> GetirWithContext(AppContext context, ENTABONESAYACAra filtre = null)
        {
            var filterHelper = new FilterHelper<ENTABONESAYACEf>();
            return filterHelper.Sayfala(Filtrele(context.ENTABONESAYACEf.AsQueryable(), filtre), filtre?.Ara).ToList<ENTABONESAYAC>();
        }


        public List<ENTABONESAYACDetay> DetayGetir(ENTABONESAYACAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTABONESAYACDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTABONESAYACEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public ENTABONESAYACDataTable Ara(ENTABONESAYACAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTABONESAYACDetay>();

                return new ENTABONESAYACDataTable
                {
                    ENTABONESAYACDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTABONESAYACEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(ENTABONESAYACAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTABONESAYACEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.ENTABONESAYACEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public ENTABONESAYAC Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.ENTABONESAYACEf.Find(id);
            }
        }

        public ENTABONESAYACDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.ENTABONESAYACEf.AsQueryable(), new ENTABONESAYACAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<ENTABONESAYAC> Ekle(List<ENTABONESAYACEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.ENTABONESAYACEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<ENTABONESAYAC> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.ENTABONESAYACEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<ENTABONESAYAC>();
                }

                return null;
            }
        }


        public void Guncelle(List<ENTABONESAYACEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<ENTABONESAYACEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.ENTABONESAYACEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.ENTABONESAYACEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<ENTABONESAYACEf> mevcutDegerler, List<ENTABONESAYACEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("ENTABONESAYAC bulunamadı");

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
                    var entry = context.ENTABONESAYACEf.Find(idler[0]);
                    if (entry != null)
                        context.ENTABONESAYACEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.ENTABONESAYACEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.ENTABONESAYACEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
