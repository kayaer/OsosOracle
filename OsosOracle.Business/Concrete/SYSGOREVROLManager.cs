using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSGOREVROLComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class SYSGOREVROLManager : BaseManager, ISYSGOREVROLService
	{
		private readonly ISYSGOREVROLDal _sYSGOREVROLDal;
		public SYSGOREVROLManager(ISYSGOREVROLDal sYSGOREVROLDal)
		{
			_sYSGOREVROLDal = sYSGOREVROLDal;
		}

		public SYSGOREVROL GetirById(int id)
		{
			return _sYSGOREVROLDal.Getir(id);
		}

		public SYSGOREVROLDetay DetayGetirById(int id)
		{
			return _sYSGOREVROLDal.DetayGetir(id);
		}

		public List<SYSGOREVROL> Getir(SYSGOREVROLAra filtre)
		{
			return _sYSGOREVROLDal.Getir(filtre);
		}

		public int KayitSayisiGetir(SYSGOREVROLAra filtre)
		{
			return _sYSGOREVROLDal.KayitSayisiGetir(filtre);
		}

		public List<SYSGOREVROLDetay> DetayGetir(SYSGOREVROLAra filtre)
		{
			return _sYSGOREVROLDal.DetayGetir(filtre);
		}

		public SYSGOREVROLDataTable Ara(SYSGOREVROLAra filtre = null)
		{
			return _sYSGOREVROLDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(SYSGOREVROLValidator))]
		private void Validate(SYSGOREVROL entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<SYSGOREVROL> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_sYSGOREVROLDal.Ekle(entityler.ConvertEfList<SYSGOREVROL, SYSGOREVROLEf>());
		}

		public void Guncelle(List<SYSGOREVROL> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_sYSGOREVROLDal.Guncelle(entityler.ConvertEfList<SYSGOREVROL, SYSGOREVROLEf>());
		}

		public void Sil(List<int> idler)
		{
			_sYSGOREVROLDal.Sil(idler);
		}

	}
}