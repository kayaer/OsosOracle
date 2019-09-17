using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTHUSAYACComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IENTHUSAYACDal
	{
		ENTHUSAYAC Getir(int id);
		ENTHUSAYACDetay DetayGetir(int id);

		List<ENTHUSAYAC> Getir(ENTHUSAYACAra filtre = null);
		List<ENTHUSAYACDetay> DetayGetir(ENTHUSAYACAra filtre = null);

		ENTHUSAYACDataTable Ara(ENTHUSAYACAra filtre = null);

		int KayitSayisiGetir(ENTHUSAYACAra filtre = null);

		List<ENTHUSAYAC> Ekle(List<ENTHUSAYACEf> entityler);
		void Guncelle(List<ENTHUSAYACEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}