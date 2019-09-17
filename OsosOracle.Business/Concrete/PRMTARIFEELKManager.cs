using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.PRMTARIFEELKComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class PRMTARIFEELKManager : BaseManager, IPRMTARIFEELKService
	{
		private readonly IPRMTARIFEELKDal _pRMTARIFEELKDal;
		public PRMTARIFEELKManager(IPRMTARIFEELKDal pRMTARIFEELKDal)
		{
			_pRMTARIFEELKDal = pRMTARIFEELKDal;
		}

		public PRMTARIFEELK GetirById(int id)
		{
			return _pRMTARIFEELKDal.Getir(id);
		}

		public PRMTARIFEELKDetay DetayGetirById(int id)
		{
			return _pRMTARIFEELKDal.DetayGetir(id);
		}

		public List<PRMTARIFEELK> Getir(PRMTARIFEELKAra filtre)
		{
			return _pRMTARIFEELKDal.Getir(filtre);
		}

		public int KayitSayisiGetir(PRMTARIFEELKAra filtre)
		{
			return _pRMTARIFEELKDal.KayitSayisiGetir(filtre);
		}

		public List<PRMTARIFEELKDetay> DetayGetir(PRMTARIFEELKAra filtre)
		{
			return _pRMTARIFEELKDal.DetayGetir(filtre);
		}

		public PRMTARIFEELKDataTable Ara(PRMTARIFEELKAra filtre = null)
		{
			return _pRMTARIFEELKDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(PRMTARIFEELKValidator))]
		private void Validate(PRMTARIFEELK entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<PRMTARIFEELK> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_pRMTARIFEELKDal.Ekle(entityler.ConvertEfList<PRMTARIFEELK, PRMTARIFEELKEf>());
		}

		public void Guncelle(List<PRMTARIFEELK> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_pRMTARIFEELKDal.Guncelle(entityler.ConvertEfList<PRMTARIFEELK, PRMTARIFEELKEf>());
		}

		public void Sil(List<int> idler)
		{
			_pRMTARIFEELKDal.Sil(idler);
		}

	}
}