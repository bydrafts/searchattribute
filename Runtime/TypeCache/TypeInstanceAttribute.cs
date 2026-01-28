using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Drafts
{
    public class TypeInstanceAttribute : PropertyAttribute { }

    [Serializable]
    public class TypeInstances<T> : IReadOnlyList<T>
    {
        [SerializeReference, TypeInstance] public T[] list;

        public int Count => list.Length;
        public T this[int index] => list[index];

        public TypeInstances(IEnumerable<T> elements) => list = elements.ToArray();
        public TypeInstances() => list = new T[0];

        public IEnumerator<T> GetEnumerator() => ((IReadOnlyList<T>)list).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}