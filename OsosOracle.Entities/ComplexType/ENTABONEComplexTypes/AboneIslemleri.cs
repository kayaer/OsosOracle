using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;
using OsosOracle.Entities.ComplexType.ENTSAYACComplexTypes;
using OsosOracle.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Entities.ComplexType.ENTABONEComplexTypes
{
    public class AboneIslemleri
    {
        public ENTABONE ENTABONE { get; set; }

        public ENTABONESAYAC ENTABONESAYAC { get; set; }

        public PRMTARIFEELK PRMTARIFEELK { get; set; }

        public PRMTARIFESU PRMTARIFESU { get; set; }
        public PRMTARIFEKALORIMETRE PrmTarifeKALORIMETRE { get; set; }
        public PRMTARIFEGAZ PrmTarifeGaz { get; set; }

        public ENTSAYACDetay EntSayacDetay { get; set; }

        public ENTABONESAYACDetay EntAboneSayacDetay{ get; set; }

        public int KalorimetreKayitNo { get; set; }//Veri taşımak için view e

        public string SuKredi { get; set; }

        public string SuYedekKredi { get; set; }

        public string SuAko { get; set; }
        public string SuAkoMesaj { get; set; }
        public string SuYko { get; set; }
        public string SuYkoMesaj { get; set; }
        public string SuKalan { get; set; }
        public string SuHarcanan { get; set; }

        public string KalorimetreKredi { get; set; }

        public string KalorimetreYedekKredi { get; set; }
        public string KalorimetreAko { get; set; }
        public string KalorimetreAkoMesaj { get; set; }
        public string KalorimetreYko { get; set; }

        public int KurumKayitNo { get; set; }
        public int AboneNo { get; set; }
    }
}
