using System;
using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.EntIsEmriComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IEntIsEmriDal
	{
		EntIsEmri Getir(int id);
		EntIsEmriDetay DetayGetir(int id);

		List<EntIsEmri> Getir(EntIsEmriAra filtre = null);
		List<EntIsEmriDetay> DetayGetir(EntIsEmriAra filtre = null);

		EntIsEmriDataTable Ara(EntIsEmriAra filtre = null);

		int KayitSayisiGetir(EntIsEmriAra filtre = null);

		List<EntIsEmri> Ekle(List<EntIsEmriEf> entityler);
		void Guncelle(List<EntIsEmriEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}