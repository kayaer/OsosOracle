using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTSATISComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class ENTSATISManager : BaseManager, IENTSATISService
	{
		private readonly IENTSATISDal _eNTSATISDal;
		public ENTSATISManager(IENTSATISDal eNTSATISDal)
		{
			_eNTSATISDal = eNTSATISDal;
		}

		public ENTSATIS GetirById(int id)
		{
			return _eNTSATISDal.Getir(id);
		}

		public ENTSATISDetay DetayGetirById(int id)
		{
			return _eNTSATISDal.DetayGetir(id);
		}

		public List<ENTSATIS> Getir(ENTSATISAra filtre)
		{
			return _eNTSATISDal.Getir(filtre);
		}

		public int KayitSayisiGetir(ENTSATISAra filtre)
		{
			return _eNTSATISDal.KayitSayisiGetir(filtre);
		}

		public List<ENTSATISDetay> DetayGetir(ENTSATISAra filtre)
		{
			return _eNTSATISDal.DetayGetir(filtre);
		}

		public ENTSATISDataTable Ara(ENTSATISAra filtre = null)
		{
			return _eNTSATISDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(ENTSATISValidator))]
		private void Validate(ENTSATIS entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<ENTSATIS> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTSATISDal.Ekle(entityler.ConvertEfList<ENTSATIS, ENTSATISEf>());
		}

		public void Guncelle(List<ENTSATIS> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTSATISDal.Guncelle(entityler.ConvertEfList<ENTSATIS, ENTSATISEf>());
		}

		public void Sil(List<int> idler)
		{
			_eNTSATISDal.Sil(idler);
		}

	}
}