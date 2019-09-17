
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.ENTSAYACDURUMSUComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IENTSAYACDURUMSUService
	{
		[OperationContract]
		ENTSAYACDURUMSU GetirById(int id);

		[OperationContract]
		ENTSAYACDURUMSUDetay DetayGetirById(int id);

		[OperationContract]
		List<ENTSAYACDURUMSU> Getir(ENTSAYACDURUMSUAra filtre = null);

		[OperationContract]
		List<ENTSAYACDURUMSUDetay> DetayGetir(ENTSAYACDURUMSUAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(ENTSAYACDURUMSUAra filtre = null);


		[OperationContract]
		ENTSAYACDURUMSUDataTable Ara(ENTSAYACDURUMSUAra filtre = null);

		[OperationContract]
		void Ekle(List<ENTSAYACDURUMSU> entityler);

		[OperationContract]
		void Guncelle(List<ENTSAYACDURUMSU> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
