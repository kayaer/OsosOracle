
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.NESNETIPComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface INESNETIPService
	{
		[OperationContract]
		NESNETIP GetirById(int id);

		[OperationContract]
		NESNETIPDetay DetayGetirById(int id);

		[OperationContract]
		List<NESNETIP> Getir(NESNETIPAra filtre = null);

		[OperationContract]
		List<NESNETIPDetay> DetayGetir(NESNETIPAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(NESNETIPAra filtre = null);


		[OperationContract]
		NESNETIPDataTable Ara(NESNETIPAra filtre = null);

		[OperationContract]
		void Ekle(List<NESNETIP> entityler);

		[OperationContract]
		void Guncelle(List<NESNETIP> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
