using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTHUSAYACComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class ENTHUSAYACManager : BaseManager, IENTHUSAYACService
	{
		private readonly IENTHUSAYACDal _eNTHUSAYACDal;
		public ENTHUSAYACManager(IENTHUSAYACDal eNTHUSAYACDal)
		{
			_eNTHUSAYACDal = eNTHUSAYACDal;
		}

		public ENTHUSAYAC GetirById(int id)
		{
			return _eNTHUSAYACDal.Getir(id);
		}

		public ENTHUSAYACDetay DetayGetirById(int id)
		{
			return _eNTHUSAYACDal.DetayGetir(id);
		}

		public List<ENTHUSAYAC> Getir(ENTHUSAYACAra filtre)
		{
			return _eNTHUSAYACDal.Getir(filtre);
		}

		public int KayitSayisiGetir(ENTHUSAYACAra filtre)
		{
			return _eNTHUSAYACDal.KayitSayisiGetir(filtre);
		}

		public List<ENTHUSAYACDetay> DetayGetir(ENTHUSAYACAra filtre)
		{
			return _eNTHUSAYACDal.DetayGetir(filtre);
		}

		public ENTHUSAYACDataTable Ara(ENTHUSAYACAra filtre = null)
		{
			return _eNTHUSAYACDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(ENTHUSAYACValidator))]
		private void Validate(ENTHUSAYAC entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<ENTHUSAYAC> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTHUSAYACDal.Ekle(entityler.ConvertEfList<ENTHUSAYAC, ENTHUSAYACEf>());
		}

		public void Guncelle(List<ENTHUSAYAC> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTHUSAYACDal.Guncelle(entityler.ConvertEfList<ENTHUSAYAC, ENTHUSAYACEf>());
		}

		public void Sil(List<int> idler)
		{
			_eNTHUSAYACDal.Sil(idler);
		}

	}
}