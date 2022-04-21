using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Netcode;
using UnityEngine;

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

        public static void Replace<T>(this NetworkList<T> networkList, T original, T replacement) where T : unmanaged, IEquatable<T>
        {
            var idx = networkList.IndexOf(original);
            networkList.Insert(idx, replacement);
            networkList.RemoveAt(idx + 1);
        }

        public static string DebugFormat<T>(this NetworkList<T> networkList) where T : unmanaged, IEquatable<T>
        {
            var networkStrings = networkList.ToEnumerable().Select(x => x.ToString());
            var list = string.Join(",", networkStrings);
            
            return $"[{list}]";
        }
    }
}