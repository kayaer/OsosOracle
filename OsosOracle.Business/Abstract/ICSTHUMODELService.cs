
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.CSTHUMODELComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface ICSTHUMODELService
	{
		[OperationContract]
		CSTHUMODEL GetirById(int id);

		[OperationContract]
		CSTHUMODELDetay DetayGetirById(int id);

		[OperationContract]
		List<CSTHUMODEL> Getir(CSTHUMODELAra filtre = null);

		[OperationContract]
		List<CSTHUMODELDetay> DetayGetir(CSTHUMODELAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(CSTHUMODELAra filtre = null);


		[OperationContract]
		CSTHUMODELDataTable Ara(CSTHUMODELAra filtre = null);

		[OperationContract]
		void Ekle(List<CSTHUMODEL> entityler);

		[OperationContract]
		void Guncelle(List<CSTHUMODEL> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
