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

        public ENTABONEBILGI ENTABONEBILGI { get; set; }

        public ENTABONESAYAC ENTABONESAYAC { get; set; }

        public PRMTARIFEELK PRMTARIFEELK { get; set; }

        public PRMTARIFESU PRMTARIFESU { get; set; }

        public ENTSAYAC ENTSAYAC { get; set; }
    }
}
