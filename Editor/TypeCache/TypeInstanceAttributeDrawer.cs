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
            if (currValue == null)
                DrawButton(position, property, fieldType, text);
            else {
                RemoveButton(position, property, text);
                EditorGUI.PropertyField(position, property, label, true);
            }
            EditorGUIUtility.labelWidth /= 1.2f;
        }

        private void RemoveButton(Rect pos, SerializedProperty property, string text) {
            //var w = EditorGUIUtility.labelWidth;
            //pos.x += fieldInfo.FieldType.IsArray ? 0 : w / 2;
            //pos.width = fieldInfo.FieldType.IsArray ? w : w / 2;
            pos.width = EditorGUIUtility.labelWidth;
            pos.height = EditorGUIUtility.singleLineHeight;

            if (!GUI.Button(pos, text)) return;
            property.managedReferenceValue = null;
            property.serializedObject.ApplyModifiedProperties();
        }

        private void DrawButton(Rect pos, SerializedProperty property, Type fieldType, string text) {
            pos.width = EditorGUIUtility.labelWidth;
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