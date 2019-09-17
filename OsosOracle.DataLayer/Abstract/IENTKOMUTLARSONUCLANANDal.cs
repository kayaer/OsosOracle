using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTKOMUTLARSONUCLANANComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IENTKOMUTLARSONUCLANANDal
	{
		ENTKOMUTLARSONUCLANAN Getir(int id);
		ENTKOMUTLARSONUCLANANDetay DetayGetir(int id);

		List<ENTKOMUTLARSONUCLANAN> Getir(ENTKOMUTLARSONUCLANANAra filtre = null);
		List<ENTKOMUTLARSONUCLANANDetay> DetayGetir(ENTKOMUTLARSONUCLANANAra filtre = null);

		ENTKOMUTLARSONUCLANANDataTable Ara(ENTKOMUTLARSONUCLANANAra filtre = null);

		int KayitSayisiGetir(ENTKOMUTLARSONUCLANANAra filtre = null);

		List<ENTKOMUTLARSONUCLANAN> Ekle(List<ENTKOMUTLARSONUCLANANEf> entityler);
		void Guncelle(List<ENTKOMUTLARSONUCLANANEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}