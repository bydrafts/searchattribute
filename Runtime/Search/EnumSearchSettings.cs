using System;
using System.Collections.Generic;
using System.Linq;

namespace Drafts
{
    public class EnumSearchSettings<T> : ISearchSettings<T> where T : Enum
    {
        public string Title => typeof(T).Name;
        public IEnumerable<T> GetItems(object target) => Enum.GetValues(typeof(T)).OfType<T>();
        public string GetName(T obj) => obj.ToString();
    }
}