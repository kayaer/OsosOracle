using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSGOREVROLComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface ISYSGOREVROLDal
	{
		SYSGOREVROL Getir(int id);
		SYSGOREVROLDetay DetayGetir(int id);

		List<SYSGOREVROL> Getir(SYSGOREVROLAra filtre = null);
		List<SYSGOREVROLDetay> DetayGetir(SYSGOREVROLAra filtre = null);

		SYSGOREVROLDataTable Ara(SYSGOREVROLAra filtre = null);

		int KayitSayisiGetir(SYSGOREVROLAra filtre = null);

		List<SYSGOREVROL> Ekle(List<SYSGOREVROLEf> entityler);
		void Guncelle(List<SYSGOREVROLEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}