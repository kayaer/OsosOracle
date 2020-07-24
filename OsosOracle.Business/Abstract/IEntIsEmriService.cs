
using System;
using System.Collections.Generic;
using System.ServiceModel;
using OsosOracle.Entities.ComplexType.EntIsEmriComplexTypes;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.Abstract
{
	[ServiceContract]
	public interface IEntIsEmriService
	{
		[OperationContract]
		EntIsEmri GetirById(int id);

		[OperationContract]
		EntIsEmriDetay DetayGetirById(int id);

		[OperationContract]
		List<EntIsEmri> Getir(EntIsEmriAra filtre = null);

		[OperationContract]
		List<EntIsEmriDetay> DetayGetir(EntIsEmriAra filtre = null);

		[OperationContract]
		int KayitSayisiGetir(EntIsEmriAra filtre = null);


		[OperationContract]
		EntIsEmriDataTable Ara(EntIsEmriAra filtre = null);

		[OperationContract]
		void Ekle(List<EntIsEmri> entityler);

		[OperationContract]
		void Guncelle(List<EntIsEmri> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}
}
