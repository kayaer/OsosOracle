using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSOPERASYONGOREVComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class SYSOPERASYONGOREVManager : BaseManager, ISYSOPERASYONGOREVService
	{
		private readonly ISYSOPERASYONGOREVDal _sYSOPERASYONGOREVDal;
		public SYSOPERASYONGOREVManager(ISYSOPERASYONGOREVDal sYSOPERASYONGOREVDal)
		{
			_sYSOPERASYONGOREVDal = sYSOPERASYONGOREVDal;
		}

		public SYSOPERASYONGOREV GetirById(int id)
		{
			return _sYSOPERASYONGOREVDal.Getir(id);
		}

		public SYSOPERASYONGOREVDetay DetayGetirById(int id)
		{
			return _sYSOPERASYONGOREVDal.DetayGetir(id);
		}

		public List<SYSOPERASYONGOREV> Getir(SYSOPERASYONGOREVAra filtre)
		{
			return _sYSOPERASYONGOREVDal.Getir(filtre);
		}

		public int KayitSayisiGetir(SYSOPERASYONGOREVAra filtre)
		{
			return _sYSOPERASYONGOREVDal.KayitSayisiGetir(filtre);
		}

		public List<SYSOPERASYONGOREVDetay> DetayGetir(SYSOPERASYONGOREVAra filtre)
		{
			return _sYSOPERASYONGOREVDal.DetayGetir(filtre);
		}

		public SYSOPERASYONGOREVDataTable Ara(SYSOPERASYONGOREVAra filtre = null)
		{
			return _sYSOPERASYONGOREVDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(SYSOPERASYONGOREVValidator))]
		private void Validate(SYSOPERASYONGOREV entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<SYSOPERASYONGOREV> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_sYSOPERASYONGOREVDal.Ekle(entityler.ConvertEfList<SYSOPERASYONGOREV, SYSOPERASYONGOREVEf>());
		}

		public void Guncelle(List<SYSOPERASYONGOREV> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_sYSOPERASYONGOREVDal.Guncelle(entityler.ConvertEfList<SYSOPERASYONGOREV, SYSOPERASYONGOREVEf>());
		}

		public void Sil(List<int> idler)
		{
			_sYSOPERASYONGOREVDal.Sil(idler);
		}
        public bool OperasyonGorevSil(int gorevid)
        {
            return _sYSOPERASYONGOREVDal.OperasyonGorevSil(gorevid);
        }
    }
}