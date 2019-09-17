using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.PRMTARIFEORTAKAVMComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfPRMTARIFEORTAKAVMDal : IPRMTARIFEORTAKAVMDal
    {


        private IQueryable<PRMTARIFEORTAKAVMDetay> DetayDoldur(IQueryable<PRMTARIFEORTAKAVMEf> result)
        {
            return result.Select(x => new PRMTARIFEORTAKAVMDetay()
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
                CARPAN = x.CARPAN,
                SAYACCAP = x.SAYACCAP,
                SAYACTARIH = x.SAYACTARIH,
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
                Kurum=x.ConKurumEf.AD

            });
        }




        private IQueryable<PRMTARIFEORTAKAVMEf> Filtrele(IQueryable<PRMTARIFEORTAKAVMEf> result, PRMTARIFEORTAKAVMAra filtre = null)
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
                    if (filtre.CARPAN != null)
                    {
                        result = result.Where(x => x.CARPAN == filtre.CARPAN);
                    }
                    if (filtre.SAYACCAP != null)
                    {
                        result = result.Where(x => x.SAYACCAP == filtre.SAYACCAP);
                    }
                    if (!string.IsNullOrEmpty(filtre.SAYACTARIH))
                    {
                        result = result.Where(x => x.SAYACTARIH.Contains(filtre.SAYACTARIH));
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

        public List<PRMTARIFEORTAKAVM> Getir(PRMTARIFEORTAKAVMAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<PRMTARIFEORTAKAVM> GetirWithContext(AppContext context, PRMTARIFEORTAKAVMAra filtre = null)
        {
            var filterHelper = new FilterHelper<PRMTARIFEORTAKAVMEf>();
            return filterHelper.Sayfala(Filtrele(context.PRMTARIFEORTAKAVMEf.AsQueryable(), filtre), filtre?.Ara).ToList<PRMTARIFEORTAKAVM>();
        }


        public List<PRMTARIFEORTAKAVMDetay> DetayGetir(PRMTARIFEORTAKAVMAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<PRMTARIFEORTAKAVMDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.PRMTARIFEORTAKAVMEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public PRMTARIFEORTAKAVMDataTable Ara(PRMTARIFEORTAKAVMAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<PRMTARIFEORTAKAVMDetay>();

                return new PRMTARIFEORTAKAVMDataTable
                {
                    PRMTARIFEORTAKAVMDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.PRMTARIFEORTAKAVMEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(PRMTARIFEORTAKAVMAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<PRMTARIFEORTAKAVMEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.PRMTARIFEORTAKAVMEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public PRMTARIFEORTAKAVM Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.PRMTARIFEORTAKAVMEf.Find(id);
            }
        }

        public PRMTARIFEORTAKAVMDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.PRMTARIFEORTAKAVMEf.AsQueryable(), new PRMTARIFEORTAKAVMAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<PRMTARIFEORTAKAVM> Ekle(List<PRMTARIFEORTAKAVMEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.PRMTARIFEORTAKAVMEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<PRMTARIFEORTAKAVM> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.PRMTARIFEORTAKAVMEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<PRMTARIFEORTAKAVM>();
                }

                return null;
            }
        }


        public void Guncelle(List<PRMTARIFEORTAKAVMEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<PRMTARIFEORTAKAVMEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.PRMTARIFEORTAKAVMEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.PRMTARIFEORTAKAVMEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<PRMTARIFEORTAKAVMEf> mevcutDegerler, List<PRMTARIFEORTAKAVMEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("PRMTARIFEORTAKAVM bulunamadı");

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
                    var entry = context.PRMTARIFEORTAKAVMEf.Find(idler[0]);
                    if (entry != null)
                        context.PRMTARIFEORTAKAVMEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.PRMTARIFEORTAKAVMEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.PRMTARIFEORTAKAVMEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
