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

        public static float GetPropertyHeight(SerializedProperty property)
        {
            if (!property.isExpanded) return EditorGUIUtility.singleLineHeight;
            return Mathf.Max(EditorGUI.GetPropertyHeight(property, true), EditorGUIUtility.singleLineHeight);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var fieldType = fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType() : fieldInfo.FieldType;
        
            if (property.propertyType != SerializedPropertyType.ManagedReference)
                throw new Exception("Field is not a ManagedReference");

            EditorGUI.BeginProperty(position, label, property);
            var currValue = property.managedReferenceValue;
            var currType = currValue?.GetType();

            var rect = position;
            rect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), GUIContent.none);
            rect.height = EditorGUIUtility.singleLineHeight;
            rect.width -= EditorGUIUtility.labelWidth;
            rect.x += EditorGUIUtility.labelWidth;
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