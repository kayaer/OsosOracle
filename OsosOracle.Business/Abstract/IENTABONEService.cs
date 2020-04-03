
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.ENTABONEComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IENTABONEService
	{
		[OperationContract]
		ENTABONE GetirById(int id);

		[OperationContract]
		ENTABONEDetay DetayGetirById(int id);

		[OperationContract]
		List<ENTABONE> Getir(ENTABONEAra filtre = null);

		[OperationContract]
		List<ENTABONEDetay> DetayGetir(ENTABONEAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(ENTABONEAra filtre = null);


		[OperationContract]
		ENTABONEDataTable Ara(ENTABONEAra filtre = null);

		[OperationContract]
        List<ENTABONE> Ekle(List<ENTABONE> entityler);

		[OperationContract]
		void Guncelle(List<ENTABONE> entityler);

		[OperationContract]
		void Sil(List<int> idler);

        [OperationContract]
        void AboneEkle(AboneIslemleri model);
        [OperationContract]
        void AboneGuncelle(AboneIslemleri model);
        List<AboneAutoComplete> AutoCompleteBilgileriGetir(ENTABONEAra filtre = null);
		[OperationContract]
		void YesilVadiAboneEkle(YesilVadiAboneIslemleri model);

		[OperationContract]
		AboneGenel AboneGenelBilgileriGetir(int aboneKayitNo);

	}
}
