using OsosOracle.MvcUI.Models.SYSGOREVModels;
using OsosOracle.MvcUI.Models.SYSROLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.SYSGOREVROLModels
{
    public class GorevRolAtamaModel
    {
        public List<Rol> RolListesi { get; set; }

        public List<Gorev> GorevListesi { get; set; }
    }
}