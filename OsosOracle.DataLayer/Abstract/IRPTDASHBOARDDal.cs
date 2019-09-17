using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.RPTDASHBOARDComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IRPTDASHBOARDDal
	{
		RPTDASHBOARD Getir(int id);
		RPTDASHBOARDDetay DetayGetir(int id);

		List<RPTDASHBOARD> Getir(RPTDASHBOARDAra filtre = null);
		List<RPTDASHBOARDDetay> DetayGetir(RPTDASHBOARDAra filtre = null);

		RPTDASHBOARDDataTable Ara(RPTDASHBOARDAra filtre = null);

		int KayitSayisiGetir(RPTDASHBOARDAra filtre = null);

		List<RPTDASHBOARD> Ekle(List<RPTDASHBOARDEf> entityler);
		void Guncelle(List<RPTDASHBOARDEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}