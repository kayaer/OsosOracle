using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTKREDIKOMUTTAKIPComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using System;

namespace OsosOracle.Business.Concrete
{
	public class ENTKREDIKOMUTTAKIPManager : BaseManager, IENTKREDIKOMUTTAKIPService
	{
		private readonly IENTKREDIKOMUTTAKIPDal _eNTKREDIKOMUTTAKIPDal;
		public ENTKREDIKOMUTTAKIPManager(IENTKREDIKOMUTTAKIPDal eNTKREDIKOMUTTAKIPDal)
		{
			_eNTKREDIKOMUTTAKIPDal = eNTKREDIKOMUTTAKIPDal;
		}

		public ENTKREDIKOMUTTAKIP GetirById(int id)
		{
			return _eNTKREDIKOMUTTAKIPDal.Getir(id);
		}

		public ENTKREDIKOMUTTAKIPDetay DetayGetirById(int id)
		{
			return _eNTKREDIKOMUTTAKIPDal.DetayGetir(id);
		}

		public List<ENTKREDIKOMUTTAKIP> Getir(ENTKREDIKOMUTTAKIPAra filtre)
		{
			return _eNTKREDIKOMUTTAKIPDal.Getir(filtre);
		}

		public int KayitSayisiGetir(ENTKREDIKOMUTTAKIPAra filtre)
		{
			return _eNTKREDIKOMUTTAKIPDal.KayitSayisiGetir(filtre);
		}

		public List<ENTKREDIKOMUTTAKIPDetay> DetayGetir(ENTKREDIKOMUTTAKIPAra filtre)
		{
			return _eNTKREDIKOMUTTAKIPDal.DetayGetir(filtre);
		}

		public ENTKREDIKOMUTTAKIPDataTable Ara(ENTKREDIKOMUTTAKIPAra filtre = null)
		{
			return _eNTKREDIKOMUTTAKIPDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(ENTKREDIKOMUTTAKIPValidator))]
		private void Validate(ENTKREDIKOMUTTAKIP entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<ENTKREDIKOMUTTAKIP> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTKREDIKOMUTTAKIPDal.Ekle(entityler.ConvertEfList<ENTKREDIKOMUTTAKIP, ENTKREDIKOMUTTAKIPEf>());
		}

		public void Guncelle(List<ENTKREDIKOMUTTAKIP> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTKREDIKOMUTTAKIPDal.Guncelle(entityler.ConvertEfList<ENTKREDIKOMUTTAKIP, ENTKREDIKOMUTTAKIPEf>());
		}

		public void Sil(List<Guid> idler)
		{
			_eNTKREDIKOMUTTAKIPDal.Sil(idler);
		}

	}
}