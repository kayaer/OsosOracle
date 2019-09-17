using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.PRMTARIFEELKComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IPRMTARIFEELKDal
	{
		PRMTARIFEELK Getir(int id);
		PRMTARIFEELKDetay DetayGetir(int id);

		List<PRMTARIFEELK> Getir(PRMTARIFEELKAra filtre = null);
		List<PRMTARIFEELKDetay> DetayGetir(PRMTARIFEELKAra filtre = null);

		PRMTARIFEELKDataTable Ara(PRMTARIFEELKAra filtre = null);

		int KayitSayisiGetir(PRMTARIFEELKAra filtre = null);

		List<PRMTARIFEELK> Ekle(List<PRMTARIFEELKEf> entityler);
		void Guncelle(List<PRMTARIFEELKEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}