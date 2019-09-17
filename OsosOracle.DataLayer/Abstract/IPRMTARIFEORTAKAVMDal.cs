using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.PRMTARIFEORTAKAVMComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IPRMTARIFEORTAKAVMDal
	{
		PRMTARIFEORTAKAVM Getir(int id);
		PRMTARIFEORTAKAVMDetay DetayGetir(int id);

		List<PRMTARIFEORTAKAVM> Getir(PRMTARIFEORTAKAVMAra filtre = null);
		List<PRMTARIFEORTAKAVMDetay> DetayGetir(PRMTARIFEORTAKAVMAra filtre = null);

		PRMTARIFEORTAKAVMDataTable Ara(PRMTARIFEORTAKAVMAra filtre = null);

		int KayitSayisiGetir(PRMTARIFEORTAKAVMAra filtre = null);

		List<PRMTARIFEORTAKAVM> Ekle(List<PRMTARIFEORTAKAVMEf> entityler);
		void Guncelle(List<PRMTARIFEORTAKAVMEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}