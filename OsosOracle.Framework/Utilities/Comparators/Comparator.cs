using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Framework.Utilities.Comparators
{
    internal class Comparator
    {
        internal virtual bool AreEqual(object value1, object value2)
        {
            return object.Equals(value1, value2);
        }
    }
}
