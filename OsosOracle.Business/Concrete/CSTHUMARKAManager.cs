using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.CSTHUMARKAComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class CSTHUMARKAManager : BaseManager, ICSTHUMARKAService
	{
		private readonly ICSTHUMARKADal _cSTHUMARKADal;
		public CSTHUMARKAManager(ICSTHUMARKADal cSTHUMARKADal)
		{
			_cSTHUMARKADal = cSTHUMARKADal;
		}

		public CSTHUMARKA GetirById(int id)
		{
			return _cSTHUMARKADal.Getir(id);
		}

		public CSTHUMARKADetay DetayGetirById(int id)
		{
			return _cSTHUMARKADal.DetayGetir(id);
		}

		public List<CSTHUMARKA> Getir(CSTHUMARKAAra filtre)
		{
			return _cSTHUMARKADal.Getir(filtre);
		}

		public int KayitSayisiGetir(CSTHUMARKAAra filtre)
		{
			return _cSTHUMARKADal.KayitSayisiGetir(filtre);
		}

		public List<CSTHUMARKADetay> DetayGetir(CSTHUMARKAAra filtre)
		{
			return _cSTHUMARKADal.DetayGetir(filtre);
		}

		public CSTHUMARKADataTable Ara(CSTHUMARKAAra filtre = null)
		{
			return _cSTHUMARKADal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(CSTHUMARKAValidator))]
		private void Validate(CSTHUMARKA entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<CSTHUMARKA> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_cSTHUMARKADal.Ekle(entityler.ConvertEfList<CSTHUMARKA, CSTHUMARKAEf>());
		}

		public void Guncelle(List<CSTHUMARKA> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_cSTHUMARKADal.Guncelle(entityler.ConvertEfList<CSTHUMARKA, CSTHUMARKAEf>());
		}

		public void Sil(List<int> idler)
		{
			_cSTHUMARKADal.Sil(idler);
		}

	}
}