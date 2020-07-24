using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.EntIsEmriComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using System;

namespace OsosOracle.Business.Concrete
{
	public class EntIsEmriManager : BaseManager, IEntIsEmriService
	{
		private readonly IEntIsEmriDal _eNTKOMUTLARBEKLEYENDal;
		public EntIsEmriManager(IEntIsEmriDal eNTKOMUTLARBEKLEYENDal)
		{
			_eNTKOMUTLARBEKLEYENDal = eNTKOMUTLARBEKLEYENDal;
		}

		public EntIsEmri GetirById(int id)
		{
			return _eNTKOMUTLARBEKLEYENDal.Getir(id);
		}

		public EntIsEmriDetay DetayGetirById(int id)
		{
			return _eNTKOMUTLARBEKLEYENDal.DetayGetir(id);
		}

		public List<EntIsEmri> Getir(EntIsEmriAra filtre)
		{
			return _eNTKOMUTLARBEKLEYENDal.Getir(filtre);
		}

		public int KayitSayisiGetir(EntIsEmriAra filtre)
		{
			return _eNTKOMUTLARBEKLEYENDal.KayitSayisiGetir(filtre);
		}

		public List<EntIsEmriDetay> DetayGetir(EntIsEmriAra filtre)
		{
			return _eNTKOMUTLARBEKLEYENDal.DetayGetir(filtre);
		}

		public EntIsEmriDataTable Ara(EntIsEmriAra filtre = null)
		{
			return _eNTKOMUTLARBEKLEYENDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(EntIsEmriValidator))]
		private void Validate(EntIsEmri entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<EntIsEmri> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTKOMUTLARBEKLEYENDal.Ekle(entityler.ConvertEfList<EntIsEmri, EntIsEmriEf>());
		}

		public void Guncelle(List<EntIsEmri> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTKOMUTLARBEKLEYENDal.Guncelle(entityler.ConvertEfList<EntIsEmri, EntIsEmriEf>());
		}

		public void Sil(List<int> idler)
		{
			_eNTKOMUTLARBEKLEYENDal.Sil(idler);
		}

	}
}