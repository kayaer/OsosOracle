using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTHABERLESMEUNITESIComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IENTHABERLESMEUNITESIDal
	{
		ENTHABERLESMEUNITESI Getir(int id);
		ENTHABERLESMEUNITESIDetay DetayGetir(int id);

		List<ENTHABERLESMEUNITESI> Getir(ENTHABERLESMEUNITESIAra filtre = null);
		List<ENTHABERLESMEUNITESIDetay> DetayGetir(ENTHABERLESMEUNITESIAra filtre = null);

		ENTHABERLESMEUNITESIDataTable Ara(ENTHABERLESMEUNITESIAra filtre = null);

		int KayitSayisiGetir(ENTHABERLESMEUNITESIAra filtre = null);

		List<ENTHABERLESMEUNITESI> Ekle(List<ENTHABERLESMEUNITESIEf> entityler);
		void Guncelle(List<ENTHABERLESMEUNITESIEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}