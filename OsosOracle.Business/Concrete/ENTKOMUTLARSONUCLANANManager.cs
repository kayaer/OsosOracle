using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTKOMUTLARSONUCLANANComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class ENTKOMUTLARSONUCLANANManager : BaseManager, IENTKOMUTLARSONUCLANANService
	{
		private readonly IENTKOMUTLARSONUCLANANDal _eNTKOMUTLARSONUCLANANDal;
		public ENTKOMUTLARSONUCLANANManager(IENTKOMUTLARSONUCLANANDal eNTKOMUTLARSONUCLANANDal)
		{
			_eNTKOMUTLARSONUCLANANDal = eNTKOMUTLARSONUCLANANDal;
		}

		public ENTKOMUTLARSONUCLANAN GetirById(int id)
		{
			return _eNTKOMUTLARSONUCLANANDal.Getir(id);
		}

		public ENTKOMUTLARSONUCLANANDetay DetayGetirById(int id)
		{
			return _eNTKOMUTLARSONUCLANANDal.DetayGetir(id);
		}

		public List<ENTKOMUTLARSONUCLANAN> Getir(ENTKOMUTLARSONUCLANANAra filtre)
		{
			return _eNTKOMUTLARSONUCLANANDal.Getir(filtre);
		}

		public int KayitSayisiGetir(ENTKOMUTLARSONUCLANANAra filtre)
		{
			return _eNTKOMUTLARSONUCLANANDal.KayitSayisiGetir(filtre);
		}

		public List<ENTKOMUTLARSONUCLANANDetay> DetayGetir(ENTKOMUTLARSONUCLANANAra filtre)
		{
			return _eNTKOMUTLARSONUCLANANDal.DetayGetir(filtre);
		}

		public ENTKOMUTLARSONUCLANANDataTable Ara(ENTKOMUTLARSONUCLANANAra filtre = null)
		{
			return _eNTKOMUTLARSONUCLANANDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(ENTKOMUTLARSONUCLANANValidator))]
		private void Validate(ENTKOMUTLARSONUCLANAN entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<ENTKOMUTLARSONUCLANAN> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTKOMUTLARSONUCLANANDal.Ekle(entityler.ConvertEfList<ENTKOMUTLARSONUCLANAN, ENTKOMUTLARSONUCLANANEf>());
		}

		public void Guncelle(List<ENTKOMUTLARSONUCLANAN> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTKOMUTLARSONUCLANANDal.Guncelle(entityler.ConvertEfList<ENTKOMUTLARSONUCLANAN, ENTKOMUTLARSONUCLANANEf>());
		}

		public void Sil(List<int> idler)
		{
			_eNTKOMUTLARSONUCLANANDal.Sil(idler);
		}

	}
}