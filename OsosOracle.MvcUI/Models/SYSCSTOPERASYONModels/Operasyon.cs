﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.SYSCSTOPERASYONModels
{
    public class Operasyon
    {
        public int KayitNo { get; set; }
        public string Ad { get; set; }
        public string Aciklama { get; set; }

        public bool Secildi { get; set; }
    }
}