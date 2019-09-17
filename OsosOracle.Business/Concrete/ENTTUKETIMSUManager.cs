using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTTUKETIMSUComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class ENTTUKETIMSUManager : BaseManager, IENTTUKETIMSUService
	{
		private readonly IENTTUKETIMSUDal _eNTTUKETIMSUDal;
		public ENTTUKETIMSUManager(IENTTUKETIMSUDal eNTTUKETIMSUDal)
		{
			_eNTTUKETIMSUDal = eNTTUKETIMSUDal;
		}

		public ENTTUKETIMSU GetirById(int id)
		{
			return _eNTTUKETIMSUDal.Getir(id);
		}

		public ENTTUKETIMSUDetay DetayGetirById(int id)
		{
			return _eNTTUKETIMSUDal.DetayGetir(id);
		}

		public List<ENTTUKETIMSU> Getir(ENTTUKETIMSUAra filtre)
		{
			return _eNTTUKETIMSUDal.Getir(filtre);
		}

		public int KayitSayisiGetir(ENTTUKETIMSUAra filtre)
		{
			return _eNTTUKETIMSUDal.KayitSayisiGetir(filtre);
		}

		public List<ENTTUKETIMSUDetay> DetayGetir(ENTTUKETIMSUAra filtre)
		{
			return _eNTTUKETIMSUDal.DetayGetir(filtre);
		}

		public ENTTUKETIMSUDataTable Ara(ENTTUKETIMSUAra filtre = null)
		{
			return _eNTTUKETIMSUDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(ENTTUKETIMSUValidator))]
		private void Validate(ENTTUKETIMSU entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<ENTTUKETIMSU> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTTUKETIMSUDal.Ekle(entityler.ConvertEfList<ENTTUKETIMSU, ENTTUKETIMSUEf>());
		}

		public void Guncelle(List<ENTTUKETIMSU> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTTUKETIMSUDal.Guncelle(entityler.ConvertEfList<ENTTUKETIMSU, ENTTUKETIMSUEf>());
		}

		public void Sil(List<int> idler)
		{
			_eNTTUKETIMSUDal.Sil(idler);
		}

	}
}