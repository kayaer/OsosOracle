using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.PRMTARIFESUComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class PRMTARIFESUManager : BaseManager, IPRMTARIFESUService
	{
		private readonly IPRMTARIFESUDal _pRMTARIFESUDal;
		public PRMTARIFESUManager(IPRMTARIFESUDal pRMTARIFESUDal)
		{
			_pRMTARIFESUDal = pRMTARIFESUDal;
		}

		public PRMTARIFESU GetirById(int id)
		{
			return _pRMTARIFESUDal.Getir(id);
		}

		public PRMTARIFESUDetay DetayGetirById(int id)
		{
			return _pRMTARIFESUDal.DetayGetir(id);
		}

		public List<PRMTARIFESU> Getir(PRMTARIFESUAra filtre)
		{
			return _pRMTARIFESUDal.Getir(filtre);
		}

		public int KayitSayisiGetir(PRMTARIFESUAra filtre)
		{
			return _pRMTARIFESUDal.KayitSayisiGetir(filtre);
		}

		public List<PRMTARIFESUDetay> DetayGetir(PRMTARIFESUAra filtre)
		{
			return _pRMTARIFESUDal.DetayGetir(filtre);
		}

		public PRMTARIFESUDataTable Ara(PRMTARIFESUAra filtre = null)
		{
			return _pRMTARIFESUDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(PRMTARIFESUValidator))]
		private void Validate(PRMTARIFESU entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<PRMTARIFESU> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_pRMTARIFESUDal.Ekle(entityler.ConvertEfList<PRMTARIFESU, PRMTARIFESUEf>());
		}

		public void Guncelle(List<PRMTARIFESU> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_pRMTARIFESUDal.Guncelle(entityler.ConvertEfList<PRMTARIFESU, PRMTARIFESUEf>());
		}

		public void Sil(List<int> idler)
		{
			_pRMTARIFESUDal.Sil(idler);
		}

	}
}