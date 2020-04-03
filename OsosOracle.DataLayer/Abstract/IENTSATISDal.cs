using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTSATISComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IENTSATISDal
	{
		ENTSATIS Getir(int id);
		ENTSATISDetay DetayGetir(int id);

		List<ENTSATIS> Getir(ENTSATISAra filtre = null);
		List<ENTSATISDetay> DetayGetir(ENTSATISAra filtre = null);

		ENTSATISDataTable Ara(ENTSATISAra filtre = null);

		int KayitSayisiGetir(ENTSATISAra filtre = null);

		List<ENTSATIS> Ekle(List<ENTSATISEf> entityler);
		void Guncelle(List<ENTSATISEf> yeniDegerler);
		void Sil(List<int> idler);
        ENTSATISDataTable SonSatisGetir(ENTSATISAra filtre);
		List<ENTSATIS> SatisGetir(int kurumKayitNo);
	}
}