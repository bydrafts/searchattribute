using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace Drafts.Editor {
    [CustomPropertyDrawer(typeof(TypeInstanceAttribute))]
    public class TypeInstanceAttributeDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => GetPropertyHeight(property);

        public static float GetPropertyHeight(SerializedProperty property) {
            if (!property.isExpanded) return EditorGUIUtility.singleLineHeight;
            return Mathf.Max(EditorGUI.GetPropertyHeight(property, true), EditorGUIUtility.singleLineHeight);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var fieldType = fieldInfo.FieldType;
            if (fieldType.IsArray) fieldType = fieldType.GetElementType();
            else if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
                fieldType = fieldType.GetGenericArguments()[0];

            if (property.propertyType != SerializedPropertyType.ManagedReference)
                throw new Exception($"Field {fieldInfo.Name} is not a ManagedReference");

            var currValue = property.managedReferenceValue;
            var currType = currValue?.GetType();

            if (currValue == null) {
                var hasLabel = label != GUIContent.none && !string.IsNullOrEmpty(label.text);
                var rect = position;
                rect.height = EditorGUIUtility.singleLineHeight;
                if (hasLabel) rect = EditorGUI.PrefixLabel(rect, GUIUtility.GetControlID(FocusType.Passive), label);
                DrawButton(rect, property, fieldType);
            } else {
                RemoveButton(position, property);
                if (fieldInfo.FieldType.IsArray) label.text = $"     {currType.Name}";
                else label.text = $"  {label.text}: {currType.Name}";
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public static void RemoveButton(Rect pos, SerializedProperty property) {
            pos.width = pos.height = EditorGUIUtility.singleLineHeight;
            if (!GUI.Button(pos, "-")) return;
            property.managedReferenceValue = null;
            property.serializedObject.ApplyModifiedProperties();
        }

        public static void DrawButton(Rect pos, SerializedProperty property, Type fieldType) {

            if (GUI.Button(pos, "null")) {
                var tgt = property.serializedObject.targetObject;
                var settings = new TypeSearchSettings(fieldType);
                settings.Search(tgt, SetValue);
            }

            void SetValue(Type type) {
                var targets = property.serializedObject.targetObjects;
                var propertyPath = property.propertyPath;

                foreach (var t in targets) {
                    var so = new SerializedObject(t);
                    var prop = so.FindProperty(propertyPath);
                    prop.managedReferenceValue = type == null ? null : Activator.CreateInstance(type);
                    so.ApplyModifiedProperties();
                }

                property.serializedObject.Update();
                property.isExpanded = true;
            }
        }
    }
}