using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.CSTSAYACMODELComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface ICSTSAYACMODELDal
	{
		CSTSAYACMODEL Getir(int id);
		CSTSAYACMODELDetay DetayGetir(int id);

		List<CSTSAYACMODEL> Getir(CSTSAYACMODELAra filtre = null);
		List<CSTSAYACMODELDetay> DetayGetir(CSTSAYACMODELAra filtre = null);

		CSTSAYACMODELDataTable Ara(CSTSAYACMODELAra filtre = null);

		int KayitSayisiGetir(CSTSAYACMODELAra filtre = null);

		List<CSTSAYACMODEL> Ekle(List<CSTSAYACMODELEf> entityler);
		void Guncelle(List<CSTSAYACMODELEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}