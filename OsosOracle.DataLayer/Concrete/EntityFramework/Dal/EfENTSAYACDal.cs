using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTSAYACComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
    public class EfENTSAYACDal : IENTSAYACDal
    {


        private IQueryable<ENTSAYACDetay> DetayDoldur(IQueryable<ENTSAYACEf> result)
        {
            return result.Select(x => new ENTSAYACDetay()
            {
                KAYITNO = x.KAYITNO,
                SERINO = x.SERINO,
                KapakSeriNo=x.KapakSeriNo,
                ACIKLAMA = x.ACIKLAMA,
                SayacTipi = x.CstSayacModelEf.AD,
                Kurum = x.ConKurumEf.AD,
                SayacModelKayitNo = x.CstSayacModelEf.KAYITNO,
                TarifeKayitNo = x.EntAboneSayacEfCollection.FirstOrDefault().TARIFEKAYITNO,
                Cap=x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeSuEf.SAYACCAP,
                SonSatisTarih = x.EntAboneSayacEfCollection.FirstOrDefault().SONSATISTARIH,
                SayacTuru=x.CstSayacModelEf.NesneDegerSayacTuruEf.KAYITNO
                

            });
        }




        private IQueryable<ENTSAYACEf> Filtrele(IQueryable<ENTSAYACEf> result, ENTSAYACAra filtre = null)
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

                    //if (!string.IsNullOrEmpty(filtre.Idler))
                    //{
                    //	var idList = filtre.Idler.ToList<int>();
                    //	result = result.Where(x => idList.Contains(x.KAYITNO));
                    //}

                    if (filtre.SayacTuru != null)
                    {
                        result = result.Where(x => x.CstSayacModelEf.SayacTuruKayitNo == filtre.SayacTuru);
                    }

                    if (filtre.SAYACMONTAJTARIH != null)
                    {
                        result = result.Where(x => x.SAYACMONTAJTARIH == filtre.SAYACMONTAJTARIH);
                    }
                   
                    if (filtre.KURUMKAYITNO != null)
                    {
                        result = result.Where(x => x.KURUMKAYITNO == filtre.KURUMKAYITNO);
                    }
                    if (!string.IsNullOrEmpty( filtre.SERINO))
                    {
                        result = result.Where(x => x.SERINO.Contains(filtre.SERINO));
                    }
                    if (filtre.SayacModelKayitNo != null)
                    {
                        result = result.Where(x => x.SayacModelKayitNo == filtre.SayacModelKayitNo);
                    }
                    if (!string.IsNullOrEmpty(filtre.ACIKLAMA))
                    {
                        result = result.Where(x => x.ACIKLAMA.Contains(filtre.ACIKLAMA));
                    }
                    if (filtre.DURUM != null)
                    {
                        result = result.Where(x => x.DURUM == filtre.DURUM);
                    }
                    if (filtre.VERSIYON != null)
                    {
                        result = result.Where(x => x.VERSIYON == filtre.VERSIYON);
                    }
                    if (!string.IsNullOrEmpty(filtre.SayacSeriNoIceren))
                    {
                        result = result.Where(x => x.SERINO.Contains(filtre.SayacSeriNoIceren.ToLower()));
                    }
                    if (!string.IsNullOrEmpty( filtre.KapakSeriNo))
                    {
                        result = result.Where(x => x.KapakSeriNo.Contains(filtre.KapakSeriNo));
                    }
                }
            }
            return result;
        }

        public List<ENTSAYAC> Getir(ENTSAYACAra filtre = null)
        {
            using (var context = new AppContext())
            {
                return GetirWithContext(context, filtre);
            }
        }

        public List<ENTSAYAC> GetirWithContext(AppContext context, ENTSAYACAra filtre = null)
        {
            var filterHelper = new FilterHelper<ENTSAYACEf>();
            return filterHelper.Sayfala(Filtrele(context.ENTSAYACEf.AsQueryable(), filtre), filtre?.Ara).ToList<ENTSAYAC>();
        }


        public List<ENTSAYACDetay> DetayGetir(ENTSAYACAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTSAYACDetay>();
                return filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTSAYACEf.AsQueryable(), filtre)), filtre?.Ara).ToList();

            }
        }


        public ENTSAYACDataTable Ara(ENTSAYACAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTSAYACDetay>();

                return new ENTSAYACDataTable
                {
                    ENTSAYACDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.ENTSAYACEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),
                    ToplamKayitSayisi = filterHelper.KayitSayisi
                };
            }
        }

        public int KayitSayisiGetir(ENTSAYACAra filtre = null)
        {
            using (var context = new AppContext())
            {
                var filterHelper = new FilterHelper<ENTSAYACEf>();
                return filterHelper.KayitSayisiGetir(Filtrele(context.ENTSAYACEf.AsQueryable(), filtre), filtre?.Ara);
            }
        }


        public ENTSAYAC Getir(int id)
        {
            using (var context = new AppContext())
            {
                return context.ENTSAYACEf.Find(id);
            }
        }

        public ENTSAYACDetay DetayGetir(int id)
        {
            using (var context = new AppContext())
            {
                return DetayDoldur(Filtrele(context.ENTSAYACEf.AsQueryable(), new ENTSAYACAra() { KAYITNO = id }))
                  .FirstOrDefault();
            }
        }

        public List<ENTSAYAC> Ekle(List<ENTSAYACEf> entityler)
        {
            using (var context = new AppContext())
            {
                if (entityler.Count == 1)
                {
                    var eklenen = context.ENTSAYACEf.Add(entityler[0]);
                    context.SaveChanges();
                    return new List<ENTSAYAC> { eklenen };
                }
                if (entityler.Count > 1)
                {
                    var eklenen = context.ENTSAYACEf.AddRange(entityler);
                    context.SaveChanges();
                    return eklenen.ToList<ENTSAYAC>();
                }

                return null;
            }
        }


        public void Guncelle(List<ENTSAYACEf> yeniDegerler)
        {
            using (var context = new AppContext())
            {
                var mevcutDegerler = new List<ENTSAYACEf>();
                if (yeniDegerler.Count == 0)
                    throw new NotificationException("Değer yok.");
                if (yeniDegerler.Count == 1)
                {
                    mevcutDegerler.Add(context.ENTSAYACEf.Find(yeniDegerler[0].KAYITNO));
                }
                else
                {
                    var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
                    mevcutDegerler = context.ENTSAYACEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                }

                AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
                context.SaveChanges();
            }
        }

        private static void AlanlariGuncelle(AppContext context, List<ENTSAYACEf> mevcutDegerler, List<ENTSAYACEf> yeniDegerler)
        {
            foreach (var yeniDeger in yeniDegerler)
            {
                var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
                if (mevcutDeger == null)
                    throw new NotificationException("ENTSAYAC bulunamadı");

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
                    var entry = context.ENTSAYACEf.Find(idler[0]);
                    if (entry != null)
                        context.ENTSAYACEf.Remove(entry);
                }
                else if (idler.Count > 1)
                {
                    var entry = context.ENTSAYACEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
                    context.ENTSAYACEf.RemoveRange(entry);
                }

                context.SaveChanges();
            }
        }

    }
}
