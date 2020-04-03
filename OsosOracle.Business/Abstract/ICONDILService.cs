
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.CONDILComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface ICONDILService
	{
		[OperationContract]
		CONDIL GetirById(int id);

		[OperationContract]
		CONDILDetay DetayGetirById(int id);

		[OperationContract]
		List<CONDIL> Getir(CONDILAra filtre = null);

		[OperationContract]
		List<CONDILDetay> DetayGetir(CONDILAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(CONDILAra filtre = null);


		[OperationContract]
		CONDILDataTable Ara(CONDILAra filtre = null);

		[OperationContract]
		void Ekle(List<CONDIL> entityler);

		[OperationContract]
		void Guncelle(List<CONDIL> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
