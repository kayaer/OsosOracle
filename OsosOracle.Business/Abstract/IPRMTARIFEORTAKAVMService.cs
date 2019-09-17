
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.PRMTARIFEORTAKAVMComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IPRMTARIFEORTAKAVMService
	{
		[OperationContract]
		PRMTARIFEORTAKAVM GetirById(int id);

		[OperationContract]
		PRMTARIFEORTAKAVMDetay DetayGetirById(int id);

		[OperationContract]
		List<PRMTARIFEORTAKAVM> Getir(PRMTARIFEORTAKAVMAra filtre = null);

		[OperationContract]
		List<PRMTARIFEORTAKAVMDetay> DetayGetir(PRMTARIFEORTAKAVMAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(PRMTARIFEORTAKAVMAra filtre = null);


		[OperationContract]
		PRMTARIFEORTAKAVMDataTable Ara(PRMTARIFEORTAKAVMAra filtre = null);

		[OperationContract]
		void Ekle(List<PRMTARIFEORTAKAVM> entityler);

		[OperationContract]
		void Guncelle(List<PRMTARIFEORTAKAVM> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
