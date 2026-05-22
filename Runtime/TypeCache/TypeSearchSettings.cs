using System;
using System.Collections.Generic;
using System.Reflection;

namespace Drafts
{
    public class TypeSearchSettings : ISearchSettings<Type>
    {
        private readonly Type _baseType;
        private readonly bool _showNull;
        public string Title { get; }

        public TypeSearchSettings(Type baseType, bool showNull = false) {
            _baseType = baseType;
            _showNull = showNull;
            Title = "Derived from " + baseType.Name;
        }

        public IEnumerable<Type> GetItems(object _) {
            if(_showNull) yield return null;
            foreach (var t in TypeCache.GetDerivedTypes(_baseType))
                if(t.GetCustomAttribute<ObsoleteAttribute>() == null)
                    yield return t;
        }

        public string GetName(Type obj) => obj?.Name ?? "null";
    }
}