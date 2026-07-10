using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

namespace Drafts
{
    [CustomPropertyDrawer(typeof(SOAttribute))]
    public class SOAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var t = fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType() : fieldInfo.FieldType;

            if (!typeof(ScriptableObject).IsAssignableFrom(t))
            {
                EditorGUI.HelpBox(position, $"Type {t.Name} is not a Component", MessageType.Warning);
                return;
            }
            
            if (property.objectReferenceValue)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }
            
            const float buttonWidth = 60;
            var fieldRect = new Rect(position.x, position.y, position.width - buttonWidth - 2, position.height);
            var buttonRect = new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, position.height);

            EditorGUI.PropertyField(fieldRect, property, label);
            if (GUI.Button(buttonRect, "Create")) ShowCreateMenu(property);
        }

        private void ShowCreateMenu(SerializedProperty property)
        {
            var type = fieldInfo.FieldType;
            if (type.IsArray) type = type.GetElementType();
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                type = type.GetGenericArguments()[0];

            var types = TypeCache.GetDerivedTypes(type);

            if (types.Count == 0)
                Debug.LogError($"No valid ScriptableObject types found for {type.Name}");
            else if (types.Count == 1)
                CreateAsset(property, types[0]);
            else
            {
                var menu = new GenericMenu();
                foreach (var t in types)
                    menu.AddItem(new GUIContent(t.Name), false, () => CreateAsset(property, t));
                menu.ShowAsContext();
            }
        }

        private static void CreateAsset(SerializedProperty property, Type type)
        {
            var instance = ScriptableObject.CreateInstance(type);
            instance.name = type.Name;

            string path;
            if (property.serializedObject.targetObject is Component component)
                path = AssetDatabase.GetAssetPath(component.gameObject);
            else
                path = AssetDatabase.GetAssetPath(property.serializedObject.targetObject);

            path = string.IsNullOrEmpty(path) ? "Assets" : Path.GetDirectoryName(path);

            var name = $"{property.serializedObject.targetObject.name} {property.name}";
            var fullPath = AssetDatabase.GenerateUniqueAssetPath($"{path}/{name}.asset");
            AssetDatabase.CreateAsset(instance, fullPath);
            AssetDatabase.SaveAssets();

            property.objectReferenceValue = instance;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
