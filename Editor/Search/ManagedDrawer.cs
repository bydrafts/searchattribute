#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Drafts
{
    [CustomPropertyDrawer(typeof(Managed<>), true)]
    public class ManagedDrawer : PropertyDrawer
    {
        private static readonly InterfaceAssetSearch Search = new();
        private static readonly Type ListType = typeof(List<>);

        private static bool ParseType(ref Type type)
        {
            try
            {
                if (type.IsArray)
                    type = type.GetElementType(); // Manged<T>[]
                else if (type.GetGenericTypeDefinition() == ListType)
                    type = type.GenericTypeArguments[0];
                type = type?.GenericTypeArguments[0];
                return true;
            }
            catch (Exception)
            {
                Debug.LogError($"Invalid type {type?.Name}");
                return false;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var target = property.FindPropertyRelative("target");
            var type = fieldInfo.FieldType;
            if (!ParseType(ref type)) return;

            GUI.enabled = true;
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label); // disabled

            position.width -= 40;
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(position, target, GUIContent.none);
            EditorGUI.EndDisabledGroup();

            position.x += position.width;
            position.width = 40;

            if (GUI.Button(position, "Find")) // not disable
                Search.Search(type, target.SetValue);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUIUtility.singleLineHeight;
    }
}
#endif