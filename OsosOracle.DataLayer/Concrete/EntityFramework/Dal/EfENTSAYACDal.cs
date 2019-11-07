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
                KAYITNO=x.KAYITNO,
                SAYACID = x.SAYACID,
                SERINO = x.SERINO,
                SAYACTUR = x.SAYACTUR,
                ACIKLAMA = x.ACIKLAMA,
                SayacTipi=x.CstSayacModelEf.AD,
                Kurum=x.ConKurumEf.AD,
                PrmTarifeSuDetay=new Entities.ComplexType.PRMTARIFESUComplexTypes.PRMTARIFESUDetay { AD=x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeSuEf.AD,SAYACCAP=x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeSuEf.SAYACCAP,
                TUKETIMKATSAYI=x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeSuEf.TUKETIMKATSAYI,BIRIMFIYAT=x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeSuEf.BIRIMFIYAT},
                PrmTarifeElkDetay = new Entities.ComplexType.PRMTARIFEELKComplexTypes.PRMTARIFEELKDetay
                {
                    AD = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.AD,
                    YEDEKKREDI = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.YEDEKKREDI,
                    KRITIKKREDI = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.KRITIKKREDI,
                    CARPAN = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.CARPAN,
                    FIYAT1 = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.FIYAT1,
                    FIYAT2 = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.FIYAT2,
                    FIYAT3 = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.FIYAT3,
                    LIMIT1 = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.LIMIT1,
                    LIMIT2 = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.LIMIT2,
                    YUKLEMELIMIT = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.YUKLEMELIMIT,
                    SABITUCRET = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.SABITUCRET,
                    KREDIKATSAYI = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.KREDIKATSAYI,
                    BAYRAM1GUN = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.BAYRAM1GUN,
                    BAYRAM1AY = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.BAYRAM1AY,
                    BAYRAM1SURE = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.BAYRAM1SURE,
                    BAYRAM2GUN = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.BAYRAM2GUN,
                    BAYRAM2AY = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.BAYRAM2AY,
                    BAYRAM2SURE = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.BAYRAM2SURE,
                    AKSAMSAAT = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.AKSAMSAAT,
                    SABAHSAAT = x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.SABAHSAAT,
                    HAFTASONUAKSAM=x.EntAboneSayacEfCollection.FirstOrDefault().PrmTarifeElkEf.HAFTASONUAKSAM

                },
                SayacModelKayitNo=x.CstSayacModelEf.KAYITNO


            });
        }




        private IQueryable<ENTSAYACEf> Filtrele(IQueryable<ENTSAYACEf> result, ENTSAYACAra filtre = null)
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

                    if (filtre.SAYACMONTAJTARIH != null)
                    {
                        result = result.Where(x => x.SAYACMONTAJTARIH == filtre.SAYACMONTAJTARIH);
                    }
                    if (!string.IsNullOrEmpty(filtre.SAYACID))
                    {
                        result = result.Where(x => x.SAYACID.Contains(filtre.SAYACID));
                    }
                    if (filtre.KURUMKAYITNO != null)
                    {
                        result = result.Where(x => x.KURUMKAYITNO == filtre.KURUMKAYITNO);
                    }
                    if (filtre.SERINO != null)
                    {
                        result = result.Where(x => x.SERINO == filtre.SERINO);
                    }
                    if (filtre.SAYACTUR != null)
                    {
                        result = result.Where(x => x.SAYACTUR == filtre.SAYACTUR);
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
                        result = result.Where(x => filtre.SayacSeriNoIceren.ToLower().Trim().Contains(x.SAYACID));
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
