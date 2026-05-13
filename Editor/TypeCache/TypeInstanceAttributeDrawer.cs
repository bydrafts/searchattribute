using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace Drafts.Editor {
    [CustomPropertyDrawer(typeof(TypeInstanceAttribute))]
    public class TypeInstanceAttributeDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
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

            EditorGUIUtility.labelWidth *= 1.2f;
            if (currValue == null) {
                DrawButton(position, property, fieldType, "null");
                if (fieldInfo.FieldType.IsArray) label.text = null;
                var hasLabel = label != GUIContent.none && !string.IsNullOrEmpty(label.text);
                var rect = position;
                rect.height = EditorGUIUtility.singleLineHeight;
                if (hasLabel) EditorGUI.PrefixLabel(rect, GUIUtility.GetControlID(FocusType.Passive), label);
            } else {
                RemoveButton(position, property, currType.Name);
                if (fieldInfo.FieldType.IsArray) label.text = "";
                EditorGUI.PropertyField(position, property, label, true);
            }
            EditorGUIUtility.labelWidth /= 1.2f;
        }

        private void RemoveButton(Rect pos, SerializedProperty property, string text) {
            var w = EditorGUIUtility.labelWidth;
            pos.x += fieldInfo.FieldType.IsArray ? 0 : w / 2;
            pos.width = fieldInfo.FieldType.IsArray ? w : w / 2;
            pos.height = EditorGUIUtility.singleLineHeight;

            if (!GUI.Button(pos, text)) return;
            property.managedReferenceValue = null;
            property.serializedObject.ApplyModifiedProperties();
        }

        private void DrawButton(Rect pos, SerializedProperty property, Type fieldType, string text) {
            var w = EditorGUIUtility.labelWidth;
            pos.x += fieldInfo.FieldType.IsArray ? 0 : w / 2;
            pos.width = fieldInfo.FieldType.IsArray ? w : w / 2;
            pos.height = EditorGUIUtility.singleLineHeight;

            if (!GUI.Button(pos, text)) return;
            var tgt = property.serializedObject.targetObject;
            var settings = new TypeSearchSettings(fieldType);
            settings.Search(tgt, SetValue);

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