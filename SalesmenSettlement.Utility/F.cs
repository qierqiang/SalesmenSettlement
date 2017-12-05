using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ZOC.IO
{
    static class F
    {
        internal static int Count(IEnumerable collection)
        {
            if (collection is IList)
                return ((IList)collection).Count;

            IEnumerator etor = collection.GetEnumerator();

            int result = 0;

            while (etor.MoveNext())
                result++;

            return result;
        }

        internal static T ElementAt<T>(IEnumerable<T> collection, int index)
        {
            if (collection is IList<T>)
                return ((IList<T>)collection)[index];

            int count = -1;
            IEnumerator<T> etor = collection.GetEnumerator();

            while (count != index)
                etor.MoveNext();

            return etor.Current;
        }

        internal static bool Any(IEnumerable collection)
        {
            IEnumerator etor = collection.GetEnumerator();
            return etor.MoveNext();
        }

        internal static bool IsNullOrBlank(string source)
        {
            return string.IsNullOrEmpty(source) || source.Trim().Length == 0;
        }
    }
}
