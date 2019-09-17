using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class ENTABONESAYACManager : BaseManager, IENTABONESAYACService
	{
		private readonly IENTABONESAYACDal _eNTABONESAYACDal;
		public ENTABONESAYACManager(IENTABONESAYACDal eNTABONESAYACDal)
		{
			_eNTABONESAYACDal = eNTABONESAYACDal;
		}

		public ENTABONESAYAC GetirById(int id)
		{
			return _eNTABONESAYACDal.Getir(id);
		}

		public ENTABONESAYACDetay DetayGetirById(int id)
		{
			return _eNTABONESAYACDal.DetayGetir(id);
		}

		public List<ENTABONESAYAC> Getir(ENTABONESAYACAra filtre)
		{
			return _eNTABONESAYACDal.Getir(filtre);
		}

		public int KayitSayisiGetir(ENTABONESAYACAra filtre)
		{
			return _eNTABONESAYACDal.KayitSayisiGetir(filtre);
		}

		public List<ENTABONESAYACDetay> DetayGetir(ENTABONESAYACAra filtre)
		{
			return _eNTABONESAYACDal.DetayGetir(filtre);
		}

		public ENTABONESAYACDataTable Ara(ENTABONESAYACAra filtre = null)
		{
			return _eNTABONESAYACDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(ENTABONESAYACValidator))]
		private void Validate(ENTABONESAYAC entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<ENTABONESAYAC> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTABONESAYACDal.Ekle(entityler.ConvertEfList<ENTABONESAYAC, ENTABONESAYACEf>());
		}

		public void Guncelle(List<ENTABONESAYAC> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTABONESAYACDal.Guncelle(entityler.ConvertEfList<ENTABONESAYAC, ENTABONESAYACEf>());
		}

		public void Sil(List<int> idler)
		{
			_eNTABONESAYACDal.Sil(idler);
		}

	}
}