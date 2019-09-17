
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.SYSGOREVComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface ISYSGOREVService
	{
		[OperationContract]
		SYSGOREV GetirById(int id);

		[OperationContract]
		SYSGOREVDetay DetayGetirById(int id);

		[OperationContract]
		List<SYSGOREV> Getir(SYSGOREVAra filtre = null);

		[OperationContract]
		List<SYSGOREVDetay> DetayGetir(SYSGOREVAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(SYSGOREVAra filtre = null);


		[OperationContract]
		SYSGOREVDataTable Ara(SYSGOREVAra filtre = null);

		[OperationContract]
		void Ekle(List<SYSGOREV> entityler);

		[OperationContract]
		void Guncelle(List<SYSGOREV> entityler);

		[OperationContract]
		void Sil(List<int> idler);

        [OperationContract]
        bool RolGorevSil(int rolid, int gorevid);
    }
}
