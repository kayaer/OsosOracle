
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.ENTABONEBILGIComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IENTABONEBILGIService
	{
		[OperationContract]
		ENTABONEBILGI GetirById(int id);

		[OperationContract]
		ENTABONEBILGIDetay DetayGetirById(int id);

		[OperationContract]
		List<ENTABONEBILGI> Getir(ENTABONEBILGIAra filtre = null);

		[OperationContract]
		List<ENTABONEBILGIDetay> DetayGetir(ENTABONEBILGIAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(ENTABONEBILGIAra filtre = null);


		[OperationContract]
		ENTABONEBILGIDataTable Ara(ENTABONEBILGIAra filtre = null);

		[OperationContract]
		void Ekle(List<ENTABONEBILGI> entityler);

		[OperationContract]
		void Guncelle(List<ENTABONEBILGI> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
