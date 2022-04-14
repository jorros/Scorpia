using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace Utils
{
    public static class NetworkListExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this NetworkList<T> networkList) where T : unmanaged, IEquatable<T>
        {
            using var enumerator = networkList.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
    }
}