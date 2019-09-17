using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.PRMTARIFESUComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfPRMTARIFESUDal : IPRMTARIFESUDal
    {


        private IQueryable<PRMTARIFESUDetay> DetayDoldur(IQueryable<PRMTARIFESUEf> result)
        {
            return result.Select(x => new PRMTARIFESUDetay()
            {
                KAYITNO = x.KAYITNO,
                BORCYUZDE = x.BORCYUZDE,
                AD = x.AD,
                YEDEKKREDI = x.YEDEKKREDI,
                ACIKLAMA = x.ACIKLAMA,
                DURUM = x.DURUM,
                FIYAT1 = x.FIYAT1,
                FIYAT2 = x.FIYAT2,
                FIYAT3 = x.FIYAT3,
                FIYAT4 = x.FIYAT4,
                FIYAT5 = x.FIYAT5,
                LIMIT1 = x.LIMIT1,
                LIMIT2 = x.LIMIT2,
                LIMIT3 = x.LIMIT3,
                LIMIT4 = x.LIMIT4,
                TUKETIMKATSAYI = x.TUKETIMKATSAYI,
                KREDIKATSAYI = x.KREDIKATSAYI,
                SABITUCRET = x.SABITUCRET,
                SAYACCAP = x.SAYACCAP,
                AVANSONAY = x.AVANSONAY,
                DONEMGUN = x.DONEMGUN,
                BAYRAM1GUN = x.BAYRAM1GUN,
                BAYRAM1AY = x.BAYRAM1AY,
                BAYRAM1SURE = x.BAYRAM1SURE,
                BAYRAM2GUN = x.BAYRAM2GUN,
                BAYRAM2AY = x.BAYRAM2AY,
                BAYRAM2SURE = x.BAYRAM2SURE,
                MAXDEBI = x.MAXDEBI,
                KRITIKKREDI = x.KRITIKKREDI,
                KURUMKAYITNO = x.KURUMKAYITNO,
                BAGLANTIPERIYOT = x.BAGLANTIPERIYOT,
                YANGINMODSURE = x.YANGINMODSURE,
                BIRIMFIYAT = x.BIRIMFIYAT,
                Kurum=x.ConKurumEf.AD

            });
        }




        private IQueryable<PRMTARIFESUEf> Filtrele(IQueryable<PRMTARIFESUEf> result, PRMTARIFESUAra filtre = null)
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

                    if (filtre.BORCYUZDE != null)
                    {
                        result = result.Where(x => x.BORCYUZDE == filtre.BORCYUZDE);
                    }
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
                    if (filtre.FIYAT5 != null)
                    {
                        result = result.Where(x => x.FIYAT5 == filtre.FIYAT5);
                    }
                    if (filtre.LIMIT1 != null)
                    {
                        result = result.Where(x => x.LIMIT1 == filtre.LIMIT1);
                    }
                    if (filtre.LIMIT2 != null)
                    {
                        result = result.Where(x => x.LIMIT2 == filtre.LIMIT2);
                    }
                    if (filtre.LIMIT3 != null)
                    {
                        result = result.Where(x => x.LIMIT3 == filtre.LIMIT3);
                    }
                    if (filtre.LIMIT4 != null)
                    {
                        result = result.Where(x => x.LIMIT4 == filtre.LIMIT4);
                    }
                    if (filtre.TUKETIMKATSAYI != null)
                    {
                        result = result.Where(x => x.TUKETIMKATSAYI == filtre.TUKETIMKATSAYI);
                    }
                    if (filtre.KREDIKATSAYI != null)
                    {
                        result = result.Where(x => x.KREDIKATSAYI == filtre.KREDIKATSAYI);
                    }
                    if (filtre.SABITUCRET != null)
                    {
                        result = result.Where(x => x.SABITUCRET == filtre.SABITUCRET);
                    }
                    if (filtre.SAYACCAP != null)
                    {
                        result = result.Where(x => x.SAYACCAP == filtre.SAYACCAP);
                    }
                    if (filtre.AVANSONAY != null)
                    {
                        result = result.Where(x => x.AVANSONAY == filtre.AVANSONAY);
                    }
                    if (filtre.DONEMGUN != null)
                    {
                        result = result.Where(x => x.DONEMGUN == filtre.DONEMGUN);
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
                    if (filtre.MAXDEBI != null)
                    {
                        result = result.Where(x => x.MAXDEBI == filtre.MAXDEBI);
                    }
                    if (filtre.KRITIKKREDI != null)
                    {
                        result = result.Where(x => x.KRITIKKREDI == filtre.KRITIKKREDI);
                    }
                    if (filtre.KURUMKAYITNO != null)
                    {
                        result = result.Where(x => x.KURUMKAYITNO == filtre.KURUMKAYITNO);
                    }
                    if (filtre.BAGLANTIPERIYOT != null)
                    {
                        result = result.Where(x => x.BAGLANTIPERIYOT == filtre.BAGLANTIPERIYOT);
                    }
                    if (filtre.YANGINMODSURE != null)
                    {
                        result = result.Where(x => x.YANGINMODSURE == filtre.YANGINMODSURE);
                    }
                    if (filtre.BIRIMFIYAT != null)
                    {
                        result = result.Where(x => x.BIRIMFIYAT == filtre.BIRIMFIYAT);
                    }
                }
            }
            return result;
        }

        public List<PRMTARIFESU> Getir(PRMTARIFESUAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<PRMTARIFESU> GetirWithContext(AppContext context, PRMTARIFESUAra filtre = null)
        {
            var filterHelper = new FilterHelper<PRMTARIFESUEf>();
            return filterHelper.Sayfala(Filtrele(context.PRMTARIFESUEf.AsQueryable(), filtre), filtre?.Ara).ToList<PRMTARIFESU>();
        }


        public List<PRMTARIFESUDetay> DetayGetir(PRMTARIFESUAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<PRMTARIFESUDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.PRMTARIFESUEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public PRMTARIFESUDataTable Ara(PRMTARIFESUAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<PRMTARIFESUDetay>();

                return new PRMTARIFESUDataTable
                {
                    PRMTARIFESUDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.PRMTARIFESUEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(PRMTARIFESUAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<PRMTARIFESUEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.PRMTARIFESUEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public PRMTARIFESU Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.PRMTARIFESUEf.Find(id);
            }
        }

        public PRMTARIFESUDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.PRMTARIFESUEf.AsQueryable(), new PRMTARIFESUAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<PRMTARIFESU> Ekle(List<PRMTARIFESUEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.PRMTARIFESUEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<PRMTARIFESU> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.PRMTARIFESUEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<PRMTARIFESU>();
                }

                return null;
            }
        }


        public void Guncelle(List<PRMTARIFESUEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<PRMTARIFESUEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.PRMTARIFESUEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.PRMTARIFESUEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<PRMTARIFESUEf> mevcutDegerler, List<PRMTARIFESUEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("PRMTARIFESU bulunamadı");

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
                    var entry = context.PRMTARIFESUEf.Find(idler[0]);
                    if (entry != null)
                        context.PRMTARIFESUEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.PRMTARIFESUEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.PRMTARIFESUEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
