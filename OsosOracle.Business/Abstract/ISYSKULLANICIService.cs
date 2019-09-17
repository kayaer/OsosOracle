
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.SYSKULLANICIComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface ISYSKULLANICIService
	{
		[OperationContract]
		SYSKULLANICI GetirById(int id);

		[OperationContract]
		SYSKULLANICIDetay DetayGetirById(int id);

		[OperationContract]
		List<SYSKULLANICI> Getir(SYSKULLANICIAra filtre = null);

		[OperationContract]
		List<SYSKULLANICIDetay> DetayGetir(SYSKULLANICIAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(SYSKULLANICIAra filtre = null);


		[OperationContract]
		SYSKULLANICIDataTable Ara(SYSKULLANICIAra filtre = null);

		[OperationContract]
		void Ekle(List<SYSKULLANICI> entityler);

		[OperationContract]
		void Guncelle(List<SYSKULLANICI> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
