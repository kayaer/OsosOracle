using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSKULLANICIDETAYComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface ISYSKULLANICIDETAYDal
	{
		SYSKULLANICIDETAY Getir(int id);
		SYSKULLANICIDETAYDetay DetayGetir(int id);

		List<SYSKULLANICIDETAY> Getir(SYSKULLANICIDETAYAra filtre = null);
		List<SYSKULLANICIDETAYDetay> DetayGetir(SYSKULLANICIDETAYAra filtre = null);

		SYSKULLANICIDETAYDataTable Ara(SYSKULLANICIDETAYAra filtre = null);

		int KayitSayisiGetir(SYSKULLANICIDETAYAra filtre = null);

		List<SYSKULLANICIDETAY> Ekle(List<SYSKULLANICIDETAYEf> entityler);
		void Guncelle(List<SYSKULLANICIDETAYEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}