using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Framework.Utilities.Comparators
{
    /// <summary>
    /// only supports EF primitive types
    /// https://msdn.microsoft.com/en-us/library/ee382832.aspx
    /// </summary>
    internal static class ComparatorFactory
    {

        internal static Comparator GetComparator(Type type)
        {
            // if this is an IEquatable<type>
            var IEqOfType = typeof(IEquatable<>).MakeGenericType(type);
            if (IEqOfType.IsAssignableFrom(type))
            {
                return new Comparator();
            }

            if (type.IsValueType)
            {
                return new Comparator();
            }

            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                var comparatorType = typeof(EnumerableComparator<>).MakeGenericType(elementType);

                return (Comparator)Activator.CreateInstance(comparatorType);
            }

            // type is IEnumerable
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                var genericType = type.GenericTypeArguments[0];
                var comparatorType = typeof(EnumerableComparator<>).MakeGenericType(genericType);

                return (Comparator)Activator.CreateInstance(comparatorType);
            }

            Debug.Fail(string.Format(
                @"Entity Framework should only use this method for primitive types. Try implementing a custom comparator for type {0} in GTHB.Core project. Alternatively, implement IEquatable<T> in type {0}.",
                type));

            return new Comparator();
        }
    }
}
