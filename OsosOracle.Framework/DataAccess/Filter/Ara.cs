using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Framework.DataAccess.Filter
{
    public class Ara
    {
        public int? Baslangic { get; set; }
        public int? Uzunluk { get; set; }

        public List<Siralama> Siralama { get; set; }

        //public Ara(int? basla, int? uzunluk)
        //{
        //    if (uzunluk.HasValue)
        //    {
        //        Uzunluk = uzunluk.Value > 0 ? uzunluk : null;
        //    }
        //}
    }
}
