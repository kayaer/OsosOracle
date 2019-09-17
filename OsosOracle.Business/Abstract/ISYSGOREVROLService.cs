
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.SYSGOREVROLComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface ISYSGOREVROLService
	{
		[OperationContract]
		SYSGOREVROL GetirById(int id);

		[OperationContract]
		SYSGOREVROLDetay DetayGetirById(int id);

		[OperationContract]
		List<SYSGOREVROL> Getir(SYSGOREVROLAra filtre = null);

		[OperationContract]
		List<SYSGOREVROLDetay> DetayGetir(SYSGOREVROLAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(SYSGOREVROLAra filtre = null);


		[OperationContract]
		SYSGOREVROLDataTable Ara(SYSGOREVROLAra filtre = null);

		[OperationContract]
		void Ekle(List<SYSGOREVROL> entityler);

		[OperationContract]
		void Guncelle(List<SYSGOREVROL> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
