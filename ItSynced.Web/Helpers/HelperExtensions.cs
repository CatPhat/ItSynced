using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItSynced.Web.Helpers
{
    public static class HelperExtensions
    {
        // Depth-first traversal, recursive
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenSelector)
        {
            if (source == null) yield break;
            foreach (var item in source)
            {
                yield return item;
                foreach (var child in childrenSelector(item).Flatten(childrenSelector))
                {
                    yield return child;
                }
            }
        }
    }
}
