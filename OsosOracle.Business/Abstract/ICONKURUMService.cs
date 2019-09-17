
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.CONKURUMComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface ICONKURUMService
	{
		[OperationContract]
		CONKURUM GetirById(int id);

		[OperationContract]
		CONKURUMDetay DetayGetirById(int id);

		[OperationContract]
		List<CONKURUM> Getir(CONKURUMAra filtre = null);

		[OperationContract]
		List<CONKURUMDetay> DetayGetir(CONKURUMAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(CONKURUMAra filtre = null);


		[OperationContract]
		CONKURUMDataTable Ara(CONKURUMAra filtre = null);

		[OperationContract]
		void Ekle(List<CONKURUM> entityler);

		[OperationContract]
		void Guncelle(List<CONKURUM> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
