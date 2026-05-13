using UnityEditor;
using UnityEngine;

namespace Drafts.Editor {
    [CustomPropertyDrawer(typeof(SingleLineAttribute), true)]
    [CustomPropertyDrawer(typeof(ISingleLineDrawer), true)]
    public class SingleLineAttributeDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            label = EditorGUI.BeginProperty(position, label, property);
            var iterator = property.Copy();
            iterator.NextVisible(true);
            EditorGUI.PropertyField(position, iterator, label);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}