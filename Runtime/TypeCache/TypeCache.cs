using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Drafts
{
    public class HideFromTypeCacheAttribute : Attribute { }

    public class TypeNotFoundExeption : Exception
    {
        public TypeNotFoundExeption(Type type, string name) : base($"TypeCache<{type?.Name}>: {name} not found.") { }
    }

    /// <summary>
    /// Cache subtypes for faster reflection calls
    /// </summary>
    public static class TypeCache
    {
        private static IEnumerable<Assembly> _assemblies = AppDomain.CurrentDomain.GetAssemblies();
        private static IReadOnlyList<Type> _foundTypes;
        private static Dictionary<Type, TypesGroup> _cache = new();

        public static IReadOnlyList<Type> FoundTypes => _foundTypes ??= FindTypes();

        public static void SetAssemblies(IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies;
            _foundTypes = null;
            _cache.Clear();
        }

        static IReadOnlyList<Type> FindTypes()
        {
            if (_assemblies == null) throw new Exception("Assembly not set. Call SetAssemblies first.");
            IEnumerable<Type> all = new List<Type>();

            foreach (var assembly in _assemblies)
            {
                try
                {
                    all = all.Concat(assembly.GetTypes());
                }
                catch (ReflectionTypeLoadException e)
                {
                    all = all.Concat(e.Types.Where(t => t != null));
                }
            }

            return all.Where(IsCompatible).OrderBy(t => t.Name).ToList();
        }

        private static bool IsCompatible(Type type) =>
            !type.IsAbstract && !type.IsGenericTypeDefinition && !type.IsInterface
            && type.GetCustomAttribute<HideFromTypeCacheAttribute>() == null;

        public static TypesGroup GetDerivedTypes(Type type)
        {
            if (_cache.TryGetValue(type, out var cache)) return cache;
            return _cache[type] = new TypesGroup(type);
        }
    }
}