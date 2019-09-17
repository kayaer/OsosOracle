
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.SYSMENUComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface ISYSMENUService
	{
		[OperationContract]
		SYSMENU GetirById(int id);

		[OperationContract]
		SYSMENUDetay DetayGetirById(int id);

		[OperationContract]
		List<SYSMENU> Getir(SYSMENUAra filtre = null);

		[OperationContract]
		List<SYSMENUDetay> DetayGetir(SYSMENUAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(SYSMENUAra filtre = null);


		[OperationContract]
		SYSMENUDataTable Ara(SYSMENUAra filtre = null);

		[OperationContract]
		void Ekle(List<SYSMENU> entityler);

		[OperationContract]
		void Guncelle(List<SYSMENU> entityler);

		[OperationContract]
		void Sil(List<int> idler);

        [OperationContract]
        List<SYSMENU> ParentMenuGetir();
    }
}
