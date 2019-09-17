using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTTUKETIMSUComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IENTTUKETIMSUDal
	{
		ENTTUKETIMSU Getir(int id);
		ENTTUKETIMSUDetay DetayGetir(int id);

		List<ENTTUKETIMSU> Getir(ENTTUKETIMSUAra filtre = null);
		List<ENTTUKETIMSUDetay> DetayGetir(ENTTUKETIMSUAra filtre = null);

		ENTTUKETIMSUDataTable Ara(ENTTUKETIMSUAra filtre = null);

		int KayitSayisiGetir(ENTTUKETIMSUAra filtre = null);

		List<ENTTUKETIMSU> Ekle(List<ENTTUKETIMSUEf> entityler);
		void Guncelle(List<ENTTUKETIMSUEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}