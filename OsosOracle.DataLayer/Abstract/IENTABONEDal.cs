using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTABONEComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IENTABONEDal
	{
		ENTABONE Getir(int id);
		ENTABONEDetay DetayGetir(int id);

		List<ENTABONE> Getir(ENTABONEAra filtre = null);
		List<ENTABONEDetay> DetayGetir(ENTABONEAra filtre = null);

		ENTABONEDataTable Ara(ENTABONEAra filtre = null);

		int KayitSayisiGetir(ENTABONEAra filtre = null);

		List<ENTABONE> Ekle(List<ENTABONEEf> entityler);
		void Guncelle(List<ENTABONEEf> yeniDegerler);
		void Sil(List<int> idler);

        List<AboneAutoComplete> AutoCompleteBilgileriGetir(ENTABONEAra filtre = null);
		
		AboneGenel AboneGenelBilgileriGetir(int aboneKayitNo);
	}
}