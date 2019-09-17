using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTSAYACDURUMSUComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IENTSAYACDURUMSUDal
	{
		ENTSAYACDURUMSU Getir(int id);
		ENTSAYACDURUMSUDetay DetayGetir(int id);

		List<ENTSAYACDURUMSU> Getir(ENTSAYACDURUMSUAra filtre = null);
		List<ENTSAYACDURUMSUDetay> DetayGetir(ENTSAYACDURUMSUAra filtre = null);

		ENTSAYACDURUMSUDataTable Ara(ENTSAYACDURUMSUAra filtre = null);

		int KayitSayisiGetir(ENTSAYACDURUMSUAra filtre = null);

		List<ENTSAYACDURUMSU> Ekle(List<ENTSAYACDURUMSUEf> entityler);
		void Guncelle(List<ENTSAYACDURUMSUEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}