
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.SYSROLKULLANICIComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface ISYSROLKULLANICIService
	{
		[OperationContract]
		SYSROLKULLANICI GetirById(int id);

		[OperationContract]
		SYSROLKULLANICIDetay DetayGetirById(int id);

		[OperationContract]
		List<SYSROLKULLANICI> Getir(SYSROLKULLANICIAra filtre = null);

		[OperationContract]
		List<SYSROLKULLANICIDetay> DetayGetir(SYSROLKULLANICIAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(SYSROLKULLANICIAra filtre = null);


		[OperationContract]
		SYSROLKULLANICIDataTable Ara(SYSROLKULLANICIAra filtre = null);

		[OperationContract]
		void Ekle(List<SYSROLKULLANICI> entityler);

		[OperationContract]
		void Guncelle(List<SYSROLKULLANICI> entityler);

		[OperationContract]
		void Sil(List<int> idler);

        [OperationContract]
        bool RolKullaniciSil(int? rolid,int? kullaniciId);
    }
}
