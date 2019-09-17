
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.SYSCSTOPERASYONModels;
using System.Collections.Generic;

namespace OsosOracle.MvcUI.Models.SYSGOREVModels
{
	public class SYSGOREVKaydetModel
	{
		public SYSGOREV SYSGOREV { get; set; }

        public List<Operasyon> OperasyonListesi { get; set; }
    }
}
