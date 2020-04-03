using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.NESNEDEGERComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class NESNEDEGERManager : BaseManager, INESNEDEGERService
	{
		private readonly INESNEDEGERDal _nESNEDEGERDal;
		public NESNEDEGERManager(INESNEDEGERDal nESNEDEGERDal)
		{
			_nESNEDEGERDal = nESNEDEGERDal;
		}

		public NESNEDEGER GetirById(int id)
		{
			return _nESNEDEGERDal.Getir(id);
		}

		public NESNEDEGERDetay DetayGetirById(int id)
		{
			return _nESNEDEGERDal.DetayGetir(id);
		}

		public List<NESNEDEGER> Getir(NESNEDEGERAra filtre)
		{
			return _nESNEDEGERDal.Getir(filtre);
		}

		public int KayitSayisiGetir(NESNEDEGERAra filtre)
		{
			return _nESNEDEGERDal.KayitSayisiGetir(filtre);
		}

		public List<NESNEDEGERDetay> DetayGetir(NESNEDEGERAra filtre)
		{
			return _nESNEDEGERDal.DetayGetir(filtre);
		}

		public NESNEDEGERDataTable Ara(NESNEDEGERAra filtre = null)
		{
			return _nESNEDEGERDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(NESNEDEGERValidator))]
		private void Validate(NESNEDEGER entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<NESNEDEGER> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_nESNEDEGERDal.Ekle(entityler.ConvertEfList<NESNEDEGER, NESNEDEGEREf>());
		}

		public void Guncelle(List<NESNEDEGER> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_nESNEDEGERDal.Guncelle(entityler.ConvertEfList<NESNEDEGER, NESNEDEGEREf>());
		}

		public void Sil(List<int> idler)
		{
			_nESNEDEGERDal.Sil(idler);
		}

	}
}