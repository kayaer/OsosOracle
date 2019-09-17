
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.RPTDASHBOARDComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IRPTDASHBOARDService
	{
		[OperationContract]
		RPTDASHBOARD GetirById(int id);

		[OperationContract]
		RPTDASHBOARDDetay DetayGetirById(int id);

		[OperationContract]
		List<RPTDASHBOARD> Getir(RPTDASHBOARDAra filtre = null);

		[OperationContract]
		List<RPTDASHBOARDDetay> DetayGetir(RPTDASHBOARDAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(RPTDASHBOARDAra filtre = null);


		[OperationContract]
		RPTDASHBOARDDataTable Ara(RPTDASHBOARDAra filtre = null);

		[OperationContract]
		void Ekle(List<RPTDASHBOARD> entityler);

		[OperationContract]
		void Guncelle(List<RPTDASHBOARD> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
