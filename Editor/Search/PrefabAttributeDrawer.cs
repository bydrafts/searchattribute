using System;
using UnityEngine;
using UnityEditor;

namespace Drafts.Editor
{
    [CustomPropertyDrawer(typeof(PrefabAttribute), true)]
    public class PrefabAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var att = attribute as PrefabAttribute ?? throw new Exception("Not a PrefabAttribute");
            var t = fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType() : fieldInfo.FieldType;
            Func<ISearchSettings> getSettings = property.propertyType == SerializedPropertyType.String
                ? GetNameSettings
                : GetSettings;
                
            if (!typeof(Component).IsAssignableFrom(t))
                EditorGUI.HelpBox(position, $"Type {t.Name} is not a Component", MessageType.Warning);
            else
                SearchAttributeDrawer.Draw(position, property, label, GetSettings, att.Lock);
        }

        ISearchSettings GetSettings()
        {
            var a = attribute as PrefabAttribute ?? throw new Exception("Not a PrefabAttribute");
            var t = fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType() : fieldInfo.FieldType;
            return new PrefabSearchSettings(t, a.Folder);
        }
        
        ISearchSettings GetNameSettings()
        {
            var a = attribute as PrefabAttribute ?? throw new Exception("Not a PrefabAttribute");
            var t = fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType() : fieldInfo.FieldType;
            return new PrefabNameSearchSettings(t, a.Folder);
        }
    }
}