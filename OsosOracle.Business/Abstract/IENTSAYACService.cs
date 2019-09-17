
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.ENTSAYACComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IENTSAYACService
	{
		[OperationContract]
		ENTSAYAC GetirById(int id);

		[OperationContract]
		ENTSAYACDetay DetayGetirById(int id);

		[OperationContract]
		List<ENTSAYAC> Getir(ENTSAYACAra filtre = null);

		[OperationContract]
		List<ENTSAYACDetay> DetayGetir(ENTSAYACAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(ENTSAYACAra filtre = null);


		[OperationContract]
		ENTSAYACDataTable Ara(ENTSAYACAra filtre = null);

		[OperationContract]
		void Ekle(List<ENTSAYAC> entityler);

		[OperationContract]
		void Guncelle(List<ENTSAYAC> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
