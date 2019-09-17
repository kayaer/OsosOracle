
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.PRMTARIFEGAZComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IPRMTARIFEGAZService
	{
		[OperationContract]
		PRMTARIFEGAZ GetirById(int id);

		[OperationContract]
		PRMTARIFEGAZDetay DetayGetirById(int id);

		[OperationContract]
		List<PRMTARIFEGAZ> Getir(PRMTARIFEGAZAra filtre = null);

		[OperationContract]
		List<PRMTARIFEGAZDetay> DetayGetir(PRMTARIFEGAZAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(PRMTARIFEGAZAra filtre = null);


		[OperationContract]
		PRMTARIFEGAZDataTable Ara(PRMTARIFEGAZAra filtre = null);

		[OperationContract]
		void Ekle(List<PRMTARIFEGAZ> entityler);

		[OperationContract]
		void Guncelle(List<PRMTARIFEGAZ> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
