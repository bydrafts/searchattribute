using System;
using System.Collections.Generic;

namespace Drafts
{
    public class TypeSearchSettings : ISearchSettings<Type>
    {
        private Type _baseType;
        public string Title { get; }

        public TypeSearchSettings(Type baseType)
        {
            _baseType = baseType;
            Title = "Derived from " + baseType.Name;
        }

        public IEnumerable<Type> GetItens(object _) => TypeCache.GetDerivedTypes(_baseType).Types;
        public string GetName(Type obj) => obj.Name;
    }
}