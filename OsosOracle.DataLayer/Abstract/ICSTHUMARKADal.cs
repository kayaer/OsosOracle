using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.CSTHUMARKAComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface ICSTHUMARKADal
	{
		CSTHUMARKA Getir(int id);
		CSTHUMARKADetay DetayGetir(int id);

		List<CSTHUMARKA> Getir(CSTHUMARKAAra filtre = null);
		List<CSTHUMARKADetay> DetayGetir(CSTHUMARKAAra filtre = null);

		CSTHUMARKADataTable Ara(CSTHUMARKAAra filtre = null);

		int KayitSayisiGetir(CSTHUMARKAAra filtre = null);

		List<CSTHUMARKA> Ekle(List<CSTHUMARKAEf> entityler);
		void Guncelle(List<CSTHUMARKAEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}