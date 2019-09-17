using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTKOMUTLARBEKLEYENComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using System;

namespace OsosOracle.Business.Concrete
{
	public class ENTKOMUTLARBEKLEYENManager : BaseManager, IENTKOMUTLARBEKLEYENService
	{
		private readonly IENTKOMUTLARBEKLEYENDal _eNTKOMUTLARBEKLEYENDal;
		public ENTKOMUTLARBEKLEYENManager(IENTKOMUTLARBEKLEYENDal eNTKOMUTLARBEKLEYENDal)
		{
			_eNTKOMUTLARBEKLEYENDal = eNTKOMUTLARBEKLEYENDal;
		}

		public ENTKOMUTLARBEKLEYEN GetirById(int id)
		{
			return _eNTKOMUTLARBEKLEYENDal.Getir(id);
		}

		public ENTKOMUTLARBEKLEYENDetay DetayGetirById(int id)
		{
			return _eNTKOMUTLARBEKLEYENDal.DetayGetir(id);
		}

		public List<ENTKOMUTLARBEKLEYEN> Getir(ENTKOMUTLARBEKLEYENAra filtre)
		{
			return _eNTKOMUTLARBEKLEYENDal.Getir(filtre);
		}

		public int KayitSayisiGetir(ENTKOMUTLARBEKLEYENAra filtre)
		{
			return _eNTKOMUTLARBEKLEYENDal.KayitSayisiGetir(filtre);
		}

		public List<ENTKOMUTLARBEKLEYENDetay> DetayGetir(ENTKOMUTLARBEKLEYENAra filtre)
		{
			return _eNTKOMUTLARBEKLEYENDal.DetayGetir(filtre);
		}

		public ENTKOMUTLARBEKLEYENDataTable Ara(ENTKOMUTLARBEKLEYENAra filtre = null)
		{
			return _eNTKOMUTLARBEKLEYENDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(ENTKOMUTLARBEKLEYENValidator))]
		private void Validate(ENTKOMUTLARBEKLEYEN entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<ENTKOMUTLARBEKLEYEN> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTKOMUTLARBEKLEYENDal.Ekle(entityler.ConvertEfList<ENTKOMUTLARBEKLEYEN, ENTKOMUTLARBEKLEYENEf>());
		}

		public void Guncelle(List<ENTKOMUTLARBEKLEYEN> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTKOMUTLARBEKLEYENDal.Guncelle(entityler.ConvertEfList<ENTKOMUTLARBEKLEYEN, ENTKOMUTLARBEKLEYENEf>());
		}

		public void Sil(List<Guid> idler)
		{
			_eNTKOMUTLARBEKLEYENDal.Sil(idler);
		}

	}
}