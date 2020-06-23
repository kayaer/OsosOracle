using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTTUKETIMSUComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfENTTUKETIMSUDal : IENTTUKETIMSUDal
    {


        private IQueryable<ENTTUKETIMSUDetay> DetayDoldur(IQueryable<ENTTUKETIMSUEf> result)
        {
            return result.Select(x => new ENTTUKETIMSUDetay()
            {
                //	ENTTUKETIMSU = x,
                KAYITNO = x.KAYITNO,
                SAYACID = x.SAYACID,
                TUKETIM = x.TUKETIM,
                HARCANANKREDI=x.HARCANANKREDI,
                KALANKREDI=x.KALANKREDI,
                OKUMATARIH=x.OKUMATARIH,
                SAYACTARIH=x.SAYACTARIH,
                //KrediDurumu=x.EntSayacDurumSuEf.KREDIBITTI
            });
        }




        private IQueryable<ENTTUKETIMSUEf> Filtrele(IQueryable<ENTTUKETIMSUEf> result, ENTTUKETIMSUAra filtre = null)
        {
            //silindi kolonu varsa silinenler gelmesin

            //TODO: filtereyi özelleştir
            if (filtre != null)
            {
                //id ve idler
                if (filtre.KAYITNO != null) { result = result.Where(x => x.KAYITNO == filtre.KAYITNO); }
                else
                {
                    if (!string.IsNullOrEmpty(filtre.SAYACID))
                    {
                        result = result.Where(x => x.SAYACID.Contains(filtre.SAYACID));
                    }
                    if (filtre.TUKETIM != null)
                    {
                        result = result.Where(x => x.TUKETIM == filtre.TUKETIM);
                    }
                    if (filtre.TUKETIM1 != null)
                    {
                        result = result.Where(x => x.TUKETIM1 == filtre.TUKETIM1);
                    }
                    if (filtre.TUKETIM2 != null)
                    {
                        result = result.Where(x => x.TUKETIM2 == filtre.TUKETIM2);
                    }
                    if (filtre.TUKETIM3 != null)
                    {
                        result = result.Where(x => x.TUKETIM3 == filtre.TUKETIM3);
                    }
                    if (filtre.TUKETIM4 != null)
                    {
                        result = result.Where(x => x.TUKETIM4 == filtre.TUKETIM4);
                    }
                    if (filtre.SAYACTARIH != null)
                    {
                        result = result.Where(x => x.SAYACTARIH == filtre.SAYACTARIH);
                    }
                    if (filtre.OKUMATARIH != null)
                    {
                        result = result.Where(x => x.OKUMATARIH == filtre.OKUMATARIH);
                    }
                    if (filtre.DATA != null)
                    {
                        result = result.Where(x => x.DATA == filtre.DATA);
                    }
                    if (filtre.HGUN != null)
                    {
                        result = result.Where(x => x.HGUN == filtre.HGUN);
                    }
                    if (filtre.HARCANANKREDI != null)
                    {
                        result = result.Where(x => x.HARCANANKREDI == filtre.HARCANANKREDI);
                    }
                    if (filtre.KALANKREDI != null)
                    {
                        result = result.Where(x => x.KALANKREDI == filtre.KALANKREDI);
                    }
                    if (filtre.VERSIYON != null)
                    {
                        result = result.Where(x => x.VERSIYON == filtre.VERSIYON);
                    }
                    if (filtre.HEADERNO != null)
                    {
                        result = result.Where(x => x.HEADERNO == filtre.HEADERNO);
                    }
                    if (filtre.FATURAMOD != null)
                    {
                        result = result.Where(x => x.FATURAMOD == filtre.FATURAMOD);
                    }
                    if (filtre.OkumaTarihiBaslangic != null)
                    {
                        result = result.Where(x => x.OKUMATARIH > filtre.OkumaTarihiBaslangic);
                    }
                    if (filtre.OkumaTarihiBitis != null)
                    {
                        result = result.Where(x => x.OKUMATARIH < filtre.OkumaTarihiBitis);
                    }
                }
            }
            return result;
        }

        public List<ENTTUKETIMSU> Getir(ENTTUKETIMSUAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<ENTTUKETIMSU> GetirWithContext(AppContext context, ENTTUKETIMSUAra filtre = null)
        {
            var filterHelper = new FilterHelper<ENTTUKETIMSUEf>();
            return filterHelper.Sayfala(Filtrele(context.EntTuketimSuEf.AsQueryable(), filtre), filtre?.Ara).ToList<ENTTUKETIMSU>();
        }


        public List<ENTTUKETIMSUDetay> DetayGetir(ENTTUKETIMSUAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTTUKETIMSUDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.EntTuketimSuEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public ENTTUKETIMSUDataTable Ara(ENTTUKETIMSUAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTTUKETIMSUDetay>();

                return new ENTTUKETIMSUDataTable
                {
                    ENTTUKETIMSUDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.EntTuketimSuEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(ENTTUKETIMSUAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTTUKETIMSUEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.EntTuketimSuEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public ENTTUKETIMSU Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.EntTuketimSuEf.Find(id);
            }
        }

        public ENTTUKETIMSUDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.EntTuketimSuEf.AsQueryable(), new ENTTUKETIMSUAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<ENTTUKETIMSU> Ekle(List<ENTTUKETIMSUEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.EntTuketimSuEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<ENTTUKETIMSU> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.EntTuketimSuEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<ENTTUKETIMSU>();
                }

                return null;
            }
        }


        public void Guncelle(List<ENTTUKETIMSUEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<ENTTUKETIMSUEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.EntTuketimSuEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.EntTuketimSuEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<ENTTUKETIMSUEf> mevcutDegerler, List<ENTTUKETIMSUEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("ENTTUKETIMSU bulunamadı");

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
                    var entry = context.EntTuketimSuEf.Find(idler[0]);
                    if (entry != null)
                        context.EntTuketimSuEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.EntTuketimSuEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.EntTuketimSuEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
