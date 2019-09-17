
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.PRMTARIFESUComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IPRMTARIFESUService
	{
		[OperationContract]
		PRMTARIFESU GetirById(int id);

		[OperationContract]
		PRMTARIFESUDetay DetayGetirById(int id);

		[OperationContract]
		List<PRMTARIFESU> Getir(PRMTARIFESUAra filtre = null);

		[OperationContract]
		List<PRMTARIFESUDetay> DetayGetir(PRMTARIFESUAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(PRMTARIFESUAra filtre = null);


		[OperationContract]
		PRMTARIFESUDataTable Ara(PRMTARIFESUAra filtre = null);

		[OperationContract]
		void Ekle(List<PRMTARIFESU> entityler);

		[OperationContract]
		void Guncelle(List<PRMTARIFESU> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
