#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Drafts.Editor
{
    [CustomPropertyDrawer(typeof(GuidRef<>), true)]
    public class GuidRefDrawer : PropertyDrawer
    {
        private static readonly GuidSearchSettings SearchSettings = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = EditorGUI.PrefixLabel(position, label);
            position.width -= position.height;

            Guid guid;
            var bytes = property.FindPropertyRelative("serializedGuid"); //byte[]

            if (bytes is not { arraySize: 16 })
                guid = Guid.Empty;
            else
            {
                var guidBytes = new byte[16];
                for (var i = 0; i < 16; i++) guidBytes[i] = (byte)bytes.GetArrayElementAtIndex(i).intValue;
                guid = new Guid(guidBytes);
            }

            if (guid == Guid.Empty)
                EditorGUI.LabelField(position, "null");
            else
            {
                var go = GuidComponent.Get(guid);

                if (!go) EditorGUI.LabelField(position, "not loaded");
                else
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUI.ObjectField(position, GUIContent.none, go, typeof(GameObject), true);
                    EditorGUI.EndDisabledGroup();
                }
            }

            position.x += position.width;
            position.width = position.height;

            if (!GUI.Button(position, "o")) return;
            if (property.propertyType != SerializedPropertyType.ManagedReference)
                throw new Exception("Only ManagedReference can be set in inspector.");

            if (property.managedReferenceValue is not IGuidRef)
                property.managedReferenceValue = Activator.CreateInstance(fieldInfo.FieldType);

            var g = (IGuidRef)property.managedReferenceValue;
            SearchSettings.Search(g.ComponentType, SetGuid);

            void SetGuid(KeyValuePair<Guid, GameObject> p)
            {
                g.Guid = p.Key;
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
#endif