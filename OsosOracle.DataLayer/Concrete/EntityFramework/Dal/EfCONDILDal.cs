using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.CONDILComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
	public class EfCONDILDal : ICONDILDal
	{


		private IQueryable<CONDILDetay> DetayDoldur(IQueryable<CONDILEf> result)
		{
			return result.Select(x => new CONDILDetay()
			{
				//	CONDIL = x,
				 KAYITNO = x.KAYITNO,

 DIL = x.DIL,
			
				//TODO: Ek detayları buraya ekleyiniz
				//örnek: CONDILDurumu = x.NesneDegerDurumEf.Adi

			});
		}

		


		private IQueryable<CONDILEf> Filtrele(IQueryable<CONDILEf> result, CONDILAra filtre = null)
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

				   if (!string.IsNullOrEmpty(filtre.DIL))
					{
						result = result.Where(x => x.DIL.Contains(filtre.DIL));
					}
			}
			}
			return result;
		 }

		public List<CONDIL> Getir(CONDILAra filtre = null)
		{
			using (var context = new AppContext())
			{
				return GetirWithContext(context, filtre);
			}
		}

		public List<CONDIL> GetirWithContext(AppContext context, CONDILAra filtre = null)
		{
			var filterHelper = new FilterHelper<CONDILEf>();
			return filterHelper.Sayfala(Filtrele(context.CONDILEf.AsQueryable(), filtre), filtre?.Ara).ToList<CONDIL>();
		}


		public List< CONDILDetay> DetayGetir( CONDILAra filtre = null)
		{
			using (var context = new AppContext())
			{
				var filterHelper = new FilterHelper<CONDILDetay>();
				return filterHelper.Sayfala(DetayDoldur(Filtrele(context.CONDILEf.AsQueryable(), filtre)), filtre?.Ara).ToList();
				
			}
		}


		public CONDILDataTable Ara(CONDILAra filtre = null)
		{
			using (var context = new AppContext())
			{
				 var filterHelper = new FilterHelper<CONDILDetay>();
				
				return new CONDILDataTable
				{
					CONDILDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.CONDILEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),ToplamKayitSayisi = filterHelper.KayitSayisi
				};
			}
		}

		public int KayitSayisiGetir(CONDILAra filtre = null)
		{
			using (var context = new AppContext())
			{
				var filterHelper = new FilterHelper<CONDILEf>();
				return filterHelper.KayitSayisiGetir(Filtrele(context.CONDILEf.AsQueryable(), filtre), filtre?.Ara);
			}
		 }
   

		public CONDIL Getir(int id)
		{
			using (var context = new AppContext())
			{
				return context.CONDILEf.Find(id);
			}
		}

		public CONDILDetay DetayGetir(int id)
		{
			using (var context = new AppContext())
			{
				 return DetayDoldur(Filtrele(context.CONDILEf.AsQueryable(), new CONDILAra() { KAYITNO = id }))
				   .FirstOrDefault();
			}
		}

		public List<CONDIL> Ekle(List<CONDILEf> entityler)
		{
			using (var context = new AppContext())
			{
				if (entityler.Count == 1)
				{
					var eklenen = context.CONDILEf.Add(entityler[0]);
					context.SaveChanges();
					return new List<CONDIL> { eklenen };
				}
				if (entityler.Count > 1)
				{
					var eklenen = context.CONDILEf.AddRange(entityler);
					context.SaveChanges();
					return eklenen.ToList<CONDIL>();
				}

                return null;
			}
		}


		public void Guncelle(List<CONDILEf> yeniDegerler)
		{
			using (var context = new AppContext())
			{
				var mevcutDegerler = new List<CONDILEf>();
				if (yeniDegerler.Count == 0)
					throw new NotificationException("Değer yok.");
				if (yeniDegerler.Count == 1)
				{
					mevcutDegerler.Add(context.CONDILEf.Find(yeniDegerler[0].KAYITNO));
				}
				else
				{
					var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
					mevcutDegerler = context.CONDILEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
				}

				AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
				context.SaveChanges();
			}
		}

		private static void AlanlariGuncelle(AppContext context, List<CONDILEf> mevcutDegerler, List<CONDILEf> yeniDegerler)
		{
			foreach (var yeniDeger in yeniDegerler)
			{
				var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
				if (mevcutDeger == null)
					throw new NotificationException("CONDIL bulunamadı");

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
					var entry = context.CONDILEf.Find(idler[0]);
					if (entry != null)
						context.CONDILEf.Remove(entry);
				}
				else if (idler.Count > 1)
				{
					var entry = context.CONDILEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
					context.CONDILEf.RemoveRange(entry);
				}

				context.SaveChanges();
			}
		}

	}
}
