
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.SYSOPERASYONGOREVComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface ISYSOPERASYONGOREVService
	{
		[OperationContract]
		SYSOPERASYONGOREV GetirById(int id);

		[OperationContract]
		SYSOPERASYONGOREVDetay DetayGetirById(int id);

		[OperationContract]
		List<SYSOPERASYONGOREV> Getir(SYSOPERASYONGOREVAra filtre = null);

		[OperationContract]
		List<SYSOPERASYONGOREVDetay> DetayGetir(SYSOPERASYONGOREVAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(SYSOPERASYONGOREVAra filtre = null);


		[OperationContract]
		SYSOPERASYONGOREVDataTable Ara(SYSOPERASYONGOREVAra filtre = null);

		[OperationContract]
		void Ekle(List<SYSOPERASYONGOREV> entityler);

		[OperationContract]
		void Guncelle(List<SYSOPERASYONGOREV> entityler);

		[OperationContract]
		void Sil(List<int> idler);

        [OperationContract]
        bool OperasyonGorevSil(int gorevid);
    }
}
