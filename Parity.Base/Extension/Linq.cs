using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq
{
    public static class LinqExtension
    {

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
                action(item);
        }

        public static int IndexOf<T>(this IEnumerable<T> enumeration, T predicate, int begin = 0)
        {
            int counter = begin;
            foreach (T item in enumeration)
            {
                if (Object.Equals(item, predicate))
                    return counter;
                counter++;
            }
            return -1;
        }

    }
}
