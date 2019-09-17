using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTSAYACComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IENTSAYACDal
	{
		ENTSAYAC Getir(int id);
		ENTSAYACDetay DetayGetir(int id);

		List<ENTSAYAC> Getir(ENTSAYACAra filtre = null);
		List<ENTSAYACDetay> DetayGetir(ENTSAYACAra filtre = null);

		ENTSAYACDataTable Ara(ENTSAYACAra filtre = null);

		int KayitSayisiGetir(ENTSAYACAra filtre = null);

		List<ENTSAYAC> Ekle(List<ENTSAYACEf> entityler);
		void Guncelle(List<ENTSAYACEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}