using System;
using System.Collections.Generic;
using System.Linq;

namespace Drafts
{
    /// <summary>
    /// exclude assemblies begining with:
    /// unity|system|mscor|ReportGen|cinemach|nunit|log4net
    /// </summary>
    public static class TypeCache<T>
    {
        static TypesGroup Cache => TypeCache.GetDerivedTypes(typeof(T));
        public static IReadOnlyList<Type> All => Cache.Types;
        public static IEnumerable<string> Names => Cache.Names;
        public static Type Get(string name) => Cache.Get(name);
        public static T New(string name, params object[] args) => (T)Activator.CreateInstance(Cache.Get(name), args);

        public static T[] InstantiateAll(params object[] args) =>
            All.Select(t => (T)Activator.CreateInstance(t, args)).ToArray();
    }
}