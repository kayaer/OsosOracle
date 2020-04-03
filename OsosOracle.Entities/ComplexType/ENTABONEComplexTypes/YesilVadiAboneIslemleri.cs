using OsosOracle.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Entities.ComplexType.ENTABONEComplexTypes
{
    public class YesilVadiAboneIslemleri
    {
        public ENTABONE ENTABONE { get; set; }

        public ENTABONESAYAC SuSayac { get; set; }

        public ENTABONESAYAC ElektrikSayac { get; set; }
    }
}
