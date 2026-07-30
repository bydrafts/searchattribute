#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Drafts
{
    internal class InterfaceAssetSearchSettingsBase : ISearchSettings<Object>
    {
        public Type Type;

        public string Title => $"Implements {Type.Name}";
        public string GetName(Object obj) => obj.name;

        public InterfaceAssetSearchSettingsBase(Type type) => Type = type;

        public IEnumerable<Object> GetItems(object target)
        {
            if (target is not AssetSearchScope scope) throw new Exception("target is not a Scope");
            return scope switch
            {
                AssetSearchScope.Scriptable => FromAssets(Type, typeof(ScriptableObject), false),
                AssetSearchScope.Prefab => FromAssets(Type, typeof(GameObject), true),
                AssetSearchScope.Hierarchy => FromHierarchy(Type),
                _ => null
            };
        }

        private static IEnumerable<Object> FromAssets(Type type, Type assetType, bool checkComponents)
        {
            if (assetType == null) yield break;

            foreach (var guid in AssetDatabase.FindAssets($"t:{assetType.Name}"))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var obj = AssetDatabase.LoadAssetAtPath(path, assetType);
                if (!obj) continue;

                if (type.IsAssignableFrom(obj.GetType())) yield return obj;
                if (!checkComponents || obj is not GameObject go) continue;
                if (go.TryGetComponent(type, out var c)) yield return c;
            }
        }

        private static IEnumerable<Object> FromHierarchy(Type type)
        {
            foreach (var mb in Resources.FindObjectsOfTypeAll(typeof(MonoBehaviour)))
            {
                if (EditorUtility.IsPersistent(mb)) continue;
                if ((mb.hideFlags & HideFlags.HideInHierarchy) != 0) continue;
                if (type.IsAssignableFrom(mb.GetType())) yield return mb;
            }
        }
    }
}
#endif