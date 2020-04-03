
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.PRMTARIFEKALORIMETREComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IPRMTARIFEKALORIMETREService
	{
		[OperationContract]
		PRMTARIFEKALORIMETRE GetirById(int id);

		[OperationContract]
		PRMTARIFEKALORIMETREDetay DetayGetirById(int id);

		[OperationContract]
		List<PRMTARIFEKALORIMETRE> Getir(PRMTARIFEKALORIMETREAra filtre = null);

		[OperationContract]
		List<PRMTARIFEKALORIMETREDetay> DetayGetir(PRMTARIFEKALORIMETREAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(PRMTARIFEKALORIMETREAra filtre = null);


		[OperationContract]
		PRMTARIFEKALORIMETREDataTable Ara(PRMTARIFEKALORIMETREAra filtre = null);

		[OperationContract]
		void Ekle(List<PRMTARIFEKALORIMETRE> entityler);

		[OperationContract]
		void Guncelle(List<PRMTARIFEKALORIMETRE> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
