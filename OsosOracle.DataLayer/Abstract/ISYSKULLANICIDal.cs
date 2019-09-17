using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSKULLANICIComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface ISYSKULLANICIDal
	{
		SYSKULLANICI Getir(int id);
		SYSKULLANICIDetay DetayGetir(int id);

		List<SYSKULLANICI> Getir(SYSKULLANICIAra filtre = null);
		List<SYSKULLANICIDetay> DetayGetir(SYSKULLANICIAra filtre = null);

		SYSKULLANICIDataTable Ara(SYSKULLANICIAra filtre = null);

		int KayitSayisiGetir(SYSKULLANICIAra filtre = null);

		List<SYSKULLANICI> Ekle(List<SYSKULLANICIEf> entityler);
		void Guncelle(List<SYSKULLANICIEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}