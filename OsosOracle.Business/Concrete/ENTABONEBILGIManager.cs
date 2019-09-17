using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTABONEBILGIComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class ENTABONEBILGIManager : BaseManager, IENTABONEBILGIService
	{
		private readonly IENTABONEBILGIDal _eNTABONEBILGIDal;
		public ENTABONEBILGIManager(IENTABONEBILGIDal eNTABONEBILGIDal)
		{
			_eNTABONEBILGIDal = eNTABONEBILGIDal;
		}

		public ENTABONEBILGI GetirById(int id)
		{
			return _eNTABONEBILGIDal.Getir(id);
		}

		public ENTABONEBILGIDetay DetayGetirById(int id)
		{
			return _eNTABONEBILGIDal.DetayGetir(id);
		}

		public List<ENTABONEBILGI> Getir(ENTABONEBILGIAra filtre)
		{
			return _eNTABONEBILGIDal.Getir(filtre);
		}

		public int KayitSayisiGetir(ENTABONEBILGIAra filtre)
		{
			return _eNTABONEBILGIDal.KayitSayisiGetir(filtre);
		}

		public List<ENTABONEBILGIDetay> DetayGetir(ENTABONEBILGIAra filtre)
		{
			return _eNTABONEBILGIDal.DetayGetir(filtre);
		}

		public ENTABONEBILGIDataTable Ara(ENTABONEBILGIAra filtre = null)
		{
			return _eNTABONEBILGIDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(ENTABONEBILGIValidator))]
		private void Validate(ENTABONEBILGI entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<ENTABONEBILGI> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTABONEBILGIDal.Ekle(entityler.ConvertEfList<ENTABONEBILGI, ENTABONEBILGIEf>());
		}

		public void Guncelle(List<ENTABONEBILGI> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTABONEBILGIDal.Guncelle(entityler.ConvertEfList<ENTABONEBILGI, ENTABONEBILGIEf>());
		}

		public void Sil(List<int> idler)
		{
			_eNTABONEBILGIDal.Sil(idler);
		}

	}
}