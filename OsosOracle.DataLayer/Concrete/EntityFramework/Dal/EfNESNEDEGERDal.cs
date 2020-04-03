using System.Collections.Generic;
using System.Linq;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.NESNEDEGERComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Dal
{
	public class EfNESNEDEGERDal : INESNEDEGERDal
	{


		private IQueryable<NESNEDEGERDetay> DetayDoldur(IQueryable<NESNEDEGEREf> result)
		{
			return result.Select(x => new NESNEDEGERDetay()
			{
				//	NESNEDEGER = x,
				 KAYITNO = x.KAYITNO,

 NESNETIPKAYITNO = x.NESNETIPKAYITNO,
 AD = x.AD,
 DEGER = x.DEGER,
 BILGI = x.BILGI,
 SIRANO = x.SIRANO,
			
				//TODO: Ek detayları buraya ekleyiniz
				//örnek: NESNEDEGERDurumu = x.NesneDegerDurumEf.Adi

			});
		}

		


		private IQueryable<NESNEDEGEREf> Filtrele(IQueryable<NESNEDEGEREf> result, NESNEDEGERAra filtre = null)
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

				   if (filtre.NesneTipId != null)
					{
						result = result.Where(x => x.NESNETIPKAYITNO == filtre.NesneTipId);
					}
				   if (!string.IsNullOrEmpty(filtre.AD))
					{
						result = result.Where(x => x.AD.Contains(filtre.AD));
					}
				   if (!string.IsNullOrEmpty(filtre.DEGER))
					{
						result = result.Where(x => x.DEGER.Contains(filtre.DEGER));
					}
				   if (!string.IsNullOrEmpty(filtre.BILGI))
					{
						result = result.Where(x => x.BILGI.Contains(filtre.BILGI));
					}
				   if (filtre.SIRANO != null)
					{
						result = result.Where(x => x.SIRANO == filtre.SIRANO);
					}
			}
			}
			return result;
		 }

		public List<NESNEDEGER> Getir(NESNEDEGERAra filtre = null)
		{
			using (var context = new AppContext())
			{
				return GetirWithContext(context, filtre);
			}
		}

		public List<NESNEDEGER> GetirWithContext(AppContext context, NESNEDEGERAra filtre = null)
		{
			var filterHelper = new FilterHelper<NESNEDEGEREf>();
			return filterHelper.Sayfala(Filtrele(context.NESNEDEGEREf.AsQueryable(), filtre), filtre?.Ara).ToList<NESNEDEGER>();
		}


		public List< NESNEDEGERDetay> DetayGetir( NESNEDEGERAra filtre = null)
		{
			using (var context = new AppContext())
			{
				var filterHelper = new FilterHelper<NESNEDEGERDetay>();
				return filterHelper.Sayfala(DetayDoldur(Filtrele(context.NESNEDEGEREf.AsQueryable(), filtre)), filtre?.Ara).ToList();
				
			}
		}


		public NESNEDEGERDataTable Ara(NESNEDEGERAra filtre = null)
		{
			using (var context = new AppContext())
			{
				 var filterHelper = new FilterHelper<NESNEDEGERDetay>();
				
				return new NESNEDEGERDataTable
				{
					NESNEDEGERDetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.NESNEDEGEREf.AsQueryable(), filtre)), filtre?.Ara).ToList(),ToplamKayitSayisi = filterHelper.KayitSayisi
				};
			}
		}

		public int KayitSayisiGetir(NESNEDEGERAra filtre = null)
		{
			using (var context = new AppContext())
			{
				var filterHelper = new FilterHelper<NESNEDEGEREf>();
				return filterHelper.KayitSayisiGetir(Filtrele(context.NESNEDEGEREf.AsQueryable(), filtre), filtre?.Ara);
			}
		 }
   

		public NESNEDEGER Getir(int id)
		{
			using (var context = new AppContext())
			{
				return context.NESNEDEGEREf.Find(id);
			}
		}

		public NESNEDEGERDetay DetayGetir(int id)
		{
			using (var context = new AppContext())
			{
				 return DetayDoldur(Filtrele(context.NESNEDEGEREf.AsQueryable(), new NESNEDEGERAra() { KAYITNO = id }))
				   .FirstOrDefault();
			}
		}

		public List<NESNEDEGER> Ekle(List<NESNEDEGEREf> entityler)
		{
			using (var context = new AppContext())
			{
				if (entityler.Count == 1)
				{
					var eklenen = context.NESNEDEGEREf.Add(entityler[0]);
					context.SaveChanges();
					return new List<NESNEDEGER> { eklenen };
				}
				if (entityler.Count > 1)
				{
					var eklenen = context.NESNEDEGEREf.AddRange(entityler);
					context.SaveChanges();
					return eklenen.ToList<NESNEDEGER>();
				}

                return null;
			}
		}


		public void Guncelle(List<NESNEDEGEREf> yeniDegerler)
		{
			using (var context = new AppContext())
			{
				var mevcutDegerler = new List<NESNEDEGEREf>();
				if (yeniDegerler.Count == 0)
					throw new NotificationException("Değer yok.");
				if (yeniDegerler.Count == 1)
				{
					mevcutDegerler.Add(context.NESNEDEGEREf.Find(yeniDegerler[0].KAYITNO));
				}
				else
				{
					var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
					mevcutDegerler = context.NESNEDEGEREf.Where(x => idler.Contains(x.KAYITNO)).ToList();
				}

				AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
				context.SaveChanges();
			}
		}

		private static void AlanlariGuncelle(AppContext context, List<NESNEDEGEREf> mevcutDegerler, List<NESNEDEGEREf> yeniDegerler)
		{
			foreach (var yeniDeger in yeniDegerler)
			{
				var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
				if (mevcutDeger == null)
					throw new NotificationException("NESNEDEGER bulunamadı");

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
					var entry = context.NESNEDEGEREf.Find(idler[0]);
					if (entry != null)
						context.NESNEDEGEREf.Remove(entry);
				}
				else if (idler.Count > 1)
				{
					var entry = context.NESNEDEGEREf.Where(x => idler.Contains(x.KAYITNO)).ToList();
					context.NESNEDEGEREf.RemoveRange(entry);
				}

				context.SaveChanges();
			}
		}

	}
}
