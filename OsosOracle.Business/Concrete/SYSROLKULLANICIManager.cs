using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSROLKULLANICIComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class SYSROLKULLANICIManager : BaseManager, ISYSROLKULLANICIService
	{
		private readonly ISYSROLKULLANICIDal _sYSROLKULLANICIDal;
		public SYSROLKULLANICIManager(ISYSROLKULLANICIDal sYSROLKULLANICIDal)
		{
			_sYSROLKULLANICIDal = sYSROLKULLANICIDal;
		}

		public SYSROLKULLANICI GetirById(int id)
		{
			return _sYSROLKULLANICIDal.Getir(id);
		}

		public SYSROLKULLANICIDetay DetayGetirById(int id)
		{
			return _sYSROLKULLANICIDal.DetayGetir(id);
		}

		public List<SYSROLKULLANICI> Getir(SYSROLKULLANICIAra filtre)
		{
			return _sYSROLKULLANICIDal.Getir(filtre);
		}

		public int KayitSayisiGetir(SYSROLKULLANICIAra filtre)
		{
			return _sYSROLKULLANICIDal.KayitSayisiGetir(filtre);
		}

		public List<SYSROLKULLANICIDetay> DetayGetir(SYSROLKULLANICIAra filtre)
		{
			return _sYSROLKULLANICIDal.DetayGetir(filtre);
		}

		public SYSROLKULLANICIDataTable Ara(SYSROLKULLANICIAra filtre = null)
		{
			return _sYSROLKULLANICIDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(SYSROLKULLANICIValidator))]
		private void Validate(SYSROLKULLANICI entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<SYSROLKULLANICI> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_sYSROLKULLANICIDal.Ekle(entityler.ConvertEfList<SYSROLKULLANICI, SYSROLKULLANICIEf>());
		}

		public void Guncelle(List<SYSROLKULLANICI> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_sYSROLKULLANICIDal.Guncelle(entityler.ConvertEfList<SYSROLKULLANICI, SYSROLKULLANICIEf>());
		}

		public void Sil(List<int> idler)
		{
			_sYSROLKULLANICIDal.Sil(idler);
		}

        public bool RolKullaniciSil(int? rolid, int? kullaniciId)
        {
            return _sYSROLKULLANICIDal.RolKullaniciSil(rolid,kullaniciId);
        }
    }
}