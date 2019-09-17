
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.SYSKULLANICIDETAYComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface ISYSKULLANICIDETAYService
	{
		[OperationContract]
		SYSKULLANICIDETAY GetirById(int id);

		[OperationContract]
		SYSKULLANICIDETAYDetay DetayGetirById(int id);

		[OperationContract]
		List<SYSKULLANICIDETAY> Getir(SYSKULLANICIDETAYAra filtre = null);

		[OperationContract]
		List<SYSKULLANICIDETAYDetay> DetayGetir(SYSKULLANICIDETAYAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(SYSKULLANICIDETAYAra filtre = null);


		[OperationContract]
		SYSKULLANICIDETAYDataTable Ara(SYSKULLANICIDETAYAra filtre = null);

		[OperationContract]
		void Ekle(List<SYSKULLANICIDETAY> entityler);

		[OperationContract]
		void Guncelle(List<SYSKULLANICIDETAY> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
