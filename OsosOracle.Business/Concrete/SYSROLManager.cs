using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSROLComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class SYSROLManager : BaseManager, ISYSROLService
	{
		private readonly ISYSROLDal _sYSROLDal;
		public SYSROLManager(ISYSROLDal sYSROLDal)
		{
			_sYSROLDal = sYSROLDal;
		}

		public SYSROL GetirById(int id)
		{
			return _sYSROLDal.Getir(id);
		}

		public SYSROLDetay DetayGetirById(int id)
		{
			return _sYSROLDal.DetayGetir(id);
		}

		public List<SYSROL> Getir(SYSROLAra filtre)
		{
			return _sYSROLDal.Getir(filtre);
		}

		public int KayitSayisiGetir(SYSROLAra filtre)
		{
			return _sYSROLDal.KayitSayisiGetir(filtre);
		}

		public List<SYSROLDetay> DetayGetir(SYSROLAra filtre)
		{
			return _sYSROLDal.DetayGetir(filtre);
		}

		public SYSROLDataTable Ara(SYSROLAra filtre = null)
		{
			return _sYSROLDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(SYSROLValidator))]
		private void Validate(SYSROL entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<SYSROL> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_sYSROLDal.Ekle(entityler.ConvertEfList<SYSROL, SYSROLEf>());
		}

		public void Guncelle(List<SYSROL> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_sYSROLDal.Guncelle(entityler.ConvertEfList<SYSROL, SYSROLEf>());
		}

		public void Sil(List<int> idler)
		{
			_sYSROLDal.Sil(idler);
		}

	}
}