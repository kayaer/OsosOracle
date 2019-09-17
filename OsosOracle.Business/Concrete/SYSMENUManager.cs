using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSMENUComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class SYSMENUManager : BaseManager, ISYSMENUService
	{
		private readonly ISYSMENUDal _sYSMENUDal;
		public SYSMENUManager(ISYSMENUDal sYSMENUDal)
		{
			_sYSMENUDal = sYSMENUDal;
		}

		public SYSMENU GetirById(int id)
		{
			return _sYSMENUDal.Getir(id);
		}

		public SYSMENUDetay DetayGetirById(int id)
		{
			return _sYSMENUDal.DetayGetir(id);
		}

		public List<SYSMENU> Getir(SYSMENUAra filtre)
		{
			return _sYSMENUDal.Getir(filtre);
		}

		public int KayitSayisiGetir(SYSMENUAra filtre)
		{
			return _sYSMENUDal.KayitSayisiGetir(filtre);
		}

		public List<SYSMENUDetay> DetayGetir(SYSMENUAra filtre)
		{
			return _sYSMENUDal.DetayGetir(filtre);
		}

		public SYSMENUDataTable Ara(SYSMENUAra filtre = null)
		{
			return _sYSMENUDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(SYSMENUValidator))]
		private void Validate(SYSMENU entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<SYSMENU> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_sYSMENUDal.Ekle(entityler.ConvertEfList<SYSMENU, SYSMENUEf>());
		}

		public void Guncelle(List<SYSMENU> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_sYSMENUDal.Guncelle(entityler.ConvertEfList<SYSMENU, SYSMENUEf>());
		}

		public void Sil(List<int> idler)
		{
			_sYSMENUDal.Sil(idler);
		}
        public List<SYSMENU> ParentMenuGetir()
        {
            return _sYSMENUDal.ParentMenuGetir();
        }
    }
}