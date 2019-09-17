using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSCSTOPERASYONComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class SYSCSTOPERASYONManager : BaseManager, ISYSCSTOPERASYONService
	{
		private readonly ISYSCSTOPERASYONDal _sYSCSTOPERASYONDal;
		public SYSCSTOPERASYONManager(ISYSCSTOPERASYONDal sYSCSTOPERASYONDal)
		{
			_sYSCSTOPERASYONDal = sYSCSTOPERASYONDal;
		}

		public SYSCSTOPERASYON GetirById(int id)
		{
			return _sYSCSTOPERASYONDal.Getir(id);
		}

		public SYSCSTOPERASYONDetay DetayGetirById(int id)
		{
			return _sYSCSTOPERASYONDal.DetayGetir(id);
		}

		public List<SYSCSTOPERASYON> Getir(SYSCSTOPERASYONAra filtre)
		{
			return _sYSCSTOPERASYONDal.Getir(filtre);
		}

		public int KayitSayisiGetir(SYSCSTOPERASYONAra filtre)
		{
			return _sYSCSTOPERASYONDal.KayitSayisiGetir(filtre);
		}

		public List<SYSCSTOPERASYONDetay> DetayGetir(SYSCSTOPERASYONAra filtre)
		{
			return _sYSCSTOPERASYONDal.DetayGetir(filtre);
		}

		public SYSCSTOPERASYONDataTable Ara(SYSCSTOPERASYONAra filtre = null)
		{
			return _sYSCSTOPERASYONDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(SYSCSTOPERASYONValidator))]
		private void Validate(SYSCSTOPERASYON entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<SYSCSTOPERASYON> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_sYSCSTOPERASYONDal.Ekle(entityler.ConvertEfList<SYSCSTOPERASYON, SYSCSTOPERASYONEf>());
		}

		public void Guncelle(List<SYSCSTOPERASYON> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_sYSCSTOPERASYONDal.Guncelle(entityler.ConvertEfList<SYSCSTOPERASYON, SYSCSTOPERASYONEf>());
		}

		public void Sil(List<int> idler)
		{
			_sYSCSTOPERASYONDal.Sil(idler);
		}

	}
}