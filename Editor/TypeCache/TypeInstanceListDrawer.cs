using System;
using UnityEditor;
using UnityEngine;

namespace Drafts.Editor
{
    [CustomPropertyDrawer(typeof(TypeInstances<>), true)]
    public class TypeInstanceListDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUI.GetPropertyHeight(property.FindPropertyRelative("list"));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            => EditorGUI.PropertyField(position, property.FindPropertyRelative("list"), label);
    }
}