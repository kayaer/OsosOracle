using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTABONEBILGIComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfENTABONEBILGIDal : IENTABONEBILGIDal
    {


        private IQueryable<ENTABONEBILGIDetay> DetayDoldur(IQueryable<ENTABONEBILGIEf> result)
        {
            return result.Select(x => new ENTABONEBILGIDetay()
            {
                //	ENTABONEBILGI = x,
                // Id = x.Id,
                BLOKEACIKLAMA = x.BLOKEACIKLAMA,
                ABONENO = x.ABONENO,
                ABONEKAYITNO = x.ABONEKAYITNO,
                EPOSTA = x.EPOSTA,

                ADRES = x.ADRES,
                VERSIYON = x.VERSIYON,

                //TODO: Ek detayları buraya ekleyiniz
                //örnek: ENTABONEBILGIDurumu = x.NesneDegerDurumEf.Adi

            });
        }




        private IQueryable<ENTABONEBILGIEf> Filtrele(IQueryable<ENTABONEBILGIEf> result, ENTABONEBILGIAra filtre = null)
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

                    if (filtre.BORC != null)
                    {
                        result = result.Where(x => x.BORC == filtre.BORC);
                    }
                    if (filtre.TELEFON != null)
                    {
                        result = result.Where(x => x.TELEFON == filtre.TELEFON);
                    }
                    if (filtre.IKIM3BILGI != null)
                    {
                        result = result.Where(x => x.IKIM3BILGI == filtre.IKIM3BILGI);
                    }
                    if (filtre.BESM3BILGI != null)
                    {
                        result = result.Where(x => x.BESM3BILGI == filtre.BESM3BILGI);
                    }
                    if (filtre.KIMLIKNO != null)
                    {
                        result = result.Where(x => x.KIMLIKNO == filtre.KIMLIKNO);
                    }
                    if (filtre.BLOKE != null)
                    {
                        result = result.Where(x => x.BLOKE == filtre.BLOKE);
                    }
                    if (!string.IsNullOrEmpty(filtre.BLOKEACIKLAMA))
                    {
                        result = result.Where(x => x.BLOKEACIKLAMA.Contains(filtre.BLOKEACIKLAMA));
                    }
                    if (!string.IsNullOrEmpty(filtre.ABONENO))
                    {
                        result = result.Where(x => x.ABONENO.Contains(filtre.ABONENO));
                    }
                    if (filtre.ABONEKAYITNO != null)
                    {
                        result = result.Where(x => x.ABONEKAYITNO == filtre.ABONEKAYITNO);
                    }
                    if (!string.IsNullOrEmpty(filtre.EPOSTA))
                    {
                        result = result.Where(x => x.EPOSTA.Contains(filtre.EPOSTA));
                    }
                    if (filtre.GSM != null)
                    {
                        result = result.Where(x => x.GSM == filtre.GSM);
                    }
                    if (!string.IsNullOrEmpty(filtre.ADRES))
                    {
                        result = result.Where(x => x.ADRES.Contains(filtre.ADRES));
                    }
                    if (filtre.VERSIYON != null)
                    {
                        result = result.Where(x => x.VERSIYON == filtre.VERSIYON);
                    }
                }
            }
            return result;
        }

        public List<ENTABONEBILGI> Getir(ENTABONEBILGIAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<ENTABONEBILGI> GetirWithContext(AppContext context, ENTABONEBILGIAra filtre = null)
        {
            var filterHelper = new FilterHelper<ENTABONEBILGIEf>();
            return filterHelper.Sayfala(Filtrele(context.ENTABONEBILGIEf.AsQueryable(), filtre), filtre?.Ara).ToList<ENTABONEBILGI>();
        }


        public List<ENTABONEBILGIDetay> DetayGetir(ENTABONEBILGIAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTABONEBILGIDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTABONEBILGIEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public ENTABONEBILGIDataTable Ara(ENTABONEBILGIAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTABONEBILGIDetay>();

                return new ENTABONEBILGIDataTable
                {
                    ENTABONEBILGIDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTABONEBILGIEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(ENTABONEBILGIAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTABONEBILGIEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.ENTABONEBILGIEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public ENTABONEBILGI Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.ENTABONEBILGIEf.Find(id);
            }
        }

        public ENTABONEBILGIDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.ENTABONEBILGIEf.AsQueryable(), new ENTABONEBILGIAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<ENTABONEBILGI> Ekle(List<ENTABONEBILGIEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.ENTABONEBILGIEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<ENTABONEBILGI> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.ENTABONEBILGIEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<ENTABONEBILGI>();
                }

                return null;
            }
        }


        public void Guncelle(List<ENTABONEBILGIEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<ENTABONEBILGIEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.ENTABONEBILGIEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.ENTABONEBILGIEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<ENTABONEBILGIEf> mevcutDegerler, List<ENTABONEBILGIEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("ENTABONEBILGI bulunamadı");

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
                    var entry = context.ENTABONEBILGIEf.Find(idler[0]);
                    if (entry != null)
                        context.ENTABONEBILGIEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.ENTABONEBILGIEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.ENTABONEBILGIEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
