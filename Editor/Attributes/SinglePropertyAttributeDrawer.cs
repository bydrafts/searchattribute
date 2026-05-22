using UnityEditor;
using UnityEngine;

namespace Drafts.Editor {
    [CustomPropertyDrawer(typeof(SinglePropertyAttribute), true)]
    [CustomPropertyDrawer(typeof(ISinglePropertyDrawer), true)]
    public class SinglePropertyAttributeDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            var copy = property.Copy();
            copy.NextVisible(true);
            return EditorGUI.GetPropertyHeight(copy, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var copy = property.Copy();
            copy.NextVisible(true);
            if(label.text.StartsWith("Element ")) label = GUIContent.none;
            if(string.IsNullOrEmpty(label.text)) label = GUIContent.none;
            EditorGUI.PropertyField(position, copy, label, true);
        }
    }
}