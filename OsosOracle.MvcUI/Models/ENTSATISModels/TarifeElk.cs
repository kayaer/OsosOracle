using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ENTSATISModels
{
    public class TarifeElk 
    { 
        public decimal SayacSeriNo { get; set; }
   
        public decimal Fiyat1 { get; set; }

        public decimal Fiyat2 { get; set; }
      
        public decimal Fiyat3 { get; set; }
     
        public int Limit1 { get; set; }

        public int Limit2 { get; set; }

        public int YuklemeLimiti { get; set; }
 
        public int AksamSaati { get; set; }

        public int SabahSaati { get; set; }

        public int HaftaSonuAksam { get; set; }

        public int FixCharge { get; set; }

        public int Tatil1Gun { get; set; }
    
        public int Tatil1Ay { get; set; }

        public int Tatil1Sure { get; set; }

        public int Tatil2Gun { get; set; }
 
        public int Tatil2Ay { get; set; }

        public int Tatil2Sure { get; set; }

        public decimal KritikKredi { get; set; }
 
        public int KartNo { get; set; }

        public decimal AnaKredi { get; set; }


        public decimal YedekKredi { get; set; }

 
        public int TarifeTip { get; set; }

        public int KrediKatsayisi { get; set; }


    }
}