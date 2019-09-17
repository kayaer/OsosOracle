using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSROLKULLANICIComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface ISYSROLKULLANICIDal
	{
		SYSROLKULLANICI Getir(int id);
		SYSROLKULLANICIDetay DetayGetir(int id);

		List<SYSROLKULLANICI> Getir(SYSROLKULLANICIAra filtre = null);
		List<SYSROLKULLANICIDetay> DetayGetir(SYSROLKULLANICIAra filtre = null);

		SYSROLKULLANICIDataTable Ara(SYSROLKULLANICIAra filtre = null);

		int KayitSayisiGetir(SYSROLKULLANICIAra filtre = null);

		List<SYSROLKULLANICI> Ekle(List<SYSROLKULLANICIEf> entityler);
		void Guncelle(List<SYSROLKULLANICIEf> yeniDegerler);
		void Sil(List<int> idler);

        bool RolKullaniciSil(int? rolid, int? kullaniciId);
    }
}