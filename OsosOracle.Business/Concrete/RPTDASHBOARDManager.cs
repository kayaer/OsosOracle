using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.RPTDASHBOARDComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;

namespace OsosOracle.Business.Concrete
{
	public class RPTDASHBOARDManager : BaseManager, IRPTDASHBOARDService
	{
		private readonly IRPTDASHBOARDDal _rPTDASHBOARDDal;
		public RPTDASHBOARDManager(IRPTDASHBOARDDal rPTDASHBOARDDal)
		{
			_rPTDASHBOARDDal = rPTDASHBOARDDal;
		}

		public RPTDASHBOARD GetirById(int id)
		{
			return _rPTDASHBOARDDal.Getir(id);
		}

		public RPTDASHBOARDDetay DetayGetirById(int id)
		{
			return _rPTDASHBOARDDal.DetayGetir(id);
		}

		public List<RPTDASHBOARD> Getir(RPTDASHBOARDAra filtre)
		{
			return _rPTDASHBOARDDal.Getir(filtre);
		}

		public int KayitSayisiGetir(RPTDASHBOARDAra filtre)
		{
			return _rPTDASHBOARDDal.KayitSayisiGetir(filtre);
		}

		public List<RPTDASHBOARDDetay> DetayGetir(RPTDASHBOARDAra filtre)
		{
			return _rPTDASHBOARDDal.DetayGetir(filtre);
		}

		public RPTDASHBOARDDataTable Ara(RPTDASHBOARDAra filtre = null)
		{
			return _rPTDASHBOARDDal.Ara(filtre);
		}

		
		[FluentValidationAspect(typeof(RPTDASHBOARDValidator))]
		private void Validate(RPTDASHBOARD entity)
		{
			//Kontroller Yapılacak
		}

		public void Ekle(List<RPTDASHBOARD> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_rPTDASHBOARDDal.Ekle(entityler.ConvertEfList<RPTDASHBOARD, RPTDASHBOARDEf>());
		}

		public void Guncelle(List<RPTDASHBOARD> entityler)
		{
			foreach (var entity in entityler)
			{
				Validate(entity);
			}
			_rPTDASHBOARDDal.Guncelle(entityler.ConvertEfList<RPTDASHBOARD, RPTDASHBOARDEf>());
		}

		public void Sil(List<int> idler)
		{
			_rPTDASHBOARDDal.Sil(idler);
		}

	}
}