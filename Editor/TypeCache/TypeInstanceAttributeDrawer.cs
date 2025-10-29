using UnityEngine;
using UnityEditor;
using System;

namespace Drafts.Editor
{
    [CustomPropertyDrawer(typeof(TypeInstanceAttribute))]
    public class TypeInstanceAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => GetPropertyHeight(property);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            => OnGUI(position, property, label, fieldInfo.FieldType);

        public static float GetPropertyHeight(SerializedProperty property)
        {
            if (!property.isExpanded) return EditorGUIUtility.singleLineHeight;
            return Mathf.Max(EditorGUI.GetPropertyHeight(property, true), EditorGUIUtility.singleLineHeight);
        }

        public static void OnGUI(Rect position, SerializedProperty property, GUIContent label, Type fieldType)
        {
            if (property.propertyType != SerializedPropertyType.ManagedReference)
                throw new Exception("Field is not a ManagedReference");

            EditorGUI.BeginProperty(position, label, property);
            var currValue = property.managedReferenceValue;
            var currType = currValue?.GetType();

            var rect = position;
            rect.height = EditorGUIUtility.singleLineHeight;
            //rect.width /= 2;
            rect.width = EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - 72.5f;
            rect.x = EditorGUIUtility.labelWidth;
            DrawButton(rect, property, new(currType?.Name), fieldType);

            if (currType == null) EditorGUI.LabelField(position, label);
            else EditorGUI.PropertyField(position, property, label, true);

            EditorGUI.EndProperty();
        }

        public static void DrawButton(Rect pos, SerializedProperty property, GUIContent label, Type fieldType)
        {
            if (GUI.Button(pos, label))
            {
                var tgt = property.serializedObject.targetObject;
                var settings = new TypeSearchSettings(fieldType);
                settings.Search(tgt, SetValue);
            }

            void SetValue(Type obj)
            {
                property.managedReferenceValue = Activator.CreateInstance((Type)obj);
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}