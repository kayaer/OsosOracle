
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.ENTHABERLESMEUNITESIComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IENTHABERLESMEUNITESIService
	{
		[OperationContract]
		ENTHABERLESMEUNITESI GetirById(int id);

		[OperationContract]
		ENTHABERLESMEUNITESIDetay DetayGetirById(int id);

		[OperationContract]
		List<ENTHABERLESMEUNITESI> Getir(ENTHABERLESMEUNITESIAra filtre = null);

		[OperationContract]
		List<ENTHABERLESMEUNITESIDetay> DetayGetir(ENTHABERLESMEUNITESIAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(ENTHABERLESMEUNITESIAra filtre = null);


		[OperationContract]
		ENTHABERLESMEUNITESIDataTable Ara(ENTHABERLESMEUNITESIAra filtre = null);

		[OperationContract]
		void Ekle(List<ENTHABERLESMEUNITESI> entityler);

		[OperationContract]
		void Guncelle(List<ENTHABERLESMEUNITESI> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
