
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.ENTSAYACDURUMSUComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IEntSayacOkumaVeriService
	{
		[OperationContract]
		EntSayacOkumaVeri GetirById(int id);

		[OperationContract]
		EntSayacOkumaVeriDetay DetayGetirById(int id);

		[OperationContract]
		List<EntSayacOkumaVeri> Getir(EntSayacOkumaVeriAra filtre = null);

		[OperationContract]
		List<EntSayacOkumaVeriDetay> DetayGetir(EntSayacOkumaVeriAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(EntSayacOkumaVeriAra filtre = null);


		[OperationContract]
		EntSayacOkumaVeriDataTable Ara(EntSayacOkumaVeriAra filtre = null);

		[OperationContract]
		void Ekle(List<EntSayacOkumaVeri> entityler);

		[OperationContract]
		void Guncelle(List<EntSayacOkumaVeri> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
