using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.CSTSAYACMODELComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class CSTSAYACMODELManager : BaseManager, ICSTSAYACMODELService
	{
		private readonly ICSTSAYACMODELDal _cSTSAYACMODELDal;
		public CSTSAYACMODELManager(ICSTSAYACMODELDal cSTSAYACMODELDal)
		{
			_cSTSAYACMODELDal = cSTSAYACMODELDal;
		}

		public CSTSAYACMODEL GetirById(int id)
		{
			return _cSTSAYACMODELDal.Getir(id);
		}

		public CSTSAYACMODELDetay DetayGetirById(int id)
		{
			return _cSTSAYACMODELDal.DetayGetir(id);
		}

		public List<CSTSAYACMODEL> Getir(CSTSAYACMODELAra filtre)
		{
			return _cSTSAYACMODELDal.Getir(filtre);
		}

		public int KayitSayisiGetir(CSTSAYACMODELAra filtre)
		{
			return _cSTSAYACMODELDal.KayitSayisiGetir(filtre);
		}

		public List<CSTSAYACMODELDetay> DetayGetir(CSTSAYACMODELAra filtre)
		{
			return _cSTSAYACMODELDal.DetayGetir(filtre);
		}

		public CSTSAYACMODELDataTable Ara(CSTSAYACMODELAra filtre = null)
		{
			return _cSTSAYACMODELDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(CSTSAYACMODELValidator))]
		private void Validate(CSTSAYACMODEL entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<CSTSAYACMODEL> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_cSTSAYACMODELDal.Ekle(entityler.ConvertEfList<CSTSAYACMODEL, CSTSAYACMODELEf>());
		}

		public void Guncelle(List<CSTSAYACMODEL> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_cSTSAYACMODELDal.Guncelle(entityler.ConvertEfList<CSTSAYACMODEL, CSTSAYACMODELEf>());
		}

		public void Sil(List<int> idler)
		{
			_cSTSAYACMODELDal.Sil(idler);
		}

	}
}