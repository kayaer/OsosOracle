using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSKULLANICIDETAYComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class SYSKULLANICIDETAYManager : BaseManager, ISYSKULLANICIDETAYService
	{
		private readonly ISYSKULLANICIDETAYDal _sYSKULLANICIDETAYDal;
		public SYSKULLANICIDETAYManager(ISYSKULLANICIDETAYDal sYSKULLANICIDETAYDal)
		{
			_sYSKULLANICIDETAYDal = sYSKULLANICIDETAYDal;
		}

		public SYSKULLANICIDETAY GetirById(int id)
		{
			return _sYSKULLANICIDETAYDal.Getir(id);
		}

		public SYSKULLANICIDETAYDetay DetayGetirById(int id)
		{
			return _sYSKULLANICIDETAYDal.DetayGetir(id);
		}

		public List<SYSKULLANICIDETAY> Getir(SYSKULLANICIDETAYAra filtre)
		{
			return _sYSKULLANICIDETAYDal.Getir(filtre);
		}

		public int KayitSayisiGetir(SYSKULLANICIDETAYAra filtre)
		{
			return _sYSKULLANICIDETAYDal.KayitSayisiGetir(filtre);
		}

		public List<SYSKULLANICIDETAYDetay> DetayGetir(SYSKULLANICIDETAYAra filtre)
		{
			return _sYSKULLANICIDETAYDal.DetayGetir(filtre);
		}

		public SYSKULLANICIDETAYDataTable Ara(SYSKULLANICIDETAYAra filtre = null)
		{
			return _sYSKULLANICIDETAYDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(SYSKULLANICIDETAYValidator))]
		private void Validate(SYSKULLANICIDETAY entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<SYSKULLANICIDETAY> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_sYSKULLANICIDETAYDal.Ekle(entityler.ConvertEfList<SYSKULLANICIDETAY, SYSKULLANICIDETAYEf>());
		}

		public void Guncelle(List<SYSKULLANICIDETAY> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_sYSKULLANICIDETAYDal.Guncelle(entityler.ConvertEfList<SYSKULLANICIDETAY, SYSKULLANICIDETAYEf>());
		}

		public void Sil(List<int> idler)
		{
			_sYSKULLANICIDETAYDal.Sil(idler);
		}

	}
}