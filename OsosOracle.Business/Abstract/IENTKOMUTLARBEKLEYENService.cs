
using System;
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.ENTKOMUTLARBEKLEYENComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IENTKOMUTLARBEKLEYENService
	{
		[OperationContract]
		ENTKOMUTLARBEKLEYEN GetirById(int id);

		[OperationContract]
		ENTKOMUTLARBEKLEYENDetay DetayGetirById(int id);

		[OperationContract]
		List<ENTKOMUTLARBEKLEYEN> Getir(ENTKOMUTLARBEKLEYENAra filtre = null);

		[OperationContract]
		List<ENTKOMUTLARBEKLEYENDetay> DetayGetir(ENTKOMUTLARBEKLEYENAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(ENTKOMUTLARBEKLEYENAra filtre = null);


		[OperationContract]
		ENTKOMUTLARBEKLEYENDataTable Ara(ENTKOMUTLARBEKLEYENAra filtre = null);

		[OperationContract]
		void Ekle(List<ENTKOMUTLARBEKLEYEN> entityler);

		[OperationContract]
		void Guncelle(List<ENTKOMUTLARBEKLEYEN> entityler);

		[OperationContract]
		void Sil(List<Guid> idler);
	}
}
