using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSMENUComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfSYSMENUDal : ISYSMENUDal
    {


        private IQueryable<SYSMENUDetay> DetayDoldur(IQueryable<SYSMENUEf> result)
        {
            return result.Select(x => new SYSMENUDetay()
            {
               
                KAYITNO = x.KAYITNO,
                TR = x.TR,
                EN = x.EN,
                YEREL = x.YEREL,
                PARENTKAYITNO = x.PARENTKAYITNO,
                MENUORDER = x.MENUORDER,
                EXTERNALURL = x.EXTERNALURL,
                AREA = x.AREA,
                ACTION = x.ACTION,
                CONTROLLER = x.CONTROLLER,
                PARAMETERS = x.PARAMETERS,
                DURUM = x.DURUM,
                VERSIYON = x.VERSIYON,
                ICON = x.ICON

            });
        }




        private IQueryable<SYSMENUEf> Filtrele(IQueryable<SYSMENUEf> result, SYSMENUAra filtre = null)
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


                    if (!string.IsNullOrEmpty(filtre.TR))
                    {
                        result = result.Where(x => x.TR.Contains(filtre.TR));
                    }
                    if (!string.IsNullOrEmpty(filtre.EN))
                    {
                        result = result.Where(x => x.EN.Contains(filtre.EN));
                    }
                    if (!string.IsNullOrEmpty(filtre.YEREL))
                    {
                        result = result.Where(x => x.YEREL.Contains(filtre.YEREL));
                    }
                    if (filtre.PARENTKAYITNO != null)
                    {
                        result = result.Where(x => x.PARENTKAYITNO == filtre.PARENTKAYITNO);
                    }
                    if (filtre.MENUORDER != null)
                    {
                        result = result.Where(x => x.MENUORDER == filtre.MENUORDER);
                    }
                    if (!string.IsNullOrEmpty(filtre.EXTERNALURL))
                    {
                        result = result.Where(x => x.EXTERNALURL.Contains(filtre.EXTERNALURL));
                    }
                    if (!string.IsNullOrEmpty(filtre.AREA))
                    {
                        result = result.Where(x => x.AREA.Contains(filtre.AREA));
                    }
                    //if (!string.IsNullOrEmpty(filtre.ACTION))
                    //{
                    //    result = result.Where(x => x.ACTION.Contains(filtre.ACTION));
                    //}
                    //if (!string.IsNullOrEmpty(filtre.CONTROLLER))
                    //{
                    //    result = result.Where(x => x.CONTROLLER.Contains(filtre.CONTROLLER));
                    //}
                    if (!string.IsNullOrEmpty(filtre.PARAMETERS))
                    {
                        result = result.Where(x => x.PARAMETERS.Contains(filtre.PARAMETERS));
                    }
                    if (filtre.DURUM != null)
                    {
                        result = result.Where(x => x.DURUM == filtre.DURUM);
                    }
                    if (filtre.VERSIYON != null)
                    {
                        result = result.Where(x => x.VERSIYON == filtre.VERSIYON);
                    }
                    if (!string.IsNullOrEmpty(filtre.ICON))
                    {
                        result = result.Where(x => x.ICON.Contains(filtre.ICON));
                    }
                }
            }
            return result;
        }

        public List<SYSMENU> Getir(SYSMENUAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<SYSMENU> GetirWithContext(AppContext context, SYSMENUAra filtre = null)
        {
            var filterHelper = new FilterHelper<SYSMENUEf>();
            return filterHelper.Sayfala(Filtrele(context.SYSMENUEf.AsQueryable(), filtre), filtre?.Ara).ToList<SYSMENU>();
        }


        public List<SYSMENUDetay> DetayGetir(SYSMENUAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSMENUDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.SYSMENUEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public SYSMENUDataTable Ara(SYSMENUAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSMENUDetay>();

                return new SYSMENUDataTable
                {
                    SYSMENUDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.SYSMENUEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(SYSMENUAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<SYSMENUEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.SYSMENUEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public SYSMENU Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.SYSMENUEf.Find(id);
            }
        }

        public SYSMENUDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.SYSMENUEf.AsQueryable(), new SYSMENUAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<SYSMENU> Ekle(List<SYSMENUEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.SYSMENUEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<SYSMENU> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.SYSMENUEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<SYSMENU>();
                }

                return null;
            }
        }


        public void Guncelle(List<SYSMENUEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<SYSMENUEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");

                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.SYSMENUEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.SYSMENUEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<SYSMENUEf> mevcutDegerler, List<SYSMENUEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("SYSMENU bulunamadı");


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
                    var entry = context.SYSMENUEf.Find(idler[0]);
                    if (entry != null)
                        context.SYSMENUEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.SYSMENUEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.SYSMENUEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }
        public List<SYSMENU> ParentMenuGetir()
        {
            using (var context = new AppContext())
            {
                return context.SYSMENUEf.Where(x => x.PARENTKAYITNO == null).ToList<SYSMENU>();
            }
        }
    }
}
