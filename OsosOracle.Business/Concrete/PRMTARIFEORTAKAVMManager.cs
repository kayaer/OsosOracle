using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.PRMTARIFEORTAKAVMComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class PRMTARIFEORTAKAVMManager : BaseManager, IPRMTARIFEORTAKAVMService
	{
		private readonly IPRMTARIFEORTAKAVMDal _pRMTARIFEORTAKAVMDal;
		public PRMTARIFEORTAKAVMManager(IPRMTARIFEORTAKAVMDal pRMTARIFEORTAKAVMDal)
		{
			_pRMTARIFEORTAKAVMDal = pRMTARIFEORTAKAVMDal;
		}

		public PRMTARIFEORTAKAVM GetirById(int id)
		{
			return _pRMTARIFEORTAKAVMDal.Getir(id);
		}

		public PRMTARIFEORTAKAVMDetay DetayGetirById(int id)
		{
			return _pRMTARIFEORTAKAVMDal.DetayGetir(id);
		}

		public List<PRMTARIFEORTAKAVM> Getir(PRMTARIFEORTAKAVMAra filtre)
		{
			return _pRMTARIFEORTAKAVMDal.Getir(filtre);
		}

		public int KayitSayisiGetir(PRMTARIFEORTAKAVMAra filtre)
		{
			return _pRMTARIFEORTAKAVMDal.KayitSayisiGetir(filtre);
		}

		public List<PRMTARIFEORTAKAVMDetay> DetayGetir(PRMTARIFEORTAKAVMAra filtre)
		{
			return _pRMTARIFEORTAKAVMDal.DetayGetir(filtre);
		}

		public PRMTARIFEORTAKAVMDataTable Ara(PRMTARIFEORTAKAVMAra filtre = null)
		{
			return _pRMTARIFEORTAKAVMDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(PRMTARIFEORTAKAVMValidator))]
		private void Validate(PRMTARIFEORTAKAVM entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<PRMTARIFEORTAKAVM> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_pRMTARIFEORTAKAVMDal.Ekle(entityler.ConvertEfList<PRMTARIFEORTAKAVM, PRMTARIFEORTAKAVMEf>());
		}

		public void Guncelle(List<PRMTARIFEORTAKAVM> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_pRMTARIFEORTAKAVMDal.Guncelle(entityler.ConvertEfList<PRMTARIFEORTAKAVM, PRMTARIFEORTAKAVMEf>());
		}

		public void Sil(List<int> idler)
		{
			_pRMTARIFEORTAKAVMDal.Sil(idler);
		}

	}
}