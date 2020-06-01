using System;
using System.Collections.Generic;
using System.Linq;

namespace Blockchain
{
    internal static class CollectionExtensions
    {
        public static int GetSequenceHashCode<T>(this ICollection<T> sequence)
        {
            if (!sequence.Any())
                return sequence.GetHashCode();

            return sequence
                .Select(item => item.GetHashCode())
                .Aggregate((total, nextCode) => total ^ nextCode);
        }

        public static IList<T> DeepClone<T>(this ICollection<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
        public static IList<T> Clone<T>(this ICollection<T> listToClone)
        {
            return new List<T>(listToClone);
        }
    }
}