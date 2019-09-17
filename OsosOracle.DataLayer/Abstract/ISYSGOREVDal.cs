using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSGOREVComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface ISYSGOREVDal
	{
		SYSGOREV Getir(int id);
		SYSGOREVDetay DetayGetir(int id);

		List<SYSGOREV> Getir(SYSGOREVAra filtre = null);
		List<SYSGOREVDetay> DetayGetir(SYSGOREVAra filtre = null);

		SYSGOREVDataTable Ara(SYSGOREVAra filtre = null);

		int KayitSayisiGetir(SYSGOREVAra filtre = null);

		List<SYSGOREV> Ekle(List<SYSGOREVEf> entityler);
		void Guncelle(List<SYSGOREVEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}