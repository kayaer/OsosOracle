using System;
using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTKREDIKOMUTTAKIPComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfENTKREDIKOMUTTAKIPDal : IENTKREDIKOMUTTAKIPDal
    {


        private IQueryable<ENTKREDIKOMUTTAKIPDetay> DetayDoldur(IQueryable<ENTKREDIKOMUTTAKIPEf> result)
        {
            return result.Select(x => new ENTKREDIKOMUTTAKIPDetay()
            {
                GUIDID = x.GUIDID,
                KONSSERINO = x.KONSSERINO,
                SAYACSERINO = x.SAYACSERINO,
                // ABONENO = x.ABONENO,
                SATISKAYITNO = x.SATISKAYITNO,
                KREDI = x.KREDI,
                ISLEMTARIH = x.ISLEMTARIH,
                ACIKLAMA = x.ACIKLAMA,
                DURUM = x.DURUM

            });
        }




        private IQueryable<ENTKREDIKOMUTTAKIPEf> Filtrele(IQueryable<ENTKREDIKOMUTTAKIPEf> result, ENTKREDIKOMUTTAKIPAra filtre = null)
        {
            //silindi kolonu varsa silinenler gelmesin

            //TODO: filtereyi özelleştir
            if (filtre != null)
            {
                //id ve idler
                if (filtre.GUIDID != null) { result = result.Where(x => x.GUIDID == filtre.GUIDID); }
                else
                {


                    if (!string.IsNullOrEmpty(filtre.KONSSERINO))
                    {
                        result = result.Where(x => x.KONSSERINO.Contains(filtre.KONSSERINO));
                    }
                    if (filtre.SATISKAYITNO != null)
                    {
                        result = result.Where(x => x.SATISKAYITNO == filtre.SATISKAYITNO);
                    }
                    if (filtre.ISLEMID != null)
                    {
                        result = result.Where(x => x.ISLEMID == filtre.ISLEMID);
                    }
                    if (filtre.INTERNET != null)
                    {
                        result = result.Where(x => x.INTERNET == filtre.INTERNET);
                    }
                    if (filtre.KOMUTKODU != null)
                    {
                        result = result.Where(x => x.KOMUTKODU == filtre.KOMUTKODU);
                    }
                    if (!string.IsNullOrEmpty(filtre.KOMUT))
                    {
                        result = result.Where(x => x.KOMUT.Contains(filtre.KOMUT));
                    }
                    if (filtre.KREDI != null)
                    {
                        result = result.Where(x => x.KREDI == filtre.KREDI);
                    }
                    if (filtre.ISLEMTARIH != null)
                    {
                        result = result.Where(x => x.ISLEMTARIH == filtre.ISLEMTARIH);
                    }
                    if (filtre.KOMUTID != null)
                    {
                        result = result.Where(x => x.KOMUTID == filtre.KOMUTID);
                    }
                    if (!string.IsNullOrEmpty(filtre.ACIKLAMA))
                    {
                        result = result.Where(x => x.ACIKLAMA.Contains(filtre.ACIKLAMA));
                    }
                    if (filtre.SAYACSERINO != null)
                    {
                        result = result.Where(x => x.SAYACSERINO == filtre.SAYACSERINO);
                    }
                    if (!string.IsNullOrEmpty(filtre.DURUM))
                    {
                        result = result.Where(x => x.DURUM.Contains(filtre.DURUM));
                    }
                    if (filtre.MAKBUZTARIH != null)
                    {
                        result = result.Where(x => x.MAKBUZTARIH == filtre.MAKBUZTARIH);
                    }
                    if (filtre.MAKBUZNO != null)
                    {
                        result = result.Where(x => x.MAKBUZNO == filtre.MAKBUZNO);
                    }
                    if (filtre.ABONENO != null)
                    {
                        result = result.Where(x => x.ABONENO == filtre.ABONENO);
                    }
                }
            }
            return result;
        }

        public List<ENTKREDIKOMUTTAKIP> Getir(ENTKREDIKOMUTTAKIPAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<ENTKREDIKOMUTTAKIP> GetirWithContext(AppContext context, ENTKREDIKOMUTTAKIPAra filtre = null)
        {
            var filterHelper = new FilterHelper<ENTKREDIKOMUTTAKIPEf>();
            return filterHelper.Sayfala(Filtrele(context.ENTKREDIKOMUTTAKIPEf.AsQueryable(), filtre), filtre?.Ara).ToList<ENTKREDIKOMUTTAKIP>();
        }


        public List<ENTKREDIKOMUTTAKIPDetay> DetayGetir(ENTKREDIKOMUTTAKIPAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTKREDIKOMUTTAKIPDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTKREDIKOMUTTAKIPEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public ENTKREDIKOMUTTAKIPDataTable Ara(ENTKREDIKOMUTTAKIPAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTKREDIKOMUTTAKIPDetay>();

                return new ENTKREDIKOMUTTAKIPDataTable
                {
                    ENTKREDIKOMUTTAKIPDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTKREDIKOMUTTAKIPEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(ENTKREDIKOMUTTAKIPAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTKREDIKOMUTTAKIPEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.ENTKREDIKOMUTTAKIPEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public ENTKREDIKOMUTTAKIP Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.ENTKREDIKOMUTTAKIPEf.Find(id);
            }
        }

        public ENTKREDIKOMUTTAKIPDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.ENTKREDIKOMUTTAKIPEf.AsQueryable(), new ENTKREDIKOMUTTAKIPAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<ENTKREDIKOMUTTAKIP> Ekle(List<ENTKREDIKOMUTTAKIPEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.ENTKREDIKOMUTTAKIPEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<ENTKREDIKOMUTTAKIP> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.ENTKREDIKOMUTTAKIPEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<ENTKREDIKOMUTTAKIP>();
                }

                return null;
            }
        }


        public void Guncelle(List<ENTKREDIKOMUTTAKIPEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<ENTKREDIKOMUTTAKIPEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.ENTKREDIKOMUTTAKIPEf.Find(yeniDegerler[0].GUIDID));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.GUIDID).ToList();
                    mevcutDegerler = context.ENTKREDIKOMUTTAKIPEf.Where(x => idler.Contains(x.GUIDID)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<ENTKREDIKOMUTTAKIPEf> mevcutDegerler, List<ENTKREDIKOMUTTAKIPEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.GUIDID == yeniDeger.GUIDID);
                if (mevcutDeger == null)
                    throw new NotificationException("ENTKREDIKOMUTTAKIP bulunamadı");

                var entry = context.Entry(mevcutDeger);
                entry.CurrentValues.SetValues(yeniDeger);

                ////Değişmemesi gereken kolonlar buraya yazılacak.
                //entry.Property(u => u.Id).IsModified = false;
                //entry.Property(u => u.EklemeTarihi).IsModified = false;
                //entry.Property(u => u.EkleyenId).IsModified = false;
            }
        }

        public void Sil(List<Guid> idler)
        {
            using (var context = new AppContext())
            {
                if (idler.Count == 1)
                {
                    var entry = context.ENTKREDIKOMUTTAKIPEf.Find(idler[0]);
                    if (entry != null)
                        context.ENTKREDIKOMUTTAKIPEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.ENTKREDIKOMUTTAKIPEf.Where(x => idler.Contains(x.GUIDID)).ToList();
                    context.ENTKREDIKOMUTTAKIPEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
