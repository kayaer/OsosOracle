using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTSATISComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfENTSATISDal : IENTSATISDal
    {


        private IQueryable<ENTSATISDetay> DetayDoldur(IQueryable<ENTSATISEf> result)
        {
            return result.Select(x => new ENTSATISDetay()
            {
                KAYITNO = x.KAYITNO,
                ABONEKAYITNO = x.ABONEKAYITNO,
                AboneNo = x.EntAboneEf.AboneBilgiEfCollection.FirstOrDefault().ABONENO,
                SAYACKAYITNO = x.SAYACKAYITNO,
                SayacSeriNo = x.EntSayacEf.SERINO,
                FATURANO = x.FATURANO,
                ODEME = x.ODEME,
                VERSIYON = x.VERSIYON,
                IPTAL = x.IPTAL,
                KREDI = x.KREDI,
                OLUSTURMATARIH = x.OLUSTURMATARIH
            });
        }




        private IQueryable<ENTSATISEf> Filtrele(IQueryable<ENTSATISEf> result, ENTSATISAra filtre = null)
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
                    if (filtre.FATURANO != null)
                    {
                        result = result.Where(x => x.FATURANO == filtre.FATURANO);
                    }
                    if (filtre.ODEME != null)
                    {
                        result = result.Where(x => x.ODEME == filtre.ODEME);
                    }
                    if (filtre.VERSIYON != null)
                    {
                        result = result.Where(x => x.VERSIYON == filtre.VERSIYON);
                    }
                    if (filtre.IPTAL != null)
                    {
                        result = result.Where(x => x.IPTAL == filtre.IPTAL);
                    }
                    if (filtre.KREDI != null)
                    {
                        result = result.Where(x => x.KREDI == filtre.KREDI);
                    }
                    if (filtre.YEDEKKREDI != null)
                    {
                        result = result.Where(x => x.YEDEKKREDI == filtre.YEDEKKREDI);
                    }
                }
            }
            return result;
        }

        public List<ENTSATIS> Getir(ENTSATISAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<ENTSATIS> GetirWithContext(AppContext context, ENTSATISAra filtre = null)
        {
            var filterHelper = new FilterHelper<ENTSATISEf>();
            return filterHelper.Sayfala(Filtrele(context.ENTSATISEf.AsQueryable(), filtre), filtre?.Ara).ToList<ENTSATIS>();
        }


        public List<ENTSATISDetay> DetayGetir(ENTSATISAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTSATISDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTSATISEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public ENTSATISDataTable Ara(ENTSATISAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTSATISDetay>();

                return new ENTSATISDataTable
                {
                    ENTSATISDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTSATISEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(ENTSATISAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTSATISEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.ENTSATISEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public ENTSATIS Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.ENTSATISEf.Find(id);
            }
        }

        public ENTSATISDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.ENTSATISEf.AsQueryable(), new ENTSATISAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<ENTSATIS> Ekle(List<ENTSATISEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.ENTSATISEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<ENTSATIS> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.ENTSATISEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<ENTSATIS>();
                }

                return null;
            }
        }


        public void Guncelle(List<ENTSATISEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<ENTSATISEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.ENTSATISEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.ENTSATISEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<ENTSATISEf> mevcutDegerler, List<ENTSATISEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("ENTSATIS bulunamadı");

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
                    var entry = context.ENTSATISEf.Find(idler[0]);
                    if (entry != null)
                        context.ENTSATISEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.ENTSATISEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.ENTSATISEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
