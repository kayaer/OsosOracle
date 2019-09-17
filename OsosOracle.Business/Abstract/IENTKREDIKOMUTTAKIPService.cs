
using System;
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.ENTKREDIKOMUTTAKIPComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IENTKREDIKOMUTTAKIPService
	{
		[OperationContract]
		ENTKREDIKOMUTTAKIP GetirById(int id);

		[OperationContract]
		ENTKREDIKOMUTTAKIPDetay DetayGetirById(int id);

		[OperationContract]
		List<ENTKREDIKOMUTTAKIP> Getir(ENTKREDIKOMUTTAKIPAra filtre = null);

		[OperationContract]
		List<ENTKREDIKOMUTTAKIPDetay> DetayGetir(ENTKREDIKOMUTTAKIPAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(ENTKREDIKOMUTTAKIPAra filtre = null);


		[OperationContract]
		ENTKREDIKOMUTTAKIPDataTable Ara(ENTKREDIKOMUTTAKIPAra filtre = null);

		[OperationContract]
		void Ekle(List<ENTKREDIKOMUTTAKIP> entityler);

		[OperationContract]
		void Guncelle(List<ENTKREDIKOMUTTAKIP> entityler);

		[OperationContract]
		void Sil(List<Guid> idler);
	}
}
