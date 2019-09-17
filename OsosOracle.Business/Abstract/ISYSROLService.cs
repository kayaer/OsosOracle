
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.SYSROLComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface ISYSROLService
	{
		[OperationContract]
		SYSROL GetirById(int id);

		[OperationContract]
		SYSROLDetay DetayGetirById(int id);

		[OperationContract]
		List<SYSROL> Getir(SYSROLAra filtre = null);

		[OperationContract]
		List<SYSROLDetay> DetayGetir(SYSROLAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(SYSROLAra filtre = null);


		[OperationContract]
		SYSROLDataTable Ara(SYSROLAra filtre = null);

		[OperationContract]
		void Ekle(List<SYSROL> entityler);

		[OperationContract]
		void Guncelle(List<SYSROL> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
