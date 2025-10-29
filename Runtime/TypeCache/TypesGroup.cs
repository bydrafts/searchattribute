using System;
using System.Collections.Generic;
using System.Linq;

namespace Drafts
{
    public class TypesGroup
    {
        Type baseType;
        public IReadOnlyList<Type> Types { get; }
        public string[] Names { get; }

        internal TypesGroup(Type t)
        {
            baseType = t;
            var derived = new List<Type>();
            foreach (var type in TypeCache.FoundTypes)
                try
                {
                    if (t.IsAssignableFrom(type)) derived.Add(type);
                }
                catch { }

            Types = derived;
            Names = Types.Select(t => t.Name).ToArray();
        }

        public Type Get(string name)
        {
            var result = Types.FirstOrDefault(t => t.Name == name);
            if (result == null) throw new TypeNotFoundExeption(baseType, name);
            return result;
        }
    }
}