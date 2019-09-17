
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.ENTHUSAYACComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IENTHUSAYACService
	{
		[OperationContract]
		ENTHUSAYAC GetirById(int id);

		[OperationContract]
		ENTHUSAYACDetay DetayGetirById(int id);

		[OperationContract]
		List<ENTHUSAYAC> Getir(ENTHUSAYACAra filtre = null);

		[OperationContract]
		List<ENTHUSAYACDetay> DetayGetir(ENTHUSAYACAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(ENTHUSAYACAra filtre = null);


		[OperationContract]
		ENTHUSAYACDataTable Ara(ENTHUSAYACAra filtre = null);

		[OperationContract]
		void Ekle(List<ENTHUSAYAC> entityler);

		[OperationContract]
		void Guncelle(List<ENTHUSAYAC> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
