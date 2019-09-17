using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSGOREVROLComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
	public class EfSYSGOREVROLDal : ISYSGOREVROLDal
	{


		private IQueryable<SYSGOREVROLDetay> DetayDoldur(IQueryable<SYSGOREVROLEf> result)
		{
			return result.Select(x => new SYSGOREVROLDetay()
			{
				//	SYSGOREVROL = x,
				 KAYITNO = x.KAYITNO,

 GOREVKAYITNO = x.GOREVKAYITNO,
 ROLKAYITNO = x.ROLKAYITNO,
 VERSIYON = x.VERSIYON,
			
				//TODO: Ek detayları buraya ekleyiniz
				//örnek: SYSGOREVROLDurumu = x.NesneDegerDurumEf.Adi

			});
		}

		


		private IQueryable<SYSGOREVROLEf> Filtrele(IQueryable<SYSGOREVROLEf> result, SYSGOREVROLAra filtre = null)
		{
			//silindi kolonu varsa silinenler gelmesin
			
			//TODO: filtereyi özelleştir
			if (filtre != null)
			{
				//id ve idler
				if (filtre.KAYITNO != null){result = result.Where(x => x.KAYITNO == filtre.KAYITNO);}
				else {

				//if (!string.IsNullOrEmpty(filtre.Idler))
				//{
				//	var idList = filtre.Idler.ToList<int>();
				//	result = result.Where(x => idList.Contains(x.KAYITNO));
				//}

				   if (filtre.GOREVKAYITNO != null)
					{
						result = result.Where(x => x.GOREVKAYITNO == filtre.GOREVKAYITNO);
					}
				   if (filtre.ROLKAYITNO != null)
					{
						result = result.Where(x => x.ROLKAYITNO == filtre.ROLKAYITNO);
					}
				   if (filtre.VERSIYON != null)
					{
						result = result.Where(x => x.VERSIYON == filtre.VERSIYON);
					}
			}
			}
			return result;
		 }

		public List<SYSGOREVROL> Getir(SYSGOREVROLAra filtre = null)
		{
			using (var context = new AppContext())
			{
				return GetirWithContext(context, filtre);
			}
		}

		public List<SYSGOREVROL> GetirWithContext(AppContext context, SYSGOREVROLAra filtre = null)
		{
			var filterHelper = new FilterHelper<SYSGOREVROLEf>();
			return filterHelper.Sayfala(Filtrele(context.SYSGOREVROLEf.AsQueryable(), filtre), filtre?.Ara).ToList<SYSGOREVROL>();
		}


		public List< SYSGOREVROLDetay> DetayGetir( SYSGOREVROLAra filtre = null)
		{
			using (var context = new AppContext())
			{
				var filterHelper = new FilterHelper<SYSGOREVROLDetay>();
				return filterHelper.Sayfala(DetayDoldur(Filtrele(context.SYSGOREVROLEf.AsQueryable(), filtre)), filtre?.Ara).ToList();
				
			}
		}


		public SYSGOREVROLDataTable Ara(SYSGOREVROLAra filtre = null)
		{
			using (var context = new AppContext())
			{
				 var filterHelper = new FilterHelper<SYSGOREVROLDetay>();
				
				return new SYSGOREVROLDataTable
				{
					SYSGOREVROLDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.SYSGOREVROLEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),ToplamKayitSayisi = filterHelper.KayitSayisi
				};
			}
		}

		public int KayitSayisiGetir(SYSGOREVROLAra filtre = null)
		{
			using (var context = new AppContext())
			{
				var filterHelper = new FilterHelper<SYSGOREVROLEf>();
				return filterHelper.KayitSayisiGetir(Filtrele(context.SYSGOREVROLEf.AsQueryable(), filtre), filtre?.Ara);
			}
		 }
   

		public SYSGOREVROL Getir(int id)
		{
			using (var context = new AppContext())
			{
				return context.SYSGOREVROLEf.Find(id);
			}
		}

		public SYSGOREVROLDetay DetayGetir(int id)
		{
			using (var context = new AppContext())
			{
				 return DetayDoldur(Filtrele(context.SYSGOREVROLEf.AsQueryable(), new SYSGOREVROLAra() { KAYITNO = id }))
				   .FirstOrDefault();
			}
		}

		public List<SYSGOREVROL> Ekle(List<SYSGOREVROLEf> entityler)
		{
			using (var context = new AppContext())
			{
				if (entityler.Count == 1)
				{
					var eklenen = context.SYSGOREVROLEf.Add(entityler[0]);
					context.SaveChanges();
					return new List<SYSGOREVROL> { eklenen };
				}
				if (entityler.Count > 1)
				{
					var eklenen = context.SYSGOREVROLEf.AddRange(entityler);
					context.SaveChanges();
					return eklenen.ToList<SYSGOREVROL>();
				}

                return null;
			}
		}


		public void Guncelle(List<SYSGOREVROLEf> yeniDegerler)
		{
			using (var context = new AppContext())
			{
				var mevcutDegerler = new List<SYSGOREVROLEf>();
				if (yeniDegerler.Count == 0)
					throw new NotificationException("Değer yok.");
				if (yeniDegerler.Count == 1)
				{
					mevcutDegerler.Add(context.SYSGOREVROLEf.Find(yeniDegerler[0].KAYITNO));
				}
				else
				{
					var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
					mevcutDegerler = context.SYSGOREVROLEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
				}

				AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
				context.SaveChanges();
			}
		}

		private static void AlanlariGuncelle(AppContext context, List<SYSGOREVROLEf> mevcutDegerler, List<SYSGOREVROLEf> yeniDegerler)
		{
			foreach (var yeniDeger in yeniDegerler)
			{
				var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
				if (mevcutDeger == null)
					throw new NotificationException("SYSGOREVROL bulunamadı");

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
					var entry = context.SYSGOREVROLEf.Find(idler[0]);
					if (entry != null)
						context.SYSGOREVROLEf.Remove(entry);
				}
				else if (idler.Count > 1)
				{
					var entry = context.SYSGOREVROLEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
					context.SYSGOREVROLEf.RemoveRange(entry);
				}

				context.SaveChanges();
			}
		}

	}
}
