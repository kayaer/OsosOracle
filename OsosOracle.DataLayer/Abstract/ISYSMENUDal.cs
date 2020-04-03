using System.Collections.Generic;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSMENUComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Abstract
{
	public interface ISYSMENUDal
	{
		SYSMENU Getir(int id);
		SYSMENUDetay DetayGetir(int id);

		List<SYSMENU> Getir(SYSMENUAra filtre = null);
		List<SYSMENUDetay> DetayGetir(SYSMENUAra filtre = null);

		SYSMENUDataTable Ara(SYSMENUAra filtre = null);

		int KayitSayisiGetir(SYSMENUAra filtre = null);

		List<SYSMENU> Ekle(List<SYSMENUEf> entityler);
		void Guncelle(List<SYSMENUEf> yeniDegerler);
		void Sil(List<int> idler);
        List<SYSMENU> ParentMenuGetir();
        List<SYSMENU> YetkiGetir(int kullaniciKayitNo);
    }
}