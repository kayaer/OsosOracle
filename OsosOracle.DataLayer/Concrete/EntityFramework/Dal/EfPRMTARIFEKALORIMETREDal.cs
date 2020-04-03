using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.PRMTARIFEKALORIMETREComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfPRMTARIFEKALORIMETREDal : IPRMTARIFEKALORIMETREDal
    {


        private IQueryable<PRMTARIFEKALORIMETREDetay> DetayDoldur(IQueryable<PRMTARIFEKALORIMETREEf> result)
        {
            return result.Select(x => new PRMTARIFEKALORIMETREDetay()
            {
                KAYITNO = x.KAYITNO,
                AD = x.AD,
                YEDEKKREDI = x.YEDEKKREDI,
                ACIKLAMA = x.ACIKLAMA,
                DURUM = x.DURUM,
                FIYAT1 = x.FIYAT1,
                FIYAT2 = x.FIYAT2,
                FIYAT3 = x.FIYAT3,
                FIYAT4 = x.FIYAT4,
                LIMIT1 = x.LIMIT1,
                LIMIT2 = x.LIMIT2,
                Kdv = x.Kdv,
                Ctv = x.Ctv,
                SAYACTIP = x.SAYACTIP,
                TUKETIMKATSAYI = x.TUKETIMKATSAYI,
                KREDIKATSAYI = x.KREDIKATSAYI,
                BAYRAM1GUN = x.BAYRAM1GUN,
                BAYRAM1AY = x.BAYRAM1AY,
                BAYRAM1SURE = x.BAYRAM1SURE,
                BAYRAM2GUN = x.BAYRAM2GUN,
                BAYRAM2AY = x.BAYRAM2AY,
                BAYRAM2SURE = x.BAYRAM2SURE,
                KURUMKAYITNO = x.KURUMKAYITNO,
                Kurum=x.ConKurumEf.AD,
                AylikBakimBedeli=x.AylikBakimBedeli,
                BirimFiyat=x.BirimFiyat,
                RezervKredi=x.RezervKredi

            });
        }




        private IQueryable<PRMTARIFEKALORIMETREEf> Filtrele(IQueryable<PRMTARIFEKALORIMETREEf> result, PRMTARIFEKALORIMETREAra filtre = null)
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
                    if (filtre.YEDEKKREDI != null)
                    {
                        result = result.Where(x => x.YEDEKKREDI == filtre.YEDEKKREDI);
                    }
                    if (!string.IsNullOrEmpty(filtre.ACIKLAMA))
                    {
                        result = result.Where(x => x.ACIKLAMA.Contains(filtre.ACIKLAMA));
                    }
                    if (filtre.DURUM != null)
                    {
                        result = result.Where(x => x.DURUM == filtre.DURUM);
                    }
                    if (filtre.FIYAT1 != null)
                    {
                        result = result.Where(x => x.FIYAT1 == filtre.FIYAT1);
                    }
                    if (filtre.FIYAT2 != null)
                    {
                        result = result.Where(x => x.FIYAT2 == filtre.FIYAT2);
                    }
                    if (filtre.FIYAT3 != null)
                    {
                        result = result.Where(x => x.FIYAT3 == filtre.FIYAT3);
                    }
                    if (filtre.FIYAT4 != null)
                    {
                        result = result.Where(x => x.FIYAT4 == filtre.FIYAT4);
                    }
                    if (filtre.LIMIT1 != null)
                    {
                        result = result.Where(x => x.LIMIT1 == filtre.LIMIT1);
                    }
                    if (filtre.LIMIT2 != null)
                    {
                        result = result.Where(x => x.LIMIT2 == filtre.LIMIT2);
                    }
                    if (filtre.Kdv != null)
                    {
                        result = result.Where(x => x.Kdv == filtre.Kdv);
                    }
                    if (filtre.Ctv != null)
                    {
                        result = result.Where(x => x.Ctv == filtre.Ctv);
                    }
                    
                    if (filtre.SAYACTIP != null)
                    {
                        result = result.Where(x => x.SAYACTIP == filtre.SAYACTIP);
                    }
                    if (filtre.TUKETIMKATSAYI != null)
                    {
                        result = result.Where(x => x.TUKETIMKATSAYI == filtre.TUKETIMKATSAYI);
                    }
                    if (filtre.KREDIKATSAYI != null)
                    {
                        result = result.Where(x => x.KREDIKATSAYI == filtre.KREDIKATSAYI);
                    }
                    if (filtre.BAYRAM1GUN != null)
                    {
                        result = result.Where(x => x.BAYRAM1GUN == filtre.BAYRAM1GUN);
                    }
                    if (filtre.BAYRAM1AY != null)
                    {
                        result = result.Where(x => x.BAYRAM1AY == filtre.BAYRAM1AY);
                    }
                    if (filtre.BAYRAM1SURE != null)
                    {
                        result = result.Where(x => x.BAYRAM1SURE == filtre.BAYRAM1SURE);
                    }
                    if (filtre.BAYRAM2GUN != null)
                    {
                        result = result.Where(x => x.BAYRAM2GUN == filtre.BAYRAM2GUN);
                    }
                    if (filtre.BAYRAM2AY != null)
                    {
                        result = result.Where(x => x.BAYRAM2AY == filtre.BAYRAM2AY);
                    }
                    if (filtre.BAYRAM2SURE != null)
                    {
                        result = result.Where(x => x.BAYRAM2SURE == filtre.BAYRAM2SURE);
                    }
                    if (filtre.KURUMKAYITNO != null)
                    {
                        result = result.Where(x => x.KURUMKAYITNO == filtre.KURUMKAYITNO);
                    }
                }
            }
            return result;
        }

        public List<PRMTARIFEKALORIMETRE> Getir(PRMTARIFEKALORIMETREAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<PRMTARIFEKALORIMETRE> GetirWithContext(AppContext context, PRMTARIFEKALORIMETREAra filtre = null)
        {
            var filterHelper = new FilterHelper<PRMTARIFEKALORIMETREEf>();
            return filterHelper.Sayfala(Filtrele(context.PRMTARIFEKALORIMETREEf.AsQueryable(), filtre), filtre?.Ara).ToList<PRMTARIFEKALORIMETRE>();
        }


        public List<PRMTARIFEKALORIMETREDetay> DetayGetir(PRMTARIFEKALORIMETREAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<PRMTARIFEKALORIMETREDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.PRMTARIFEKALORIMETREEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public PRMTARIFEKALORIMETREDataTable Ara(PRMTARIFEKALORIMETREAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<PRMTARIFEKALORIMETREDetay>();

                return new PRMTARIFEKALORIMETREDataTable
                {
                    PRMTARIFEKALORIMETREDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.PRMTARIFEKALORIMETREEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(PRMTARIFEKALORIMETREAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<PRMTARIFEKALORIMETREEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.PRMTARIFEKALORIMETREEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public PRMTARIFEKALORIMETRE Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.PRMTARIFEKALORIMETREEf.Find(id);
            }
        }

        public PRMTARIFEKALORIMETREDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.PRMTARIFEKALORIMETREEf.AsQueryable(), new PRMTARIFEKALORIMETREAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<PRMTARIFEKALORIMETRE> Ekle(List<PRMTARIFEKALORIMETREEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.PRMTARIFEKALORIMETREEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<PRMTARIFEKALORIMETRE> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.PRMTARIFEKALORIMETREEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<PRMTARIFEKALORIMETRE>();
                }

                return null;
            }
        }


        public void Guncelle(List<PRMTARIFEKALORIMETREEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<PRMTARIFEKALORIMETREEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.PRMTARIFEKALORIMETREEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.PRMTARIFEKALORIMETREEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<PRMTARIFEKALORIMETREEf> mevcutDegerler, List<PRMTARIFEKALORIMETREEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("PRMTARIFEKALORIMETRE bulunamadı");

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
                    var entry = context.PRMTARIFEKALORIMETREEf.Find(idler[0]);
                    if (entry != null)
                        context.PRMTARIFEKALORIMETREEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.PRMTARIFEKALORIMETREEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.PRMTARIFEKALORIMETREEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
