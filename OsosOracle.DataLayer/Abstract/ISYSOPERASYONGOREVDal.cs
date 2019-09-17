using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSOPERASYONGOREVComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface ISYSOPERASYONGOREVDal
	{
		SYSOPERASYONGOREV Getir(int id);
		SYSOPERASYONGOREVDetay DetayGetir(int id);

		List<SYSOPERASYONGOREV> Getir(SYSOPERASYONGOREVAra filtre = null);
		List<SYSOPERASYONGOREVDetay> DetayGetir(SYSOPERASYONGOREVAra filtre = null);

		SYSOPERASYONGOREVDataTable Ara(SYSOPERASYONGOREVAra filtre = null);

		int KayitSayisiGetir(SYSOPERASYONGOREVAra filtre = null);

		List<SYSOPERASYONGOREV> Ekle(List<SYSOPERASYONGOREVEf> entityler);
		void Guncelle(List<SYSOPERASYONGOREVEf> yeniDegerler);
		void Sil(List<int> idler);

        bool OperasyonGorevSil(int gorevid);
    }
}