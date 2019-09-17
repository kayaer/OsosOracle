
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.PRMTARIFEELKComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IPRMTARIFEELKService
	{
		[OperationContract]
		PRMTARIFEELK GetirById(int id);

		[OperationContract]
		PRMTARIFEELKDetay DetayGetirById(int id);

		[OperationContract]
		List<PRMTARIFEELK> Getir(PRMTARIFEELKAra filtre = null);

		[OperationContract]
		List<PRMTARIFEELKDetay> DetayGetir(PRMTARIFEELKAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(PRMTARIFEELKAra filtre = null);


		[OperationContract]
		PRMTARIFEELKDataTable Ara(PRMTARIFEELKAra filtre = null);

		[OperationContract]
		void Ekle(List<PRMTARIFEELK> entityler);

		[OperationContract]
		void Guncelle(List<PRMTARIFEELK> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
