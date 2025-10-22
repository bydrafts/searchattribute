using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BlueGravity.Editor {
	public static class SearchWindowHelper {

		public static void OpenWindow<T>(this T provider, VisualElement anchor = null) where T : ScriptableObject, ISearchWindowProvider {
			var pos = new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition));
			if(anchor != null) {
				var point = anchor.worldBound.center + new Vector2(0, anchor.worldBound.height + 8);
				pos = new SearchWindowContext(GUIUtility.GUIToScreenPoint(point));
			}
			SearchWindow.Open(pos, provider);
		}

		public static void AddEntry(this List<SearchTreeEntry> list, string label, int level, object data = null) {
			var entry = new SearchTreeEntry(new GUIContent(label));
			entry.level = level;
			entry.userData = data;
			list.Add(entry);
		}

		public static void AddGroup(this List<SearchTreeEntry> list, string label, int level, object data = null) {
			var entry = new SearchTreeGroupEntry(new GUIContent(label));
			entry.level = level;
			entry.userData = data;
			list.Add(entry);
		}
	}
}
