using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Drafts
{
    public class HideFromTypeCacheAttribute : Attribute { }

    /// <summary>
    /// Cache subtypes for faster reflection calls
    /// </summary>
    public static class TypeCache
    {
        private static IEnumerable<Assembly> _assemblies = AppDomain.CurrentDomain.GetAssemblies();
        private static IReadOnlyList<Type> _foundTypes;
        private static Dictionary<Type, IReadOnlyList<Type>> _cache = new();

        public static IReadOnlyList<Type> FoundTypes => _foundTypes ??= FindTypes();

        public static void SetAssemblies(IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies;
            _foundTypes = null;
            _cache.Clear();
        }

        private static IReadOnlyList<Type> FindTypes()
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
            !type.IsAbstract && !type.IsGenericType && !type.IsInterface
            && type.GetCustomAttribute<HideFromTypeCacheAttribute>() == null;

        public static IReadOnlyList<Type> GetDerivedTypes(Type type)
        {
            if (_cache.TryGetValue(type, out var cache)) return cache;
            return _cache[type] = FoundTypes.Where(type.IsAssignableFrom).ToList();
        }

        public static Type Get(Type type, string name)
        {
            return GetDerivedTypes(type).FirstOrDefault(t => t.Name == name);
        }
    }
}