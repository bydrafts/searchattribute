using System;
using UnityEditor;
using UnityEngine;

namespace Drafts.Editor
{
    [CustomPropertyDrawer(typeof(SearchAttribute), true)]
    public class SearchAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var a = attribute as SearchAttribute ?? throw new Exception("Not SearchAttribute");
            Draw(position, property, label, () => a.Settings, a.Lock);
        }

        public static void Draw(Rect position, SerializedProperty property, GUIContent label, Func<ISearchSettings> getSettings, bool @lock)
        {
            if (position.width > 95)
            {
                var labelRect = position;
                labelRect.width -= 35;
                position.width = 35;
                position.x += labelRect.width;

                EditorGUI.BeginDisabledGroup(@lock);
                EditorGUI.PropertyField(labelRect, property, label);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                position.width -= 2;
                position.x += 2;
            }

            if (!GUI.Button(position, "Find")) return;
            var target = property.serializedObject.targetObject;
            SearchProvider.Create(getSettings(), target, property.SetValue).OpenWindow();
        }
    }
}