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
	public class EntSayacOkumaVeriManager : BaseManager, IEntSayacOkumaVeriService
	{
		private readonly IEntSayacOkumaVeriDal _eNTSAYACDURUMSUDal;
		public EntSayacOkumaVeriManager(IEntSayacOkumaVeriDal eNTSAYACDURUMSUDal)
		{
			_eNTSAYACDURUMSUDal = eNTSAYACDURUMSUDal;
		}

		public EntSayacOkumaVeri GetirById(int id)
		{
			return _eNTSAYACDURUMSUDal.Getir(id);
		}

		public EntSayacOkumaVeriDetay DetayGetirById(int id)
		{
			return _eNTSAYACDURUMSUDal.DetayGetir(id);
		}

		public List<EntSayacOkumaVeri> Getir(EntSayacOkumaVeriAra filtre)
		{
			return _eNTSAYACDURUMSUDal.Getir(filtre);
		}

		public int KayitSayisiGetir(EntSayacOkumaVeriAra filtre)
		{
			return _eNTSAYACDURUMSUDal.KayitSayisiGetir(filtre);
		}

		public List<EntSayacOkumaVeriDetay> DetayGetir(EntSayacOkumaVeriAra filtre)
		{
			return _eNTSAYACDURUMSUDal.DetayGetir(filtre);
		}

		public EntSayacOkumaVeriDataTable Ara(EntSayacOkumaVeriAra filtre = null)
		{
			return _eNTSAYACDURUMSUDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(ENTSAYACDURUMSUValidator))]
		private void Validate(EntSayacOkumaVeri entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<EntSayacOkumaVeri> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTSAYACDURUMSUDal.Ekle(entityler.ConvertEfList<EntSayacOkumaVeri, EntSayacOkumaVeriEf>());
		}

		public void Guncelle(List<EntSayacOkumaVeri> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_eNTSAYACDURUMSUDal.Guncelle(entityler.ConvertEfList<EntSayacOkumaVeri, EntSayacOkumaVeriEf>());
		}

		public void Sil(List<int> idler)
		{
			_eNTSAYACDURUMSUDal.Sil(idler);
		}

	}
}