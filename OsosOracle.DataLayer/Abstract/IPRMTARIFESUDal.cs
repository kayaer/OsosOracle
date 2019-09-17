using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.PRMTARIFESUComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IPRMTARIFESUDal
	{
		PRMTARIFESU Getir(int id);
		PRMTARIFESUDetay DetayGetir(int id);

		List<PRMTARIFESU> Getir(PRMTARIFESUAra filtre = null);
		List<PRMTARIFESUDetay> DetayGetir(PRMTARIFESUAra filtre = null);

		PRMTARIFESUDataTable Ara(PRMTARIFESUAra filtre = null);

		int KayitSayisiGetir(PRMTARIFESUAra filtre = null);

		List<PRMTARIFESU> Ekle(List<PRMTARIFESUEf> entityler);
		void Guncelle(List<PRMTARIFESUEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}