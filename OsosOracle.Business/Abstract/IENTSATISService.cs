
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.ENTSATISComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IENTSATISService
	{
		[OperationContract]
		ENTSATIS GetirById(int id);

		[OperationContract]
		ENTSATISDetay DetayGetirById(int id);

		[OperationContract]
		List<ENTSATIS> Getir(ENTSATISAra filtre = null);

		[OperationContract]
		List<ENTSATISDetay> DetayGetir(ENTSATISAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(ENTSATISAra filtre = null);


		[OperationContract]
		ENTSATISDataTable Ara(ENTSATISAra filtre = null);

		[OperationContract]
		void Ekle(List<ENTSATIS> entityler);

		[OperationContract]
		void Guncelle(List<ENTSATIS> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
