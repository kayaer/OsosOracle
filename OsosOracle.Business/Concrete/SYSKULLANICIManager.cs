using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSKULLANICIComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class SYSKULLANICIManager : BaseManager, ISYSKULLANICIService
	{
		private readonly ISYSKULLANICIDal _sYSKULLANICIDal;
		public SYSKULLANICIManager(ISYSKULLANICIDal sYSKULLANICIDal)
		{
			_sYSKULLANICIDal = sYSKULLANICIDal;
		}

		public SYSKULLANICI GetirById(int id)
		{
			return _sYSKULLANICIDal.Getir(id);
		}

		public SYSKULLANICIDetay DetayGetirById(int id)
		{
			return _sYSKULLANICIDal.DetayGetir(id);
		}

		public List<SYSKULLANICI> Getir(SYSKULLANICIAra filtre)
		{
			return _sYSKULLANICIDal.Getir(filtre);
		}

		public int KayitSayisiGetir(SYSKULLANICIAra filtre)
		{
			return _sYSKULLANICIDal.KayitSayisiGetir(filtre);
		}

		public List<SYSKULLANICIDetay> DetayGetir(SYSKULLANICIAra filtre)
		{
			return _sYSKULLANICIDal.DetayGetir(filtre);
		}

		public SYSKULLANICIDataTable Ara(SYSKULLANICIAra filtre = null)
		{
			return _sYSKULLANICIDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(SYSKULLANICIValidator))]
		private void Validate(SYSKULLANICI entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<SYSKULLANICI> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_sYSKULLANICIDal.Ekle(entityler.ConvertEfList<SYSKULLANICI, SYSKULLANICIEf>());
		}

		public void Guncelle(List<SYSKULLANICI> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_sYSKULLANICIDal.Guncelle(entityler.ConvertEfList<SYSKULLANICI, SYSKULLANICIEf>());
		}

		public void Sil(List<int> idler)
		{
			_sYSKULLANICIDal.Sil(idler);
		}

	}
}