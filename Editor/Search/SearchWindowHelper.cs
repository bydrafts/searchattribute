using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Drafts.Editor
{
    public static class SearchWindowHelper
    {
        public static void OpenWindow<T>(this T provider, Vector2? position = null) where T : ScriptableObject, ISearchWindowProvider
        {
            Vector2 pos;

            if (position.HasValue)
                pos = position.Value;
            else if (Event.current != null)
                pos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            else
            {
                var w = EditorWindow.focusedWindow.position;
                pos = new Vector2(w.x, w.y + w.height * 0.5f);
            }

            SearchWindow.Open(new SearchWindowContext(pos), provider);
        }

        public static void AddEntry(this List<SearchTreeEntry> list, string label, int level, object data = null)
        {
            var entry = new SearchTreeEntry(new GUIContent(label));
            entry.level = level;
            entry.userData = data;
            list.Add(entry);
        }

        public static void AddGroup(this List<SearchTreeEntry> list, string label, int level, object data = null)
        {
            var entry = new SearchTreeGroupEntry(new GUIContent(label));
            entry.level = level;
            entry.userData = data;
            list.Add(entry);
        }
    }
}