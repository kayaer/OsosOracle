using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.PRMTARIFEGAZComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class PRMTARIFEGAZManager : BaseManager, IPRMTARIFEGAZService
	{
		private readonly IPRMTARIFEGAZDal _pRMTARIFEGAZDal;
		public PRMTARIFEGAZManager(IPRMTARIFEGAZDal pRMTARIFEGAZDal)
		{
			_pRMTARIFEGAZDal = pRMTARIFEGAZDal;
		}

		public PRMTARIFEGAZ GetirById(int id)
		{
			return _pRMTARIFEGAZDal.Getir(id);
		}

		public PRMTARIFEGAZDetay DetayGetirById(int id)
		{
			return _pRMTARIFEGAZDal.DetayGetir(id);
		}

		public List<PRMTARIFEGAZ> Getir(PRMTARIFEGAZAra filtre)
		{
			return _pRMTARIFEGAZDal.Getir(filtre);
		}

		public int KayitSayisiGetir(PRMTARIFEGAZAra filtre)
		{
			return _pRMTARIFEGAZDal.KayitSayisiGetir(filtre);
		}

		public List<PRMTARIFEGAZDetay> DetayGetir(PRMTARIFEGAZAra filtre)
		{
			return _pRMTARIFEGAZDal.DetayGetir(filtre);
		}

		public PRMTARIFEGAZDataTable Ara(PRMTARIFEGAZAra filtre = null)
		{
			return _pRMTARIFEGAZDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(PRMTARIFEGAZValidator))]
		private void Validate(PRMTARIFEGAZ entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<PRMTARIFEGAZ> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_pRMTARIFEGAZDal.Ekle(entityler.ConvertEfList<PRMTARIFEGAZ, PRMTARIFEGAZEf>());
		}

		public void Guncelle(List<PRMTARIFEGAZ> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_pRMTARIFEGAZDal.Guncelle(entityler.ConvertEfList<PRMTARIFEGAZ, PRMTARIFEGAZEf>());
		}

		public void Sil(List<int> idler)
		{
			_pRMTARIFEGAZDal.Sil(idler);
		}

	}
}