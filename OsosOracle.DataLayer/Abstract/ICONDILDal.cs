using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.CONDILComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface ICONDILDal
	{
		CONDIL Getir(int id);
		CONDILDetay DetayGetir(int id);

		List<CONDIL> Getir(CONDILAra filtre = null);
		List<CONDILDetay> DetayGetir(CONDILAra filtre = null);

		CONDILDataTable Ara(CONDILAra filtre = null);

		int KayitSayisiGetir(CONDILAra filtre = null);

		List<CONDIL> Ekle(List<CONDILEf> entityler);
		void Guncelle(List<CONDILEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}