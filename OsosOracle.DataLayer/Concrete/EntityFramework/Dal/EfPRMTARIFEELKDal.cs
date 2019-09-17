using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.PRMTARIFEELKComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfPRMTARIFEELKDal : IPRMTARIFEELKDal
    {


        private IQueryable<PRMTARIFEELKDetay> DetayDoldur(IQueryable<PRMTARIFEELKEf> result)
        {
            return result.Select(x => new PRMTARIFEELKDetay()
            {
                KAYITNO = x.KAYITNO,
                KREDIKATSAYI = x.KREDIKATSAYI,
                AD = x.AD,
                YEDEKKREDI = x.YEDEKKREDI,
                KRITIKKREDI = x.KRITIKKREDI,
                CARPAN = x.CARPAN,
                ACIKLAMA = x.ACIKLAMA,
                DURUM = x.DURUM,
                FIYAT1 = x.FIYAT1,
                FIYAT2 = x.FIYAT2,
                FIYAT3 = x.FIYAT3,
                LIMIT1 = x.LIMIT1,
                LIMIT2 = x.LIMIT2,
                YUKLEMELIMIT = x.YUKLEMELIMIT,
                SABAHSAAT = x.SABAHSAAT,
                AKSAMSAAT = x.AKSAMSAAT,
                HAFTASONUAKSAM = x.HAFTASONUAKSAM,
                SABITUCRET = x.SABITUCRET,
                BAYRAM1GUN = x.BAYRAM1GUN,
                BAYRAM1AY = x.BAYRAM1AY,
                BAYRAM1SURE = x.BAYRAM1SURE,
                BAYRAM2GUN = x.BAYRAM2GUN,
                BAYRAM2AY = x.BAYRAM2AY,
                BAYRAM2SURE = x.BAYRAM2SURE,
                KURUMKAYITNO = x.KURUMKAYITNO,
                Kurum=x.ConKurumEf.AD

            });
        }




        private IQueryable<PRMTARIFEELKEf> Filtrele(IQueryable<PRMTARIFEELKEf> result, PRMTARIFEELKAra filtre = null)
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

                    if (filtre.KREDIKATSAYI != null)
                    {
                        result = result.Where(x => x.KREDIKATSAYI == filtre.KREDIKATSAYI);
                    }
                    if (!string.IsNullOrEmpty(filtre.AD))
                    {
                        result = result.Where(x => x.AD.Contains(filtre.AD));
                    }
                    if (filtre.YEDEKKREDI != null)
                    {
                        result = result.Where(x => x.YEDEKKREDI == filtre.YEDEKKREDI);
                    }
                    if (filtre.KRITIKKREDI != null)
                    {
                        result = result.Where(x => x.KRITIKKREDI == filtre.KRITIKKREDI);
                    }
                    if (filtre.CARPAN != null)
                    {
                        result = result.Where(x => x.CARPAN == filtre.CARPAN);
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
                    if (filtre.LIMIT1 != null)
                    {
                        result = result.Where(x => x.LIMIT1 == filtre.LIMIT1);
                    }
                    if (filtre.LIMIT2 != null)
                    {
                        result = result.Where(x => x.LIMIT2 == filtre.LIMIT2);
                    }
                    if (filtre.YUKLEMELIMIT != null)
                    {
                        result = result.Where(x => x.YUKLEMELIMIT == filtre.YUKLEMELIMIT);
                    }
                    if (!string.IsNullOrEmpty(filtre.SABAHSAAT))
                    {
                        result = result.Where(x => x.SABAHSAAT.Contains(filtre.SABAHSAAT));
                    }
                    if (!string.IsNullOrEmpty(filtre.AKSAMSAAT))
                    {
                        result = result.Where(x => x.AKSAMSAAT.Contains(filtre.AKSAMSAAT));
                    }
                    if (!string.IsNullOrEmpty(filtre.HAFTASONUAKSAM))
                    {
                        result = result.Where(x => x.HAFTASONUAKSAM.Contains(filtre.HAFTASONUAKSAM));
                    }
                    if (filtre.SABITUCRET != null)
                    {
                        result = result.Where(x => x.SABITUCRET == filtre.SABITUCRET);
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

        public List<PRMTARIFEELK> Getir(PRMTARIFEELKAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<PRMTARIFEELK> GetirWithContext(AppContext context, PRMTARIFEELKAra filtre = null)
        {
            var filterHelper = new FilterHelper<PRMTARIFEELKEf>();
            return filterHelper.Sayfala(Filtrele(context.PRMTARIFEELKEf.AsQueryable(), filtre), filtre?.Ara).ToList<PRMTARIFEELK>();
        }


        public List<PRMTARIFEELKDetay> DetayGetir(PRMTARIFEELKAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<PRMTARIFEELKDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.PRMTARIFEELKEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public PRMTARIFEELKDataTable Ara(PRMTARIFEELKAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<PRMTARIFEELKDetay>();

                return new PRMTARIFEELKDataTable
                {
                    PRMTARIFEELKDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.PRMTARIFEELKEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(PRMTARIFEELKAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<PRMTARIFEELKEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.PRMTARIFEELKEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public PRMTARIFEELK Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.PRMTARIFEELKEf.Find(id);
            }
        }

        public PRMTARIFEELKDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.PRMTARIFEELKEf.AsQueryable(), new PRMTARIFEELKAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<PRMTARIFEELK> Ekle(List<PRMTARIFEELKEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.PRMTARIFEELKEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<PRMTARIFEELK> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.PRMTARIFEELKEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<PRMTARIFEELK>();
                }

                return null;
            }
        }


        public void Guncelle(List<PRMTARIFEELKEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<PRMTARIFEELKEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.PRMTARIFEELKEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.PRMTARIFEELKEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<PRMTARIFEELKEf> mevcutDegerler, List<PRMTARIFEELKEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("PRMTARIFEELK bulunamadı");

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
                    var entry = context.PRMTARIFEELKEf.Find(idler[0]);
                    if (entry != null)
                        context.PRMTARIFEELKEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.PRMTARIFEELKEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.PRMTARIFEELKEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
