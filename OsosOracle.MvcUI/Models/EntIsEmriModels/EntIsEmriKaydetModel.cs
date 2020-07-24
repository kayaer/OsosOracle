
using OsosOracle.Entities.Concrete;
using System;

namespace OsosOracle.MvcUI.Models.EntIsEmriModels
{
	public class EntIsEmriKaydetModel
	{
		public EntIsEmri EntIsEmri { get; set; }
		public DateTime Tarih { get; set; }

		public string ServerIp { get; set; }

		public string ServerPort { get; set; }

		public string ApnAdi { get; set; }

		public string ApnUser { get; set; }

		public string ApnPassword { get; set; }

		public bool UstKapakCezaTemizle { get; set; }
		public bool PilKapakCezaTemizle { get; set; }

		public bool GuncelTarih { get; set; }

		public string KrediIslemNo { get; set; }

		public string Kredi { get; set; }

		public int ZamanlanmisGorev { get; set; }
	}
}
