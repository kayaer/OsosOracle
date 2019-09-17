using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTHABERLESMEUNITESIComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class ENTHABERLESMEUNITESIManager : BaseManager, IENTHABERLESMEUNITESIService
	{
		private readonly IENTHABERLESMEUNITESIDal _eNTHABERLESMEUNITESIDal;
		public ENTHABERLESMEUNITESIManager(IENTHABERLESMEUNITESIDal eNTHABERLESMEUNITESIDal)
		{
			_eNTHABERLESMEUNITESIDal = eNTHABERLESMEUNITESIDal;
		}

		public ENTHABERLESMEUNITESI GetirById(int id)
		{
			return _eNTHABERLESMEUNITESIDal.Getir(id);
		}

		public ENTHABERLESMEUNITESIDetay DetayGetirById(int id)
		{
			return _eNTHABERLESMEUNITESIDal.DetayGetir(id);
		}

		public List<ENTHABERLESMEUNITESI> Getir(ENTHABERLESMEUNITESIAra filtre)
		{
			return _eNTHABERLESMEUNITESIDal.Getir(filtre);
		}

		public int KayitSayisiGetir(ENTHABERLESMEUNITESIAra filtre)
		{
			return _eNTHABERLESMEUNITESIDal.KayitSayisiGetir(filtre);
		}

		public List<ENTHABERLESMEUNITESIDetay> DetayGetir(ENTHABERLESMEUNITESIAra filtre)
		{
			return _eNTHABERLESMEUNITESIDal.DetayGetir(filtre);
		}

		public ENTHABERLESMEUNITESIDataTable Ara(ENTHABERLESMEUNITESIAra filtre = null)
		{
			return _eNTHABERLESMEUNITESIDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(ENTHABERLESMEUNITESIValidator))]
		private void Validate(ENTHABERLESMEUNITESI entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<ENTHABERLESMEUNITESI> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTHABERLESMEUNITESIDal.Ekle(entityler.ConvertEfList<ENTHABERLESMEUNITESI, ENTHABERLESMEUNITESIEf>());
		}

		public void Guncelle(List<ENTHABERLESMEUNITESI> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTHABERLESMEUNITESIDal.Guncelle(entityler.ConvertEfList<ENTHABERLESMEUNITESI, ENTHABERLESMEUNITESIEf>());
		}

		public void Sil(List<int> idler)
		{
			_eNTHABERLESMEUNITESIDal.Sil(idler);
		}

	}
}