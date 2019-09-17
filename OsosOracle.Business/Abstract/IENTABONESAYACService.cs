
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IENTABONESAYACService
	{
		[OperationContract]
		ENTABONESAYAC GetirById(int id);

		[OperationContract]
		ENTABONESAYACDetay DetayGetirById(int id);

		[OperationContract]
		List<ENTABONESAYAC> Getir(ENTABONESAYACAra filtre = null);

		[OperationContract]
		List<ENTABONESAYACDetay> DetayGetir(ENTABONESAYACAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(ENTABONESAYACAra filtre = null);


		[OperationContract]
		ENTABONESAYACDataTable Ara(ENTABONESAYACAra filtre = null);

		[OperationContract]
		void Ekle(List<ENTABONESAYAC> entityler);

		[OperationContract]
		void Guncelle(List<ENTABONESAYAC> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
