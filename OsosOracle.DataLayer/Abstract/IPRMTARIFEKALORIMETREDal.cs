using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.PRMTARIFEKALORIMETREComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IPRMTARIFEKALORIMETREDal
	{
		PRMTARIFEKALORIMETRE Getir(int id);
		PRMTARIFEKALORIMETREDetay DetayGetir(int id);

		List<PRMTARIFEKALORIMETRE> Getir(PRMTARIFEKALORIMETREAra filtre = null);
		List<PRMTARIFEKALORIMETREDetay> DetayGetir(PRMTARIFEKALORIMETREAra filtre = null);

		PRMTARIFEKALORIMETREDataTable Ara(PRMTARIFEKALORIMETREAra filtre = null);

		int KayitSayisiGetir(PRMTARIFEKALORIMETREAra filtre = null);

		List<PRMTARIFEKALORIMETRE> Ekle(List<PRMTARIFEKALORIMETREEf> entityler);
		void Guncelle(List<PRMTARIFEKALORIMETREEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}