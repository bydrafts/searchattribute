using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Drafts.Editor
{
    /// <summary>
    /// Whapper to UnityEditor.SearchProvider api
    /// </summary>
    public class SearchProvider : ScriptableObject, ISearchWindowProvider
    {
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

            foreach (var asset in settings.GetItems(target))
                list.AddEntry(settings.GetName(asset), 1, asset);

            return list;
        }

        public virtual bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            onSelected(entry.userData);
            return true;
        }

        public static void Open<T>(Vector2? position, ISearchSettings settings, object target, Action<T> onSelected)
        {
            Create(settings,target,onSelected).OpenWindow(position);
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