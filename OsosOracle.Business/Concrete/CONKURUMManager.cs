using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.CONKURUMComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class CONKURUMManager : BaseManager, ICONKURUMService
	{
		private readonly ICONKURUMDal _cONKURUMDal;
		public CONKURUMManager(ICONKURUMDal cONKURUMDal)
		{
			_cONKURUMDal = cONKURUMDal;
		}

		public CONKURUM GetirById(int id)
		{
			return _cONKURUMDal.Getir(id);
		}

		public CONKURUMDetay DetayGetirById(int id)
		{
			return _cONKURUMDal.DetayGetir(id);
		}

		public List<CONKURUM> Getir(CONKURUMAra filtre)
		{
			return _cONKURUMDal.Getir(filtre);
		}

		public int KayitSayisiGetir(CONKURUMAra filtre)
		{
			return _cONKURUMDal.KayitSayisiGetir(filtre);
		}

		public List<CONKURUMDetay> DetayGetir(CONKURUMAra filtre)
		{
			return _cONKURUMDal.DetayGetir(filtre);
		}

		public CONKURUMDataTable Ara(CONKURUMAra filtre = null)
		{
			return _cONKURUMDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(CONKURUMValidator))]
		private void Validate(CONKURUM entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<CONKURUM> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_cONKURUMDal.Ekle(entityler.ConvertEfList<CONKURUM, CONKURUMEf>());
		}

		public void Guncelle(List<CONKURUM> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_cONKURUMDal.Guncelle(entityler.ConvertEfList<CONKURUM, CONKURUMEf>());
		}

		public void Sil(List<int> idler)
		{
			_cONKURUMDal.Sil(idler);
		}

	}
}