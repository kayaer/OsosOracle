
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.CSTHUMARKAComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface ICSTHUMARKAService
	{
		[OperationContract]
		CSTHUMARKA GetirById(int id);

		[OperationContract]
		CSTHUMARKADetay DetayGetirById(int id);

		[OperationContract]
		List<CSTHUMARKA> Getir(CSTHUMARKAAra filtre = null);

		[OperationContract]
		List<CSTHUMARKADetay> DetayGetir(CSTHUMARKAAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(CSTHUMARKAAra filtre = null);


		[OperationContract]
		CSTHUMARKADataTable Ara(CSTHUMARKAAra filtre = null);

		[OperationContract]
		void Ekle(List<CSTHUMARKA> entityler);

		[OperationContract]
		void Guncelle(List<CSTHUMARKA> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
