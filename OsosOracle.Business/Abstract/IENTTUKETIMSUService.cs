
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.ENTTUKETIMSUComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IENTTUKETIMSUService
	{
		[OperationContract]
		ENTTUKETIMSU GetirById(int id);

		[OperationContract]
		ENTTUKETIMSUDetay DetayGetirById(int id);

		[OperationContract]
		List<ENTTUKETIMSU> Getir(ENTTUKETIMSUAra filtre = null);

		[OperationContract]
		List<ENTTUKETIMSUDetay> DetayGetir(ENTTUKETIMSUAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(ENTTUKETIMSUAra filtre = null);


		[OperationContract]
		ENTTUKETIMSUDataTable Ara(ENTTUKETIMSUAra filtre = null);

		[OperationContract]
		void Ekle(List<ENTTUKETIMSU> entityler);

		[OperationContract]
		void Guncelle(List<ENTTUKETIMSU> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
