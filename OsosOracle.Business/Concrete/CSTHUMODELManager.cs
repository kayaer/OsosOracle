using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.CSTHUMODELComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class CSTHUMODELManager : BaseManager, ICSTHUMODELService
	{
		private readonly ICSTHUMODELDal _cSTHUMODELDal;
		public CSTHUMODELManager(ICSTHUMODELDal cSTHUMODELDal)
		{
			_cSTHUMODELDal = cSTHUMODELDal;
		}

		public CSTHUMODEL GetirById(int id)
		{
			return _cSTHUMODELDal.Getir(id);
		}

		public CSTHUMODELDetay DetayGetirById(int id)
		{
			return _cSTHUMODELDal.DetayGetir(id);
		}

		public List<CSTHUMODEL> Getir(CSTHUMODELAra filtre)
		{
			return _cSTHUMODELDal.Getir(filtre);
		}

		public int KayitSayisiGetir(CSTHUMODELAra filtre)
		{
			return _cSTHUMODELDal.KayitSayisiGetir(filtre);
		}

		public List<CSTHUMODELDetay> DetayGetir(CSTHUMODELAra filtre)
		{
			return _cSTHUMODELDal.DetayGetir(filtre);
		}

		public CSTHUMODELDataTable Ara(CSTHUMODELAra filtre = null)
		{
			return _cSTHUMODELDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(CSTHUMODELValidator))]
		private void Validate(CSTHUMODEL entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<CSTHUMODEL> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_cSTHUMODELDal.Ekle(entityler.ConvertEfList<CSTHUMODEL, CSTHUMODELEf>());
		}

		public void Guncelle(List<CSTHUMODEL> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_cSTHUMODELDal.Guncelle(entityler.ConvertEfList<CSTHUMODEL, CSTHUMODELEf>());
		}

		public void Sil(List<int> idler)
		{
			_cSTHUMODELDal.Sil(idler);
		}

	}
}