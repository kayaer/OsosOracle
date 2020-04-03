using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTHABERLESMEUNITESIComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfENTHABERLESMEUNITESIDal : IENTHABERLESMEUNITESIDal
    {


        private IQueryable<ENTHABERLESMEUNITESIDetay> DetayDoldur(IQueryable<ENTHABERLESMEUNITESIEf> result)
        {
            return result.Select(x => new ENTHABERLESMEUNITESIDetay()
            {
                //	ENTHABERLESMEUNITESI = x,
                KAYITNO = x.KAYITNO,
                SERINO = x.SERINO,
                SIMTELNO = x.SIMTELNO,
                IP = x.IP,
                ACIKLAMA = x.ACIKLAMA,
                DURUM = x.DURUM,
                VERSIYON = x.VERSIYON,
                MARKA = x.CstHuMarkaEf.AD,
                MODEL = x.CstHuModelEf.AD,
                KURUMKAYITNO = x.KURUMKAYITNO,
                Kurum=x.ConKurumEf.AD

            });
        }




        private IQueryable<ENTHABERLESMEUNITESIEf> Filtrele(IQueryable<ENTHABERLESMEUNITESIEf> result, ENTHABERLESMEUNITESIAra filtre = null)
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

                    if (!string.IsNullOrEmpty(filtre.SERINO))
                    {
                        result = result.Where(x => x.SERINO.Contains(filtre.SERINO));
                    }
                    if (!string.IsNullOrEmpty(filtre.SIMTELNO))
                    {
                        result = result.Where(x => x.SIMTELNO.Contains(filtre.SIMTELNO));
                    }
                    if (!string.IsNullOrEmpty(filtre.IP))
                    {
                        result = result.Where(x => x.IP.Contains(filtre.IP));
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
                    if (filtre.MARKAKAYITNO != null)
                    {
                        result = result.Where(x => x.MARKA == filtre.MARKAKAYITNO);
                    }
                    if (filtre.MODELKAYITNO != null)
                    {
                        result = result.Where(x => x.MODEL == filtre.MODELKAYITNO);
                    }
                    if (filtre.KURUMKAYITNO != null)
                    {
                        result = result.Where(x => x.KURUMKAYITNO == filtre.KURUMKAYITNO);
                    }
                }
            }
            return result;
        }

        public List<ENTHABERLESMEUNITESI> Getir(ENTHABERLESMEUNITESIAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<ENTHABERLESMEUNITESI> GetirWithContext(AppContext context, ENTHABERLESMEUNITESIAra filtre = null)
        {
            var filterHelper = new FilterHelper<ENTHABERLESMEUNITESIEf>();
            return filterHelper.Sayfala(Filtrele(context.ENTHABERLESMEUNITESIEf.AsQueryable(), filtre), filtre?.Ara).ToList<ENTHABERLESMEUNITESI>();
        }


        public List<ENTHABERLESMEUNITESIDetay> DetayGetir(ENTHABERLESMEUNITESIAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTHABERLESMEUNITESIDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTHABERLESMEUNITESIEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public ENTHABERLESMEUNITESIDataTable Ara(ENTHABERLESMEUNITESIAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTHABERLESMEUNITESIDetay>();

                return new ENTHABERLESMEUNITESIDataTable
                {
                    ENTHABERLESMEUNITESIDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTHABERLESMEUNITESIEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(ENTHABERLESMEUNITESIAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTHABERLESMEUNITESIEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.ENTHABERLESMEUNITESIEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public ENTHABERLESMEUNITESI Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.ENTHABERLESMEUNITESIEf.Find(id);
            }
        }

        public ENTHABERLESMEUNITESIDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.ENTHABERLESMEUNITESIEf.AsQueryable(), new ENTHABERLESMEUNITESIAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<ENTHABERLESMEUNITESI> Ekle(List<ENTHABERLESMEUNITESIEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.ENTHABERLESMEUNITESIEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<ENTHABERLESMEUNITESI> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.ENTHABERLESMEUNITESIEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<ENTHABERLESMEUNITESI>();
                }

                return null;
            }
        }


        public void Guncelle(List<ENTHABERLESMEUNITESIEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<ENTHABERLESMEUNITESIEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");

                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.ENTHABERLESMEUNITESIEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.ENTHABERLESMEUNITESIEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<ENTHABERLESMEUNITESIEf> mevcutDegerler, List<ENTHABERLESMEUNITESIEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("ENTHABERLESMEUNITESI bulunamadı");


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
                    var entry = context.ENTHABERLESMEUNITESIEf.Find(idler[0]);
                    if (entry != null)
                        context.ENTHABERLESMEUNITESIEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.ENTHABERLESMEUNITESIEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.ENTHABERLESMEUNITESIEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
