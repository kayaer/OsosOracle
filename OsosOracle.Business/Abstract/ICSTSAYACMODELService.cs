
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.CSTSAYACMODELComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface ICSTSAYACMODELService
	{
		[OperationContract]
		CSTSAYACMODEL GetirById(int id);

		[OperationContract]
		CSTSAYACMODELDetay DetayGetirById(int id);

		[OperationContract]
		List<CSTSAYACMODEL> Getir(CSTSAYACMODELAra filtre = null);

		[OperationContract]
		List<CSTSAYACMODELDetay> DetayGetir(CSTSAYACMODELAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(CSTSAYACMODELAra filtre = null);


		[OperationContract]
		CSTSAYACMODELDataTable Ara(CSTSAYACMODELAra filtre = null);

		[OperationContract]
		void Ekle(List<CSTSAYACMODEL> entityler);

		[OperationContract]
		void Guncelle(List<CSTSAYACMODEL> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
