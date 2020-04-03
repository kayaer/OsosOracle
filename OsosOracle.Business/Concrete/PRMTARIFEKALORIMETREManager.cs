using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.PRMTARIFEKALORIMETREComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;

namespace OsosOracle.Business.Concrete
{
	public class PRMTARIFEKALORIMETREManager : BaseManager, IPRMTARIFEKALORIMETREService
	{
		private readonly IPRMTARIFEKALORIMETREDal _pRMTARIFEKALORIMETREDal;
		public PRMTARIFEKALORIMETREManager(IPRMTARIFEKALORIMETREDal pRMTARIFEKALORIMETREDal)
		{
			_pRMTARIFEKALORIMETREDal = pRMTARIFEKALORIMETREDal;
		}

		public PRMTARIFEKALORIMETRE GetirById(int id)
		{
			return _pRMTARIFEKALORIMETREDal.Getir(id);
		}

		public PRMTARIFEKALORIMETREDetay DetayGetirById(int id)
		{
			return _pRMTARIFEKALORIMETREDal.DetayGetir(id);
		}

		public List<PRMTARIFEKALORIMETRE> Getir(PRMTARIFEKALORIMETREAra filtre)
		{
			return _pRMTARIFEKALORIMETREDal.Getir(filtre);
		}

		public int KayitSayisiGetir(PRMTARIFEKALORIMETREAra filtre)
		{
			return _pRMTARIFEKALORIMETREDal.KayitSayisiGetir(filtre);
		}

		public List<PRMTARIFEKALORIMETREDetay> DetayGetir(PRMTARIFEKALORIMETREAra filtre)
		{
			return _pRMTARIFEKALORIMETREDal.DetayGetir(filtre);
		}

		public PRMTARIFEKALORIMETREDataTable Ara(PRMTARIFEKALORIMETREAra filtre = null)
		{
			return _pRMTARIFEKALORIMETREDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(PRMTARIFEKALORIMETREValidator))]
		private void Validate(PRMTARIFEKALORIMETRE entity)
		{
			if (entity.YEDEKKREDI > 2)
			{
				throw new NotificationException("Yedek kredi 2 den büyük olamaz");
			}
		}

		public void Ekle(List<PRMTARIFEKALORIMETRE> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_pRMTARIFEKALORIMETREDal.Ekle(entityler.ConvertEfList<PRMTARIFEKALORIMETRE, PRMTARIFEKALORIMETREEf>());
		}

		public void Guncelle(List<PRMTARIFEKALORIMETRE> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_pRMTARIFEKALORIMETREDal.Guncelle(entityler.ConvertEfList<PRMTARIFEKALORIMETRE, PRMTARIFEKALORIMETREEf>());
		}

		public void Sil(List<int> idler)
		{
			_pRMTARIFEKALORIMETREDal.Sil(idler);
		}

	}
}