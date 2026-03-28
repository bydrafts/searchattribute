using System;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Drafts.Editor
{
    [CustomPropertyDrawer(typeof(PrefabAttribute), true)]
    public class PrefabAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var att = attribute as PrefabAttribute ?? throw new Exception("Not a PrefabAttribute");
            var t = fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType() : fieldInfo.FieldType;

            if (!typeof(Component).IsAssignableFrom(t))
            {
                EditorGUI.HelpBox(position, $"Type {t.Name} is not a Component", MessageType.Warning);
                return;
            }

            var target = property.serializedObject.targetObject;
            var showCreate = !property.objectReferenceValue && EditorUtility.IsPersistent(target);

            if (showCreate)
            {
                const float btnW = 60;
                var btnRect = new Rect(position.x + position.width - btnW, position.y, btnW, position.height);
                var propRect = new Rect(position.x, position.y, position.width - btnW - 2, position.height);

                if (GUI.Button(btnRect, "Create")) CreatePrefab(property, t);
                SearchAttributeDrawer.Draw(propRect, property, label, GetSettings, att.Lock);
            }
            else
                SearchAttributeDrawer.Draw(position, property, label, GetSettings, att.Lock);
        }

        private void CreatePrefab(SerializedProperty property, Type componentType)
        {
            var go = new GameObject(componentType.Name);
            go.AddComponent(componentType);

            var path = AssetDatabase.GetAssetPath(property.serializedObject.targetObject);
            path = string.IsNullOrEmpty(path) ? "Assets" : Path.GetDirectoryName(path);

            var fullPath = AssetDatabase.GenerateUniqueAssetPath($"{path}/{componentType.Name}.prefab");
            var prefab = PrefabUtility.SaveAsPrefabAsset(go, fullPath);
            UnityEngine.Object.DestroyImmediate(go);

            property.objectReferenceValue = prefab.GetComponent(componentType);
            property.serializedObject.ApplyModifiedProperties();
        }

        private ISearchSettings GetSettings()
        {
            var a = attribute as PrefabAttribute ?? throw new Exception("Not a PrefabAttribute");
            var t = fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType() : fieldInfo.FieldType;
            return new PrefabSearchSettings(t, a.Folder);
        }
    }
}
