using System;
using UnityEngine;
using static UnityEditor.EditorGUIUtility;

namespace Drafts
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FixedAttribute : PropertyAttribute
    {
        public string[] Names { get; }
        public string ArrayPath { get; }

        public FixedAttribute(int size, string arrayPath = null)
        {
            Names = new string[size];
            ArrayPath = arrayPath;
        }

        public FixedAttribute(string[] names, string arrayPath = null)
        {
            Names = names;
            ArrayPath = arrayPath;
        }
        
        public FixedAttribute(Type enumType, string arrayPath = null)
        {
            Names = Enum.GetNames(enumType);
            ArrayPath = arrayPath;
        }
    }

#if UNITY_EDITOR
    namespace Editor
    {
        using UnityEditor;

        [CustomPropertyDrawer(typeof(FixedAttribute), true)]
        public class FixedAttributeDrawer : PropertyDrawer
        {
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                var names = ((FixedAttribute)attribute).Names;
                var size = names.Length;
                var path = ((FixedAttribute)attribute).ArrayPath;
                var array = path == null ? property : property.FindPropertyRelative(path);
                if (array.arraySize != size) array.arraySize = size;
                if (!array.isExpanded) return singleLineHeight;

                var height = singleLineHeight * 2;

                for (var i = 0; i < size; i++)
                {
                    var element = array.GetArrayElementAtIndex(i);
                    height += EditorGUI.GetPropertyHeight(element, true);
                }

                return height;
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                var names = ((FixedAttribute)attribute).Names;
                var size = names.Length;
                var path = ((FixedAttribute)attribute).ArrayPath;
                var array = path == null ? property : property.FindPropertyRelative(path);

                if (!array.isArray)
                {
                    EditorGUI.HelpBox(position, new($"{label.text} is not array or list"));
                    return;
                }

                position.height = singleLineHeight;
                array.isExpanded = EditorGUI.Foldout(position, array.isExpanded, label, true);
                position.y += singleLineHeight + standardVerticalSpacing;
                if (!array.isExpanded) return;

                EditorGUI.indentLevel++;

                for (var i = 0; i < size; i++)
                {
                    var element = array.GetArrayElementAtIndex(i);
                    if (names[i] == null) EditorGUI.PropertyField(position, element, true);
                    else EditorGUI.PropertyField(position, element, new(names[i]), true);
                    position.y += EditorGUI.GetPropertyHeight(element) + standardVerticalSpacing;
                }

                EditorGUI.indentLevel--;
            }
        }
    }
#endif
}