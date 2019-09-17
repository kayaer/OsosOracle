using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSCSTOPERASYONComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface ISYSCSTOPERASYONDal
	{
		SYSCSTOPERASYON Getir(int id);
		SYSCSTOPERASYONDetay DetayGetir(int id);

		List<SYSCSTOPERASYON> Getir(SYSCSTOPERASYONAra filtre = null);
		List<SYSCSTOPERASYONDetay> DetayGetir(SYSCSTOPERASYONAra filtre = null);

		SYSCSTOPERASYONDataTable Ara(SYSCSTOPERASYONAra filtre = null);

		int KayitSayisiGetir(SYSCSTOPERASYONAra filtre = null);

		List<SYSCSTOPERASYON> Ekle(List<SYSCSTOPERASYONEf> entityler);
		void Guncelle(List<SYSCSTOPERASYONEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}