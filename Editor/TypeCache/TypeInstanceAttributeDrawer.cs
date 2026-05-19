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

            var text = fieldInfo.FieldType.IsArray ? "" : label.text + ": ";
            text += currType?.Name ?? "null";
            label.text = " ";

            EditorGUIUtility.labelWidth *= 1.2f;
            DrawButton(position, property, fieldType, text);
            if (currValue != null)
                EditorGUI.PropertyField(position, property, label, true);
            EditorGUIUtility.labelWidth /= 1.2f;
        }

        private void DrawButton(Rect pos, SerializedProperty property, Type fieldType, string text) {
            var delta = EditorGUI.IndentedRect(pos).x - pos.x;
            pos.width = EditorGUIUtility.labelWidth - delta;
            pos.x += delta;
            pos.height = EditorGUIUtility.singleLineHeight;

            if (!GUI.Button(pos, text)) return;
            var tgt = property.serializedObject.targetObject;
            var settings = new TypeSearchSettings(fieldType, true);
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