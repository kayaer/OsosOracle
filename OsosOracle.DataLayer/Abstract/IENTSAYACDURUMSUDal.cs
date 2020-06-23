using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTSAYACDURUMSUComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IEntSayacOkumaVeriDal
	{
		EntSayacOkumaVeri Getir(int id);
		EntSayacOkumaVeriDetay DetayGetir(int id);

		List<EntSayacOkumaVeri> Getir(EntSayacOkumaVeriAra filtre = null);
		List<EntSayacOkumaVeriDetay> DetayGetir(EntSayacOkumaVeriAra filtre = null);

		EntSayacOkumaVeriDataTable Ara(EntSayacOkumaVeriAra filtre = null);

		int KayitSayisiGetir(EntSayacOkumaVeriAra filtre = null);

		List<EntSayacOkumaVeri> Ekle(List<EntSayacOkumaVeriEf> entityler);
		void Guncelle(List<EntSayacOkumaVeriEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}