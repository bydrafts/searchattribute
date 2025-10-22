using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BlueGravity.Editor
{
    /// <summary>
    /// Whapper to UnityEditor.SearchProvider api
    /// </summary>
    public class SearchProvider : ScriptableObject, ISearchWindowProvider
    {
        //static SearchProvider() => AssetSearchSettings._findAssets = FindAssets;
        //
        // static IEnumerable<UnityEngine.Object> FindAssets(Type type, string folder) {
        // 	if(string.IsNullOrEmpty(folder))
        // 		return EditorUtil.FindAssets(type);
        // 	return EditorUtil.FindAssets(type, folder);
        // }

        internal object target;
        internal ISearchSettings settings;
        public Action<object> onSelected;

        public static SearchProvider Create(ISearchSettings settings, object target, Action<object> onSelected)
        {
            var so = CreateInstance<SearchProvider>();
            so.target = target;
            so.settings = settings;
            so.onSelected = onSelected;
            return so;
        }

        public static SearchProvider Create<T>(ISearchSettings settings, object target, Action<T> onSelected)
        {
            var so = CreateInstance<SearchProvider>();
            so.target = target;
            so.settings = settings;
            so.onSelected = obj => onSelected((T)obj);
            return so;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var list = new List<SearchTreeEntry>();
            list.AddGroup(settings.Title, 0);

            foreach (var asset in settings.GetItens(target))
                list.AddEntry(settings.GetName(asset), 1, asset);

            return list;
        }

        public virtual bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            onSelected(entry.userData);
            return true;
        }
    }

    public static class ExtensionsISearchSettings
    {
        public static void Search(this ISearchSettings settings, object target, Action<object> onSelected)
            => SearchProvider.Create(settings, target, onSelected).OpenWindow();

        public static void Search<T>(this ISearchSettings<T> settings, object target, Action<T> onSelected)
            => SearchProvider.Create(settings, target, onSelected).OpenWindow();
    }
}