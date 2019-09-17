using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSROLComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface ISYSROLDal
	{
		SYSROL Getir(int id);
		SYSROLDetay DetayGetir(int id);

		List<SYSROL> Getir(SYSROLAra filtre = null);
		List<SYSROLDetay> DetayGetir(SYSROLAra filtre = null);

		SYSROLDataTable Ara(SYSROLAra filtre = null);

		int KayitSayisiGetir(SYSROLAra filtre = null);

		List<SYSROL> Ekle(List<SYSROLEf> entityler);
		void Guncelle(List<SYSROLEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}