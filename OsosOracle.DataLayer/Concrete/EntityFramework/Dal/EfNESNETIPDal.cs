using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.NESNETIPComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
	public class EfNESNETIPDal : INESNETIPDal
	{


		private IQueryable<NESNETIPDetay> DetayDoldur(IQueryable<NESNETIPEf> result)
		{
			return result.Select(x => new NESNETIPDetay()
			{
				//	NESNETIP = x,
				 KAYITNO = x.KAYITNO,

 ADI = x.ADI,
			
				//TODO: Ek detayları buraya ekleyiniz
				//örnek: NESNETIPDurumu = x.NesneDegerDurumEf.Adi

			});
		}

		


		private IQueryable<NESNETIPEf> Filtrele(IQueryable<NESNETIPEf> result, NESNETIPAra filtre = null)
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

				   if (!string.IsNullOrEmpty(filtre.ADI))
					{
						result = result.Where(x => x.ADI.Contains(filtre.ADI));
					}
			}
			}
			return result;
		 }

		public List<NESNETIP> Getir(NESNETIPAra filtre = null)
		{
			using (var context = new AppContext())
			{
				return GetirWithContext(context, filtre);
			}
		}

		public List<NESNETIP> GetirWithContext(AppContext context, NESNETIPAra filtre = null)
		{
			var filterHelper = new FilterHelper<NESNETIPEf>();
			return filterHelper.Sayfala(Filtrele(context.NESNETIPEf.AsQueryable(), filtre), filtre?.Ara).ToList<NESNETIP>();
		}


		public List< NESNETIPDetay> DetayGetir( NESNETIPAra filtre = null)
		{
			using (var context = new AppContext())
			{
				var filterHelper = new FilterHelper<NESNETIPDetay>();
				return filterHelper.Sayfala(DetayDoldur(Filtrele(context.NESNETIPEf.AsQueryable(), filtre)), filtre?.Ara).ToList();
				
			}
		}


		public NESNETIPDataTable Ara(NESNETIPAra filtre = null)
		{
			using (var context = new AppContext())
			{
				 var filterHelper = new FilterHelper<NESNETIPDetay>();
				
				return new NESNETIPDataTable
				{
					NESNETIPDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.NESNETIPEf.AsQueryable(), filtre)), filtre?.Ara).ToList(),ToplamKayitSayisi = filterHelper.KayitSayisi
				};
			}
		}

		public int KayitSayisiGetir(NESNETIPAra filtre = null)
		{
			using (var context = new AppContext())
			{
				var filterHelper = new FilterHelper<NESNETIPEf>();
				return filterHelper.KayitSayisiGetir(Filtrele(context.NESNETIPEf.AsQueryable(), filtre), filtre?.Ara);
			}
		 }
   

		public NESNETIP Getir(int id)
		{
			using (var context = new AppContext())
			{
				return context.NESNETIPEf.Find(id);
			}
		}

		public NESNETIPDetay DetayGetir(int id)
		{
			using (var context = new AppContext())
			{
				 return DetayDoldur(Filtrele(context.NESNETIPEf.AsQueryable(), new NESNETIPAra() { KAYITNO = id }))
				   .FirstOrDefault();
			}
		}

		public List<NESNETIP> Ekle(List<NESNETIPEf> entityler)
		{
			using (var context = new AppContext())
			{
				if (entityler.Count == 1)
				{
					var eklenen = context.NESNETIPEf.Add(entityler[0]);
					context.SaveChanges();
					return new List<NESNETIP> { eklenen };
				}
				if (entityler.Count > 1)
				{
					var eklenen = context.NESNETIPEf.AddRange(entityler);
					context.SaveChanges();
					return eklenen.ToList<NESNETIP>();
				}

                return null;
			}
		}


		public void Guncelle(List<NESNETIPEf> yeniDegerler)
		{
			using (var context = new AppContext())
			{
				var mevcutDegerler = new List<NESNETIPEf>();
				if (yeniDegerler.Count == 0)
					throw new NotificationException("Değer yok.");
				if (yeniDegerler.Count == 1)
				{
					mevcutDegerler.Add(context.NESNETIPEf.Find(yeniDegerler[0].KAYITNO));
				}
				else
				{
					var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
					mevcutDegerler = context.NESNETIPEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
				}

				AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
				context.SaveChanges();
			}
		}

		private static void AlanlariGuncelle(AppContext context, List<NESNETIPEf> mevcutDegerler, List<NESNETIPEf> yeniDegerler)
		{
			foreach (var yeniDeger in yeniDegerler)
			{
				var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
				if (mevcutDeger == null)
					throw new NotificationException("NESNETIP bulunamadı");

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
					var entry = context.NESNETIPEf.Find(idler[0]);
					if (entry != null)
						context.NESNETIPEf.Remove(entry);
				}
				else if (idler.Count > 1)
				{
					var entry = context.NESNETIPEf.Where(x => idler.Contains(x.KAYITNO)).ToList();
					context.NESNETIPEf.RemoveRange(entry);
				}

				context.SaveChanges();
			}
		}

	}
}
