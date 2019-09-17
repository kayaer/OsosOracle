using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTSAYACComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class ENTSAYACManager : BaseManager, IENTSAYACService
	{
		private readonly IENTSAYACDal _eNTSAYACDal;
		public ENTSAYACManager(IENTSAYACDal eNTSAYACDal)
		{
			_eNTSAYACDal = eNTSAYACDal;
		}

		public ENTSAYAC GetirById(int id)
		{
			return _eNTSAYACDal.Getir(id);
		}

		public ENTSAYACDetay DetayGetirById(int id)
		{
			return _eNTSAYACDal.DetayGetir(id);
		}

		public List<ENTSAYAC> Getir(ENTSAYACAra filtre)
		{
			return _eNTSAYACDal.Getir(filtre);
		}

		public int KayitSayisiGetir(ENTSAYACAra filtre)
		{
			return _eNTSAYACDal.KayitSayisiGetir(filtre);
		}

		public List<ENTSAYACDetay> DetayGetir(ENTSAYACAra filtre)
		{
			return _eNTSAYACDal.DetayGetir(filtre);
		}

		public ENTSAYACDataTable Ara(ENTSAYACAra filtre = null)
		{
			return _eNTSAYACDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(ENTSAYACValidator))]
		private void Validate(ENTSAYAC entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<ENTSAYAC> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTSAYACDal.Ekle(entityler.ConvertEfList<ENTSAYAC, ENTSAYACEf>());
		}

		public void Guncelle(List<ENTSAYAC> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTSAYACDal.Guncelle(entityler.ConvertEfList<ENTSAYAC, ENTSAYACEf>());
		}

		public void Sil(List<int> idler)
		{
			_eNTSAYACDal.Sil(idler);
		}

	}
}