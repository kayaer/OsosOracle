using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTSAYACDURUMSUComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class ENTSAYACDURUMSUManager : BaseManager, IENTSAYACDURUMSUService
	{
		private readonly IENTSAYACDURUMSUDal _eNTSAYACDURUMSUDal;
		public ENTSAYACDURUMSUManager(IENTSAYACDURUMSUDal eNTSAYACDURUMSUDal)
		{
			_eNTSAYACDURUMSUDal = eNTSAYACDURUMSUDal;
		}

		public ENTSAYACDURUMSU GetirById(int id)
		{
			return _eNTSAYACDURUMSUDal.Getir(id);
		}

		public ENTSAYACDURUMSUDetay DetayGetirById(int id)
		{
			return _eNTSAYACDURUMSUDal.DetayGetir(id);
		}

		public List<ENTSAYACDURUMSU> Getir(ENTSAYACDURUMSUAra filtre)
		{
			return _eNTSAYACDURUMSUDal.Getir(filtre);
		}

		public int KayitSayisiGetir(ENTSAYACDURUMSUAra filtre)
		{
			return _eNTSAYACDURUMSUDal.KayitSayisiGetir(filtre);
		}

		public List<ENTSAYACDURUMSUDetay> DetayGetir(ENTSAYACDURUMSUAra filtre)
		{
			return _eNTSAYACDURUMSUDal.DetayGetir(filtre);
		}

		public ENTSAYACDURUMSUDataTable Ara(ENTSAYACDURUMSUAra filtre = null)
		{
			return _eNTSAYACDURUMSUDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(ENTSAYACDURUMSUValidator))]
		private void Validate(ENTSAYACDURUMSU entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<ENTSAYACDURUMSU> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTSAYACDURUMSUDal.Ekle(entityler.ConvertEfList<ENTSAYACDURUMSU, ENTSAYACDURUMSUEf>());
		}

		public void Guncelle(List<ENTSAYACDURUMSU> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTSAYACDURUMSUDal.Guncelle(entityler.ConvertEfList<ENTSAYACDURUMSU, ENTSAYACDURUMSUEf>());
		}

		public void Sil(List<int> idler)
		{
			_eNTSAYACDURUMSUDal.Sil(idler);
		}

	}
}