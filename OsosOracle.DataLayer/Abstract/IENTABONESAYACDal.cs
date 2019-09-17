using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IENTABONESAYACDal
	{
		ENTABONESAYAC Getir(int id);
		ENTABONESAYACDetay DetayGetir(int id);

		List<ENTABONESAYAC> Getir(ENTABONESAYACAra filtre = null);
		List<ENTABONESAYACDetay> DetayGetir(ENTABONESAYACAra filtre = null);

		ENTABONESAYACDataTable Ara(ENTABONESAYACAra filtre = null);

		int KayitSayisiGetir(ENTABONESAYACAra filtre = null);

		List<ENTABONESAYAC> Ekle(List<ENTABONESAYACEf> entityler);
		void Guncelle(List<ENTABONESAYACEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}