using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drafts
{
    public class TypeInstanceAttribute : PropertyAttribute { }

    [Serializable]
    public class TypeInstances<T> : IReadOnlyList<T>
    {
        [SerializeReference] public List<T> list = new();

        public int Count => list.Count;
        public T this[int index] => list[index];

        public void Add(T item) => list.Add(item);
        public void AddRange(IEnumerable<T> itens) => list.AddRange(itens);
        public void Remove(T item) => list.Remove(item);
        public void Clear() => list.Clear();

        public TypeInstances(IEnumerable<T> elements) => list.AddRange(elements);
        public TypeInstances() { }

        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}