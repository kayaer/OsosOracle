using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.NESNETIPComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class NESNETIPManager : BaseManager, INESNETIPService
	{
		private readonly INESNETIPDal _nESNETIPDal;
		public NESNETIPManager(INESNETIPDal nESNETIPDal)
		{
			_nESNETIPDal = nESNETIPDal;
		}

		public NESNETIP GetirById(int id)
		{
			return _nESNETIPDal.Getir(id);
		}

		public NESNETIPDetay DetayGetirById(int id)
		{
			return _nESNETIPDal.DetayGetir(id);
		}

		public List<NESNETIP> Getir(NESNETIPAra filtre)
		{
			return _nESNETIPDal.Getir(filtre);
		}

		public int KayitSayisiGetir(NESNETIPAra filtre)
		{
			return _nESNETIPDal.KayitSayisiGetir(filtre);
		}

		public List<NESNETIPDetay> DetayGetir(NESNETIPAra filtre)
		{
			return _nESNETIPDal.DetayGetir(filtre);
		}

		public NESNETIPDataTable Ara(NESNETIPAra filtre = null)
		{
			return _nESNETIPDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(NESNETIPValidator))]
		private void Validate(NESNETIP entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<NESNETIP> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_nESNETIPDal.Ekle(entityler.ConvertEfList<NESNETIP, NESNETIPEf>());
		}

		public void Guncelle(List<NESNETIP> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_nESNETIPDal.Guncelle(entityler.ConvertEfList<NESNETIP, NESNETIPEf>());
		}

		public void Sil(List<int> idler)
		{
			_nESNETIPDal.Sil(idler);
		}

	}
}