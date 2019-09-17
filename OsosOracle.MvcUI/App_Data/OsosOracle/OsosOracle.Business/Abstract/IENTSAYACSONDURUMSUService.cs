
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.ENTSAYACSONDURUMSUComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IENTSAYACSONDURUMSUService
	{
		[OperationContract]
		ENTSAYACSONDURUMSU GetirById(int id);

		[OperationContract]
		ENTSAYACSONDURUMSUDetay DetayGetirById(int id);

		[OperationContract]
		List<ENTSAYACSONDURUMSU> Getir(ENTSAYACSONDURUMSUAra filtre = null);

		[OperationContract]
		List<ENTSAYACSONDURUMSUDetay> DetayGetir(ENTSAYACSONDURUMSUAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(ENTSAYACSONDURUMSUAra filtre = null);


		[OperationContract]
		ENTSAYACSONDURUMSUDataTable Ara(ENTSAYACSONDURUMSUAra filtre = null);

		[OperationContract]
		void Ekle(List<ENTSAYACSONDURUMSU> entityler);

		[OperationContract]
		void Guncelle(List<ENTSAYACSONDURUMSU> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
