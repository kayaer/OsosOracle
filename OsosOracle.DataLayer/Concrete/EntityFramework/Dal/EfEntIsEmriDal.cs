using System;
using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.EntIsEmriComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Entities.Enums;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfEntIsEmriDal : IEntIsEmriDal
    {


        private IQueryable<EntIsEmriDetay> DetayDoldur(IQueryable<EntIsEmriEf> result)
        {
            return result.Select(x => new EntIsEmriDetay()
            {
                KayitNo = x.KAYITNO,
                SayacKayitNo = x.SayacKayitNo,
                IsEmriKayitNo = x.IsEmriKayitNo,
                IsEmriKod=x.NesneDegerIsEmriEf.DEGER,
                IsEmriDurumKayitNo = x.IsEmriDurumKayitNo,
                IsEmriCevap = x.IsEmriCevap,
                Parametre = x.Parametre,
                Cevap = x.Cevap,
                IsEmriAdi=x.NesneDegerIsEmriEf.AD,
                IsEmriDurum=x.NesneDegerIsEmriDurumEf.AD,
                SayacSeriNo=x.EntSayacEf.SERINO,
                OlusturmaTarih=x.OlusturmaTarih,
                GuncellemeTarih=x.GuncellemeTarih
            });
        }




        private IQueryable<EntIsEmriEf> Filtrele(IQueryable<EntIsEmriEf> result, EntIsEmriAra filtre = null)
        {
            //silindi kolonu varsa silinenler gelmesin

            //TODO: filtereyi özelleştir
            if (filtre != null)
            {
                //id ve idler
                if (filtre.KayitNo != null) { result = result.Where(x => x.KAYITNO == filtre.KayitNo); }
                else
                {
                    if (filtre.SayacKayitNo != null)
                    {
                        result = result.Where(x => x.SayacKayitNo == filtre.SayacKayitNo);
                    }
                    if (filtre.IsEmriKayitNo != null)
                    {
                        result = result.Where(x => x.IsEmriKayitNo == filtre.IsEmriKayitNo);
                    }
                    if (filtre.IsEmriDurumKayitNo != null)
                    {
                        result = result.Where(x => x.IsEmriDurumKayitNo == filtre.IsEmriDurumKayitNo);
                    }
                    if (filtre.IsEmriCevap != null)
                    {
                        result = result.Where(x => x.IsEmriCevap == filtre.IsEmriCevap);
                    }
                    if (!string.IsNullOrEmpty(filtre.KonsSeriNo))
                    {
                        result = result.Where(x => x.EntSayacEf.KonsSeriNo == filtre.KonsSeriNo);
                    }

                }
            }
            return result;
        }

        public List<EntIsEmri> Getir(EntIsEmriAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<EntIsEmri> GetirWithContext(AppContext context, EntIsEmriAra filtre = null)
        {
            var filterHelper = new FilterHelper<EntIsEmriEf>();
            return filterHelper.Sayfala(Filtrele(context.EntIsEmriEf.AsQueryable(), filtre), filtre?.Ara).ToList<EntIsEmri>();
        }


        public List<EntIsEmriDetay> DetayGetir(EntIsEmriAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<EntIsEmriDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.EntIsEmriEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public EntIsEmriDataTable Ara(EntIsEmriAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<EntIsEmriDetay>();

                return new EntIsEmriDataTable
                {
                    EntIsEmriDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.EntIsEmriEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(EntIsEmriAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<EntIsEmriEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.EntIsEmriEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public EntIsEmri Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.EntIsEmriEf.Find(id);
            }
        }

        public EntIsEmriDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.EntIsEmriEf.AsQueryable(), new EntIsEmriAra() { KayitNo = id }))
                  .FirstOrDefault();
            }
        }

        public List<EntIsEmri> Ekle(List<EntIsEmriEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.EntIsEmriEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<EntIsEmri> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.EntIsEmriEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<EntIsEmri>();
                }

                return null;
            }
        }


        public void Guncelle(List<EntIsEmriEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<EntIsEmriEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.EntIsEmriEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.EntIsEmriEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<EntIsEmriEf> mevcutDegerler, List<EntIsEmriEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("EntIsEmri bulunamadı");

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
                    var entry = context.EntIsEmriEf.Find(idler[0]);
                    if (entry != null)
                        context.EntIsEmriEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.EntIsEmriEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.EntIsEmriEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
