using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Drafts
{
    public class PrefabSearchSettings : ISearchSettings<Component>
    {
        public Type Type { get; }
        public string Folder { get; }
        public string Title => $"{Type.Name} in {Folder}";

        private readonly Func<Component, bool> _validate;

        public IEnumerable<Component> GetItens(object target) => GetPrefabs(Type, Folder, _validate);
        public string GetName(Component o) => o?.name;

        public PrefabSearchSettings(Type type, string folder, Func<Component, bool> validate = null)
        {
            Type = type;
            Folder = folder;
            _validate = validate ?? (c => c);
        }

        public static IEnumerable<Component> GetPrefabs(Type type, string folder, Func<Component, bool> validate)
            => AssetSearchSettings._findAssets(typeof(GameObject), folder)
                .Select(o => (o as GameObject)?.GetComponent(type)).Where(validate);
    }

    public class PrefabNameSearchSettings : ISearchSettings<string>
    {
        public Type Type { get; }
        public string Folder { get; }
        public string Title => $"{Type.Name} in {Folder}";

        private readonly Func<Component, bool> _validate;

        public IEnumerable<string> GetItens(object target)
            => PrefabSearchSettings.GetPrefabs(Type, Folder, _validate).Select(c => c.name);

        public string GetName(string o) => o;

        public PrefabNameSearchSettings(Type type, string folder, Func<Component, bool> validate = null)
        {
            Type = type;
            Folder = folder;
            _validate = validate ?? (c => c);
        }
    }
}