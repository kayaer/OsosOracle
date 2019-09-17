using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.PRMTARIFEGAZComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IPRMTARIFEGAZDal
	{
		PRMTARIFEGAZ Getir(int id);
		PRMTARIFEGAZDetay DetayGetir(int id);

		List<PRMTARIFEGAZ> Getir(PRMTARIFEGAZAra filtre = null);
		List<PRMTARIFEGAZDetay> DetayGetir(PRMTARIFEGAZAra filtre = null);

		PRMTARIFEGAZDataTable Ara(PRMTARIFEGAZAra filtre = null);

		int KayitSayisiGetir(PRMTARIFEGAZAra filtre = null);

		List<PRMTARIFEGAZ> Ekle(List<PRMTARIFEGAZEf> entityler);
		void Guncelle(List<PRMTARIFEGAZEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}