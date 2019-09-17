using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTKOMUTLARSONUCLANANComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfENTKOMUTLARSONUCLANANDal : IENTKOMUTLARSONUCLANANDal
    {


        private IQueryable<ENTKOMUTLARSONUCLANANDetay> DetayDoldur(IQueryable<ENTKOMUTLARSONUCLANANEf> result)
        {
            return result.Select(x => new ENTKOMUTLARSONUCLANANDetay()
            {
                //	ENTKOMUTLARSONUCLANAN = x,
                KAYITNO = x.KAYITNO,

                KONSSERINO = x.KONSSERINO,
                KOMUTKODU = x.KOMUTKODU,
                KOMUT = x.KOMUT,
                ISLEMTARIH = x.ISLEMTARIH,
                SONUC = x.SONUC,
                ISLEMSURESI = x.ISLEMSURESI,
                KOMUTID = x.KOMUTID,
                ACIKLAMA = x.ACIKLAMA,
                CEVAP = x.CEVAP,

                //TODO: Ek detayları buraya ekleyiniz
                //örnek: ENTKOMUTLARSONUCLANANDurumu = x.NesneDegerDurumEf.Adi

            });
        }




        private IQueryable<ENTKOMUTLARSONUCLANANEf> Filtrele(IQueryable<ENTKOMUTLARSONUCLANANEf> result, ENTKOMUTLARSONUCLANANAra filtre = null)
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
                    if (!string.IsNullOrEmpty(filtre.SONUC))
                    {
                        result = result.Where(x => x.SONUC.Contains(filtre.SONUC));
                    }
                    if (filtre.ISLEMSURESI != null)
                    {
                        result = result.Where(x => x.ISLEMSURESI == filtre.ISLEMSURESI);
                    }
                    if (filtre.KOMUTID != null)
                    {
                        result = result.Where(x => x.KOMUTID == filtre.KOMUTID);
                    }
                    if (!string.IsNullOrEmpty(filtre.ACIKLAMA))
                    {
                        result = result.Where(x => x.ACIKLAMA.Contains(filtre.ACIKLAMA));
                    }
                    if (!string.IsNullOrEmpty(filtre.CEVAP))
                    {
                        result = result.Where(x => x.CEVAP.Contains(filtre.CEVAP));
                    }
                }
            }
            return result;
        }

        public List<ENTKOMUTLARSONUCLANAN> Getir(ENTKOMUTLARSONUCLANANAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<ENTKOMUTLARSONUCLANAN> GetirWithContext(AppContext context, ENTKOMUTLARSONUCLANANAra filtre = null)
        {
            var filterHelper = new FilterHelper<ENTKOMUTLARSONUCLANANEf>();
            return filterHelper.Sayfala(Filtrele(context.ENTKOMUTLARSONUCLANANEf.AsQueryable(), filtre), filtre?.Ara).ToList<ENTKOMUTLARSONUCLANAN>();
        }


        public List<ENTKOMUTLARSONUCLANANDetay> DetayGetir(ENTKOMUTLARSONUCLANANAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTKOMUTLARSONUCLANANDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTKOMUTLARSONUCLANANEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public ENTKOMUTLARSONUCLANANDataTable Ara(ENTKOMUTLARSONUCLANANAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTKOMUTLARSONUCLANANDetay>();

                return new ENTKOMUTLARSONUCLANANDataTable
                {
                    ENTKOMUTLARSONUCLANANDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTKOMUTLARSONUCLANANEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(ENTKOMUTLARSONUCLANANAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTKOMUTLARSONUCLANANEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.ENTKOMUTLARSONUCLANANEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public ENTKOMUTLARSONUCLANAN Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.ENTKOMUTLARSONUCLANANEf.Find(id);
            }
        }

        public ENTKOMUTLARSONUCLANANDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.ENTKOMUTLARSONUCLANANEf.AsQueryable(), new ENTKOMUTLARSONUCLANANAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<ENTKOMUTLARSONUCLANAN> Ekle(List<ENTKOMUTLARSONUCLANANEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.ENTKOMUTLARSONUCLANANEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<ENTKOMUTLARSONUCLANAN> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.ENTKOMUTLARSONUCLANANEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<ENTKOMUTLARSONUCLANAN>();
                }

                return null;
            }
        }


        public void Guncelle(List<ENTKOMUTLARSONUCLANANEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<ENTKOMUTLARSONUCLANANEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.ENTKOMUTLARSONUCLANANEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.ENTKOMUTLARSONUCLANANEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<ENTKOMUTLARSONUCLANANEf> mevcutDegerler, List<ENTKOMUTLARSONUCLANANEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("ENTKOMUTLARSONUCLANAN bulunamadı");

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
                    var entry = context.ENTKOMUTLARSONUCLANANEf.Find(idler[0]);
                    if (entry != null)
                        context.ENTKOMUTLARSONUCLANANEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.ENTKOMUTLARSONUCLANANEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.ENTKOMUTLARSONUCLANANEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
