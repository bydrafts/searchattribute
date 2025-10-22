using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace BlueGravity.Editor
{
    [CustomPropertyDrawer(typeof(AssetAttribute), true)]
    public class AssetAttributeDrawer : PropertyDrawer
    {
        [InitializeOnLoadMethod]
        private static void Init() => AssetSearchSettings._findAssets = FindAssets;

        static IEnumerable<Object> FindAssets(Type type, string folder)
        {
            var guids = AssetDatabase.FindAssets($"t:{type.Name}", new[] { folder });
            var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
            return paths.Select(p => AssetDatabase.LoadAssetAtPath(p, type));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var att = attribute as PrefabAttribute ?? throw new Exception("Not a PrefabAttribute");
            var t = fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType() : fieldInfo.FieldType;
            Func<ISearchSettings> getSettings = property.propertyType == SerializedPropertyType.String
                ? GetNameSettings
                : GetSettings;

            SearchAttributeDrawer.Draw(position, property, label, getSettings, att.Lock);
        }

        ISearchSettings GetSettings()
        {
            var a = attribute as PrefabAttribute ?? throw new Exception("Not a PrefabAttribute");
            var t = fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType() : fieldInfo.FieldType;
            return new AssetSearchSettings(t, a.Folder);
        }

        ISearchSettings GetNameSettings()
        {
            var a = attribute as PrefabAttribute ?? throw new Exception("Not a PrefabAttribute");
            var t = fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType() : fieldInfo.FieldType;
            return new AssetNameSearchSettings(t, a.Folder);
        }
    }
}