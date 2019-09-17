
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.ENTKOMUTLARSONUCLANANComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IENTKOMUTLARSONUCLANANService
	{
		[OperationContract]
		ENTKOMUTLARSONUCLANAN GetirById(int id);

		[OperationContract]
		ENTKOMUTLARSONUCLANANDetay DetayGetirById(int id);

		[OperationContract]
		List<ENTKOMUTLARSONUCLANAN> Getir(ENTKOMUTLARSONUCLANANAra filtre = null);

		[OperationContract]
		List<ENTKOMUTLARSONUCLANANDetay> DetayGetir(ENTKOMUTLARSONUCLANANAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(ENTKOMUTLARSONUCLANANAra filtre = null);


		[OperationContract]
		ENTKOMUTLARSONUCLANANDataTable Ara(ENTKOMUTLARSONUCLANANAra filtre = null);

		[OperationContract]
		void Ekle(List<ENTKOMUTLARSONUCLANAN> entityler);

		[OperationContract]
		void Guncelle(List<ENTKOMUTLARSONUCLANAN> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
