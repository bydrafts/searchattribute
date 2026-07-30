using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Drafts
{
    [Serializable]
    public class Managed<T> where T : class
    {
        [SerializeField] private Object target;
        public T Value => target as T;
        public Object Target { get => target; set => target = value is T ? value : null; }
        public static implicit operator T(Managed<T> m) => m?.Value;
    }

    public static class ManagedExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Enumerate<T>(this IEnumerable<Managed<T>> mList) where T : class
        {
            foreach (var managed in mList)
                yield return managed.Value;
        }
    }
}