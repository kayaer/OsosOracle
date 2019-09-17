using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Framework.SharedModels
{
    public class AutoCompleteData
    {
        public string id { get; set; }
        public string text { get; set; }
        public string description { get; set; }
    }

    public class AutoCompleteDataPaged
    {
        public List<AutoCompleteData> acData { get; set; }
        public int total { get; set; }
    }
}
