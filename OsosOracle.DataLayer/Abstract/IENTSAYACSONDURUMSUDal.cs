using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTSAYACSONDURUMSUComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IENTSAYACSONDURUMSUDal
	{
		ENTSAYACSONDURUMSU Getir(int id);
		ENTSAYACSONDURUMSUDetay DetayGetir(int id);

		List<ENTSAYACSONDURUMSU> Getir(ENTSAYACSONDURUMSUAra filtre = null);
		List<ENTSAYACSONDURUMSUDetay> DetayGetir(ENTSAYACSONDURUMSUAra filtre = null);

		ENTSAYACSONDURUMSUDataTable Ara(ENTSAYACSONDURUMSUAra filtre = null);

		int KayitSayisiGetir(ENTSAYACSONDURUMSUAra filtre = null);

		List<ENTSAYACSONDURUMSU> Ekle(List<ENTSAYACSONDURUMSUEf> entityler);
		void Guncelle(List<ENTSAYACSONDURUMSUEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}