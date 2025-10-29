using System;
using UnityEditor;
using UnityEngine;

namespace Drafts.Editor
{
    [CustomPropertyDrawer(typeof(TypeInstances<>), true)]
    public class TypeInstanceListDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => GetPropertyHeight(property.FindPropertyRelative("list"));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            OnGUI(position, property.FindPropertyRelative("list"), label,
                fieldInfo.FieldType.GetField("list").FieldType.GenericTypeArguments[0]);
        }

        public static float GetPropertyHeight(SerializedProperty property)
        {
            if (!property.isExpanded) return EditorGUIUtility.singleLineHeight;
            return EditorGUI.GetPropertyHeight(property, true) - EditorGUIUtility.singleLineHeight * 2 + 4;
        }

        public static void OnGUI(Rect position, SerializedProperty property, GUIContent label, Type type)
        {
            void AddElement(Type t)
            {
                var instance = Activator.CreateInstance(t);
                property.arraySize++;
                var p = property.GetArrayElementAtIndex(property.arraySize - 1);
                p.managedReferenceValue = instance;
                property.serializedObject.ApplyModifiedProperties();
            }

            void DrawHeader(Rect pos)
            {
                var btnRect = pos;
                var foldRect = pos;

                btnRect.width = 35;
                foldRect.width -= btnRect.width;
                btnRect.x += foldRect.width;

                property.isExpanded = EditorGUI.Foldout(foldRect, property.isExpanded, label, true);

                if (GUI.Button(btnRect, "Add"))
                {
                    var settings = new TypeSearchSettings(type);
                    var tgt = property.serializedObject.targetObject;
                    settings.Search(tgt, AddElement);
                }
            }

            EditorGUI.BeginProperty(position, label, property);

            if (!property.isExpanded)
            {
                DrawHeader(position);
                EditorGUI.EndProperty();
                return;
            }

            DrawHeader(NextY(ref position, 14));

            var removeRect = position;
            removeRect.width = 20;
            removeRect.height = 14;
            removeRect.x += position.width - removeRect.width;

            EditorGUI.indentLevel++;
            int removeElement = -1;

            for (int i = 0; i < property.arraySize; i++)
            {
                var ele = property.GetArrayElementAtIndex(i);
                var obj = ele.managedReferenceValue;
                var height = EditorGUI.GetPropertyHeight(ele, true);
                var name = obj?.GetType().Name;
                ele.isExpanded = true;

                if (GUI.Button(removeRect, "-")) removeElement = i;
                EditorGUI.PropertyField(NextY(ref position, height), ele, new(name), true);

                removeRect.y += height;
            }

            if (removeElement >= 0)
            {
                property.DeleteArrayElementAtIndex(removeElement);
                property.serializedObject.ApplyModifiedProperties();
            }

            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }
        
        static Rect NextY(ref Rect pos, float height)
        {
            var result = pos;
            result.height = height;
            pos.height -= height;
            pos.y += height;
            return result;
        }
    }
}