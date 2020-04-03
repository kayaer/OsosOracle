using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.NESNETIPComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface INESNETIPDal
	{
		NESNETIP Getir(int id);
		NESNETIPDetay DetayGetir(int id);

		List<NESNETIP> Getir(NESNETIPAra filtre = null);
		List<NESNETIPDetay> DetayGetir(NESNETIPAra filtre = null);

		NESNETIPDataTable Ara(NESNETIPAra filtre = null);

		int KayitSayisiGetir(NESNETIPAra filtre = null);

		List<NESNETIP> Ekle(List<NESNETIPEf> entityler);
		void Guncelle(List<NESNETIPEf> yeniDegerler);
		void Sil(List<int> idler);
	}
}