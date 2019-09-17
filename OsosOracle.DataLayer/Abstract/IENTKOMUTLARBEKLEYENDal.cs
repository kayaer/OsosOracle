using System;
using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTKOMUTLARBEKLEYENComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface IENTKOMUTLARBEKLEYENDal
	{
		ENTKOMUTLARBEKLEYEN Getir(int id);
		ENTKOMUTLARBEKLEYENDetay DetayGetir(int id);

		List<ENTKOMUTLARBEKLEYEN> Getir(ENTKOMUTLARBEKLEYENAra filtre = null);
		List<ENTKOMUTLARBEKLEYENDetay> DetayGetir(ENTKOMUTLARBEKLEYENAra filtre = null);

		ENTKOMUTLARBEKLEYENDataTable Ara(ENTKOMUTLARBEKLEYENAra filtre = null);

		int KayitSayisiGetir(ENTKOMUTLARBEKLEYENAra filtre = null);

		List<ENTKOMUTLARBEKLEYEN> Ekle(List<ENTKOMUTLARBEKLEYENEf> entityler);
		void Guncelle(List<ENTKOMUTLARBEKLEYENEf> yeniDegerler);
		void Sil(List<Guid> idler);
	}
}