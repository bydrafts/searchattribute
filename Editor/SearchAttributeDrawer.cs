using System;
using UnityEditor;
using UnityEngine;

namespace BlueGravity.Editor {
	[CustomPropertyDrawer(typeof(SearchAttribute), true)]
	public class SearchAttributeDrawer : PropertyDrawer {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			var a = attribute as SearchAttribute ?? throw new Exception("Not SearchAttribute");
			Draw(position, property, label, () => a.Settings, a.Lock);
		}

		public static void Draw(Rect position, SerializedProperty property, GUIContent label, Func<ISearchSettings> getSettings, bool @lock) {
			var propW = position.width - 35;
			EditorGUI.BeginDisabledGroup(@lock);
			EditorGUI.PropertyField(position.NextX(propW), property, label);
			EditorGUI.EndDisabledGroup();
			if(GUI.Button(position, "Find")) {
				var target = property.serializedObject.targetObject;
				SearchProvider.Create(getSettings(), target, property.SetValue).OpenWindow();
			}
		}
	}
}
