using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.CSTHUMODELComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface ICSTHUMODELDal
	{
		CSTHUMODEL Getir(int id);
		CSTHUMODELDetay DetayGetir(int id);

		List<CSTHUMODEL> Getir(CSTHUMODELAra filtre = null);
		List<CSTHUMODELDetay> DetayGetir(CSTHUMODELAra filtre = null);

		CSTHUMODELDataTable Ara(CSTHUMODELAra filtre = null);

		int KayitSayisiGetir(CSTHUMODELAra filtre = null);

		List<CSTHUMODEL> Ekle(List<CSTHUMODELEf> entityler);
		void Guncelle(List<CSTHUMODELEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}