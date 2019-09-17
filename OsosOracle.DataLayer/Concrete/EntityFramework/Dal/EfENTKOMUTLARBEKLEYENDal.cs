using System;
using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTKOMUTLARBEKLEYENComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfENTKOMUTLARBEKLEYENDal : IENTKOMUTLARBEKLEYENDal
    {


        private IQueryable<ENTKOMUTLARBEKLEYENDetay> DetayDoldur(IQueryable<ENTKOMUTLARBEKLEYENEf> result)
        {
            return result.Select(x => new ENTKOMUTLARBEKLEYENDetay()
            {
                KOMUTID = x.KOMUTID,
                BAGLANTIDENEMESAYISI = x.BAGLANTIDENEMESAYISI,
                ACIKLAMA = x.ACIKLAMA,
                GUIDID = x.GUIDID,
                KONSSERINO = x.KONSSERINO,
                KOMUTKODU = x.KOMUTKODU,
                KOMUT = x.KOMUT,
                ISLEMTARIH = x.ISLEMTARIH,

                //TODO: Ek detayları buraya ekleyiniz
                //örnek: ENTKOMUTLARBEKLEYENDurumu = x.NesneDegerDurumEf.Adi

            });
        }




        private IQueryable<ENTKOMUTLARBEKLEYENEf> Filtrele(IQueryable<ENTKOMUTLARBEKLEYENEf> result, ENTKOMUTLARBEKLEYENAra filtre = null)
        {
            //silindi kolonu varsa silinenler gelmesin

            //TODO: filtereyi özelleştir
            if (filtre != null)
            {
                //id ve idler
                if (filtre.KAYITNO != null) { result = result.Where(x => x.GUIDID == filtre.GUIDID); }
                else
                {

                    //if (!string.IsNullOrEmpty(filtre.Idler))
                    //{
                    //	var idList = filtre.Idler.ToList<int>();
                    //	result = result.Where(x => idList.Contains(x.KAYITNO));
                    //}

                    if (filtre.KOMUTID != null)
                    {
                        result = result.Where(x => x.KOMUTID == filtre.KOMUTID);
                    }
                    if (filtre.BAGLANTIDENEMESAYISI != null)
                    {
                        result = result.Where(x => x.BAGLANTIDENEMESAYISI == filtre.BAGLANTIDENEMESAYISI);
                    }
                    if (!string.IsNullOrEmpty(filtre.ACIKLAMA))
                    {
                        result = result.Where(x => x.ACIKLAMA.Contains(filtre.ACIKLAMA));
                    }
                    //if (filtre.GUIDID != null)
                    //{
                    //    result = result.Where(x => x.GUIDID == filtre.GUIDID);
                    //}
                    if (!string.IsNullOrEmpty(filtre.KONSSERINO))
                    {
                        result = result.Where(x => x.KONSSERINO.Contains(filtre.KONSSERINO));
                    }
                    if (filtre.KOMUTKODU != null)
                    {
                        result = result.Where(x => x.KOMUTKODU == filtre.KOMUTKODU);
                    }
                    if (!string.IsNullOrEmpty(filtre.KOMUT))
                    {
                        result = result.Where(x => x.KOMUT.Contains(filtre.KOMUT));
                    }
                    if (filtre.ISLEMTARIH != null)
                    {
                        result = result.Where(x => x.ISLEMTARIH == filtre.ISLEMTARIH);
                    }
                }
            }
            return result;
        }

        public List<ENTKOMUTLARBEKLEYEN> Getir(ENTKOMUTLARBEKLEYENAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<ENTKOMUTLARBEKLEYEN> GetirWithContext(AppContext context, ENTKOMUTLARBEKLEYENAra filtre = null)
        {
            var filterHelper = new FilterHelper<ENTKOMUTLARBEKLEYENEf>();
            return filterHelper.Sayfala(Filtrele(context.ENTKOMUTLARBEKLEYENEf.AsQueryable(), filtre), filtre?.Ara).ToList<ENTKOMUTLARBEKLEYEN>();
        }


        public List<ENTKOMUTLARBEKLEYENDetay> DetayGetir(ENTKOMUTLARBEKLEYENAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTKOMUTLARBEKLEYENDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTKOMUTLARBEKLEYENEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public ENTKOMUTLARBEKLEYENDataTable Ara(ENTKOMUTLARBEKLEYENAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTKOMUTLARBEKLEYENDetay>();

                return new ENTKOMUTLARBEKLEYENDataTable
                {
                    ENTKOMUTLARBEKLEYENDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTKOMUTLARBEKLEYENEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(ENTKOMUTLARBEKLEYENAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTKOMUTLARBEKLEYENEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.ENTKOMUTLARBEKLEYENEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public ENTKOMUTLARBEKLEYEN Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.ENTKOMUTLARBEKLEYENEf.Find(id);
            }
        }

        public ENTKOMUTLARBEKLEYENDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.ENTKOMUTLARBEKLEYENEf.AsQueryable(), new ENTKOMUTLARBEKLEYENAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<ENTKOMUTLARBEKLEYEN> Ekle(List<ENTKOMUTLARBEKLEYENEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.ENTKOMUTLARBEKLEYENEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<ENTKOMUTLARBEKLEYEN> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.ENTKOMUTLARBEKLEYENEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<ENTKOMUTLARBEKLEYEN>();
                }

                return null;
            }
        }


        public void Guncelle(List<ENTKOMUTLARBEKLEYENEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<ENTKOMUTLARBEKLEYENEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.ENTKOMUTLARBEKLEYENEf.Find(yeniDegerler[0].GUIDID));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.GUIDID).ToList();
                    mevcutDegerler = context.ENTKOMUTLARBEKLEYENEf.Where(x => idler.Contains(x.GUIDID)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<ENTKOMUTLARBEKLEYENEf> mevcutDegerler, List<ENTKOMUTLARBEKLEYENEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.GUIDID == yeniDeger.GUIDID);
                if (mevcutDeger == null)
                    throw new NotificationException("ENTKOMUTLARBEKLEYEN bulunamadı");

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
                    var entry = context.ENTKOMUTLARBEKLEYENEf.Find(idler[0]);
                    if (entry != null)
                        context.ENTKOMUTLARBEKLEYENEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.ENTKOMUTLARBEKLEYENEf.Where(x => idler.Contains(x.GUIDID)).ToList();
                    context.ENTKOMUTLARBEKLEYENEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
