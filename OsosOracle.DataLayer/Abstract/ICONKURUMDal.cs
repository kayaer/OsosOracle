using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.CONKURUMComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface ICONKURUMDal
	{
		CONKURUM Getir(int id);
		CONKURUMDetay DetayGetir(int id);

		List<CONKURUM> Getir(CONKURUMAra filtre = null);
		List<CONKURUMDetay> DetayGetir(CONKURUMAra filtre = null);

		CONKURUMDataTable Ara(CONKURUMAra filtre = null);

		int KayitSayisiGetir(CONKURUMAra filtre = null);

		List<CONKURUM> Ekle(List<CONKURUMEf> entityler);
		void Guncelle(List<CONKURUMEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}