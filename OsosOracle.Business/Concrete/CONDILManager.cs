using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.CONDILComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class CONDILManager : BaseManager, ICONDILService
	{
		private readonly ICONDILDal _cONDILDal;
		public CONDILManager(ICONDILDal cONDILDal)
		{
			_cONDILDal = cONDILDal;
		}

		public CONDIL GetirById(int id)
		{
			return _cONDILDal.Getir(id);
		}

		public CONDILDetay DetayGetirById(int id)
		{
			return _cONDILDal.DetayGetir(id);
		}

		public List<CONDIL> Getir(CONDILAra filtre)
		{
			return _cONDILDal.Getir(filtre);
		}

		public int KayitSayisiGetir(CONDILAra filtre)
		{
			return _cONDILDal.KayitSayisiGetir(filtre);
		}

		public List<CONDILDetay> DetayGetir(CONDILAra filtre)
		{
			return _cONDILDal.DetayGetir(filtre);
		}

		public CONDILDataTable Ara(CONDILAra filtre = null)
		{
			return _cONDILDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(CONDILValidator))]
		private void Validate(CONDIL entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<CONDIL> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_cONDILDal.Ekle(entityler.ConvertEfList<CONDIL, CONDILEf>());
		}

		public void Guncelle(List<CONDIL> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_cONDILDal.Guncelle(entityler.ConvertEfList<CONDIL, CONDILEf>());
		}

		public void Sil(List<int> idler)
		{
			_cONDILDal.Sil(idler);
		}

	}
}