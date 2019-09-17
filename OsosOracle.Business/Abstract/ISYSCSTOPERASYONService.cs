
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.SYSCSTOPERASYONComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface ISYSCSTOPERASYONService
	{
		[OperationContract]
		SYSCSTOPERASYON GetirById(int id);

		[OperationContract]
		SYSCSTOPERASYONDetay DetayGetirById(int id);

		[OperationContract]
		List<SYSCSTOPERASYON> Getir(SYSCSTOPERASYONAra filtre = null);

		[OperationContract]
		List<SYSCSTOPERASYONDetay> DetayGetir(SYSCSTOPERASYONAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(SYSCSTOPERASYONAra filtre = null);


		[OperationContract]
		SYSCSTOPERASYONDataTable Ara(SYSCSTOPERASYONAra filtre = null);

		[OperationContract]
		void Ekle(List<SYSCSTOPERASYON> entityler);

		[OperationContract]
		void Guncelle(List<SYSCSTOPERASYON> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
