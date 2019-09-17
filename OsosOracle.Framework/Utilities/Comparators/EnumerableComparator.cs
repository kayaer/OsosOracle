using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Framework.Utilities.Comparators
{
    internal class EnumerableComparator<T> : Comparator
    {
        internal override bool AreEqual(object value1, object value2)
        {
            var val1 = (IEnumerable<T>)value1;
            var val2 = (IEnumerable<T>)value2;

            if (val1 == null && val2 == null)
            {
                return true;
            }

            if (val1 == null || val2 == null)
            {
                return false;
            }

            return System.Linq.Enumerable.SequenceEqual<T>(val1, val2);
        }
    }
}
