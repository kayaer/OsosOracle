using System;
using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTKREDIKOMUTTAKIPComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IENTKREDIKOMUTTAKIPDal
	{
		ENTKREDIKOMUTTAKIP Getir(int id);
		ENTKREDIKOMUTTAKIPDetay DetayGetir(int id);

		List<ENTKREDIKOMUTTAKIP> Getir(ENTKREDIKOMUTTAKIPAra filtre = null);
		List<ENTKREDIKOMUTTAKIPDetay> DetayGetir(ENTKREDIKOMUTTAKIPAra filtre = null);

		ENTKREDIKOMUTTAKIPDataTable Ara(ENTKREDIKOMUTTAKIPAra filtre = null);

		int KayitSayisiGetir(ENTKREDIKOMUTTAKIPAra filtre = null);

		List<ENTKREDIKOMUTTAKIP> Ekle(List<ENTKREDIKOMUTTAKIPEf> entityler);
		void Guncelle(List<ENTKREDIKOMUTTAKIPEf> yeniDegerler);
		void Sil(List<Guid> idler);
	}
}