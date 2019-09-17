using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTSAYACSONDURUMSUComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class ENTSAYACSONDURUMSUManager : BaseManager, IENTSAYACSONDURUMSUService
	{
		private readonly IENTSAYACSONDURUMSUDal _eNTSAYACSONDURUMSUDal;
		public ENTSAYACSONDURUMSUManager(IENTSAYACSONDURUMSUDal eNTSAYACSONDURUMSUDal)
		{
			_eNTSAYACSONDURUMSUDal = eNTSAYACSONDURUMSUDal;
		}

		public ENTSAYACSONDURUMSU GetirById(int id)
		{
			return _eNTSAYACSONDURUMSUDal.Getir(id);
		}

		public ENTSAYACSONDURUMSUDetay DetayGetirById(int id)
		{
			return _eNTSAYACSONDURUMSUDal.DetayGetir(id);
		}

		public List<ENTSAYACSONDURUMSU> Getir(ENTSAYACSONDURUMSUAra filtre)
		{
			return _eNTSAYACSONDURUMSUDal.Getir(filtre);
		}

		public int KayitSayisiGetir(ENTSAYACSONDURUMSUAra filtre)
		{
			return _eNTSAYACSONDURUMSUDal.KayitSayisiGetir(filtre);
		}

		public List<ENTSAYACSONDURUMSUDetay> DetayGetir(ENTSAYACSONDURUMSUAra filtre)
		{
			return _eNTSAYACSONDURUMSUDal.DetayGetir(filtre);
		}

		public ENTSAYACSONDURUMSUDataTable Ara(ENTSAYACSONDURUMSUAra filtre = null)
		{
			return _eNTSAYACSONDURUMSUDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(ENTSAYACSONDURUMSUValidator))]
		private void Validate(ENTSAYACSONDURUMSU entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<ENTSAYACSONDURUMSU> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTSAYACSONDURUMSUDal.Ekle(entityler.ConvertEfList<ENTSAYACSONDURUMSU, ENTSAYACSONDURUMSUEf>());
		}

		public void Guncelle(List<ENTSAYACSONDURUMSU> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTSAYACSONDURUMSUDal.Guncelle(entityler.ConvertEfList<ENTSAYACSONDURUMSU, ENTSAYACSONDURUMSUEf>());
		}

		public void Sil(List<int> idler)
		{
			_eNTSAYACSONDURUMSUDal.Sil(idler);
		}

	}
}