using OsosOracle.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ENTABONEModels
{
    public class YesilVadiAboneKaydetModel
    {
        public ENTABONE ENTABONE { get; set; }

        public ENTABONESAYAC SuSayac { get; set; }

        public ENTABONESAYAC ElektrikSayac { get; set; }
    }
}