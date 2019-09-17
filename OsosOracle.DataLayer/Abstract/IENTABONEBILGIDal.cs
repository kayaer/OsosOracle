using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTABONEBILGIComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IENTABONEBILGIDal
	{
		ENTABONEBILGI Getir(int id);
		ENTABONEBILGIDetay DetayGetir(int id);

		List<ENTABONEBILGI> Getir(ENTABONEBILGIAra filtre = null);
		List<ENTABONEBILGIDetay> DetayGetir(ENTABONEBILGIAra filtre = null);

		ENTABONEBILGIDataTable Ara(ENTABONEBILGIAra filtre = null);

		int KayitSayisiGetir(ENTABONEBILGIAra filtre = null);

		List<ENTABONEBILGI> Ekle(List<ENTABONEBILGIEf> entityler);
		void Guncelle(List<ENTABONEBILGIEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}