using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.NESNEDEGERComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface INESNEDEGERDal
	{
		NESNEDEGER Getir(int id);
		NESNEDEGERDetay DetayGetir(int id);

		List<NESNEDEGER> Getir(NESNEDEGERAra filtre = null);
		List<NESNEDEGERDetay> DetayGetir(NESNEDEGERAra filtre = null);

		NESNEDEGERDataTable Ara(NESNEDEGERAra filtre = null);

		int KayitSayisiGetir(NESNEDEGERAra filtre = null);

		List<NESNEDEGER> Ekle(List<NESNEDEGEREf> entityler);
		void Guncelle(List<NESNEDEGEREf> yeniDegerler);
		void Sil(List<int> idler);
	}
}