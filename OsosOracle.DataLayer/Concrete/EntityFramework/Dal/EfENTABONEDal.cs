using System.Collections.Generic;
using System.Data;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTABONEComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfENTABONEDal : IENTABONEDal
    {


        private IQueryable<ENTABONEDetay> DetayDoldur(IQueryable<ENTABONEEf> result)
        {
            return result.Select(x => new ENTABONEDetay()
            {
                KAYITNO = x.KAYITNO,
                AD = x.AD,
                SOYAD = x.SOYAD,
                DURUM = x.DURUM,
                VERSIYON = x.VERSIYON,
                KURUMKAYITNO = x.KURUMKAYITNO,
                AboneNo = x.ABONENO,
                SonSatisTarih=x.SonSatisTarih,
                SayacModel = x.AboneSayacEfCollection.FirstOrDefault().EntSayacEf.CstSayacModelEf.AD,
                SayacSeriNo = x.AboneSayacEfCollection.FirstOrDefault().EntSayacEf.SERINO,
                KimlikNo=x.KimlikNo,
                Daire=x.Daire,
                Blok=x.Blok
               
            });
        }




        private IQueryable<ENTABONEEf> Filtrele(IQueryable<ENTABONEEf> result, ENTABONEAra filtre = null)
        {
            //silindi kolonu varsa silinenler gelmesin
            result = result.Where(x => x.DURUM == 1);
            //TODO: filtereyi özelleştir
            if (filtre != null)
            {
                //id ve idler
                if (filtre.KAYITNO != null) { result = result.Where(x => x.KAYITNO == filtre.KAYITNO); }
                else
                {
                    if (!string.IsNullOrEmpty(filtre.AD))
                    {
                        result = result.Where(x => x.AD.Contains(filtre.AD));
                    }
                    if (!string.IsNullOrEmpty(filtre.SOYAD))
                    {
                        result = result.Where(x => x.SOYAD.Contains(filtre.SOYAD));
                    }
                    if (filtre.DURUM != null)
                    {
                        result = result.Where(x => x.DURUM == filtre.DURUM);
                    }
                    if (filtre.VERSIYON != null)
                    {
                        result = result.Where(x => x.VERSIYON == filtre.VERSIYON);
                    }
                    if (filtre.KURUMKAYITNO != null)
                    {
                        result = result.Where(x => x.KURUMKAYITNO == filtre.KURUMKAYITNO);
                    }
                    if (!string.IsNullOrEmpty(filtre.AboneNo))
                    {
                        result = result.Where(x => x.ABONENO == filtre.AboneNo);
                    }
                    if (!string.IsNullOrEmpty(filtre.AboneNoVeyAdiVeyaSoyadi))
                    {
                        result = result.Where(x => (x.AD + " " + x.SOYAD).ToLower().Trim().Contains(filtre.AboneNoVeyAdiVeyaSoyadi.ToLower().Trim()) || x.ABONENO.Contains(filtre.AboneNoVeyAdiVeyaSoyadi.ToLower().Trim()));
                    }
                    if (filtre.SayacKayitNo != null)
                    {
                        result = result.Where(x => x.AboneSayacEfCollection.Any(y => y.EntSayacEf.KAYITNO == filtre.SayacKayitNo));
                    }
                    if (filtre.SonSatisTarihiBaslangic != null)
                    {
                        result = result.Where(x => x.SonSatisTarih >= filtre.SonSatisTarihiBaslangic);
                    }
                    if (filtre.SonSatisTarihBitis != null)
                    {
                        result = result.Where(x => x.SonSatisTarih <= filtre.SonSatisTarihBitis);
                    }
                    if (!string.IsNullOrEmpty(filtre.KimlikNo))
                    {
                        result = result.Where(x => x.KimlikNo == filtre.KimlikNo);
                    }

                }
            }
            return result;
        }

        public List<ENTABONE> Getir(ENTABONEAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<ENTABONE> GetirWithContext(AppContext context, ENTABONEAra filtre = null)
        {
            var filterHelper = new FilterHelper<ENTABONEEf>();
            return filterHelper.Sayfala(Filtrele(context.ENTABONEEf.AsQueryable(), filtre), filtre?.Ara).ToList<ENTABONE>();
        }


        public List<ENTABONEDetay> DetayGetir(ENTABONEAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTABONEDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTABONEEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public ENTABONEDataTable Ara(ENTABONEAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTABONEDetay>();

                return new ENTABONEDataTable
                {
                    ENTABONEDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTABONEEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(ENTABONEAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTABONEEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.ENTABONEEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public ENTABONE Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.ENTABONEEf.Find(id);
            }
        }

        public ENTABONEDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.ENTABONEEf.AsQueryable(), new ENTABONEAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<ENTABONE> Ekle(List<ENTABONEEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.ENTABONEEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<ENTABONE> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.ENTABONEEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<ENTABONE>();
                }

                return null;
            }
        }


        public void Guncelle(List<ENTABONEEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<ENTABONEEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.ENTABONEEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.ENTABONEEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<ENTABONEEf> mevcutDegerler, List<ENTABONEEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("ENTABONE bulunamadı");

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
                    var entry = context.ENTABONEEf.Find(idler[0]);
                    if (entry != null)
                        context.ENTABONEEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.ENTABONEEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.ENTABONEEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

        public List<AboneAutoComplete> AutoCompleteBilgileriGetir(ENTABONEAra filtre = null)
        {
            using (var context = new AppContext())
            {
                using (var tran = context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        var filterHelper = new FilterHelper<AboneAutoComplete>();
                        var result = Filtrele(context.ENTABONEEf.AsQueryable(), filtre);


                        return filterHelper.Sayfala(
                                                      result.Select(x => new AboneAutoComplete
                                                      {
                                                          KayitNo = x.KAYITNO,
                                                          Adi = x.AD,
                                                          Soyadi = x.SOYAD,
                                                          AboneNo = x.ABONENO,
                                                      }), filtre?.Ara).ToList();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }

        }

        public AboneGenel AboneGenelBilgileriGetir(int aboneKayitNo)
        {
            using (var context = new AppContext())
            {
                return context.ENTABONEEf
                     .Where(x => x.KAYITNO == aboneKayitNo).Select(x => new AboneGenel
                     {
                         KayitNo = x.KAYITNO,
                         AboneNo = x.ABONENO,
                         Adi = x.AD,
                         Soyadi = x.SOYAD,
                         AdiSoyadi= x.AD+" "+x.SOYAD,
                         TcKimlikNo=x.KimlikNo,
                         SonSatisTarihi=x.SonSatisTarih
                     }).FirstOrDefault();

            }
        }
    }
}
