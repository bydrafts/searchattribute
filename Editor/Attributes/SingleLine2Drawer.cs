using UnityEditor;
using UnityEngine;

namespace Drafts.Editor {
    [CustomPropertyDrawer(typeof(SingleLine2Attribute), true)]
    [CustomPropertyDrawer(typeof(SingleLine2Drawer), true)]
    public class SingleLine2Drawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var statRect = new Rect(position.x, position.y, position.width * 0.6f, position.height);
            var scaleRect = new Rect(position.x + position.width * 0.65f, position.y, position.width * 0.35f, position.height);

            var iterator = property.Copy();
            iterator.NextVisible(true);
            EditorGUI.PropertyField(statRect, iterator, GUIContent.none);
            iterator.NextVisible(true);
            EditorGUI.PropertyField(scaleRect, iterator, GUIContent.none);

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}