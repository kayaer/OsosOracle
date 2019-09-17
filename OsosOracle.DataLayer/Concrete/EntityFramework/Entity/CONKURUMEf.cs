using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class CONKURUMEf : CONKURUM
    {
        public CONKURUMEf()
        {
            EntHaberlesmeUnitesiEfCollection = new List<ENTHABERLESMEUNITESIEf>();
            EntSayacEfCollection = new List<ENTSAYACEf>();
            SysKullaniciEfCollection = new List<SYSKULLANICIEf>();
            SysRolEfCollection = new List<SYSROLEf>();
            PrmTarifeGazEfCollection = new List<PRMTARIFEGAZEf>();
            PrmTarifeElkEfCollection = new List<PRMTARIFEELKEf>();
            PrmTarifeSuEfCollection = new List<PRMTARIFESUEf>();
            PrmTarifeOrtakAvmEfCollection = new List<PRMTARIFEORTAKAVMEf>();
        }
        public ICollection<ENTHABERLESMEUNITESIEf> EntHaberlesmeUnitesiEfCollection { get; set; }

        public ICollection<ENTSAYACEf> EntSayacEfCollection { get; set; }
        public ICollection<SYSKULLANICIEf> SysKullaniciEfCollection { get; set; }
        public ICollection<SYSROLEf> SysRolEfCollection { get; set; }

        public ICollection<PRMTARIFEGAZEf> PrmTarifeGazEfCollection { get; set; }
        public ICollection<PRMTARIFEELKEf> PrmTarifeElkEfCollection { get; set; }
        public ICollection<PRMTARIFESUEf> PrmTarifeSuEfCollection { get; set; }
        public ICollection<PRMTARIFEORTAKAVMEf> PrmTarifeOrtakAvmEfCollection { get; set; }

    }
}
