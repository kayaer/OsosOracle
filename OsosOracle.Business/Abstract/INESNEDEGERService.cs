
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.NESNEDEGERComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface INESNEDEGERService
	{
		[OperationContract]
		NESNEDEGER GetirById(int id);

		[OperationContract]
		NESNEDEGERDetay DetayGetirById(int id);

		[OperationContract]
		List<NESNEDEGER> Getir(NESNEDEGERAra filtre = null);

		[OperationContract]
		List<NESNEDEGERDetay> DetayGetir(NESNEDEGERAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(NESNEDEGERAra filtre = null);


		[OperationContract]
		NESNEDEGERDataTable Ara(NESNEDEGERAra filtre = null);

		[OperationContract]
		void Ekle(List<NESNEDEGER> entityler);

		[OperationContract]
		void Guncelle(List<NESNEDEGER> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
