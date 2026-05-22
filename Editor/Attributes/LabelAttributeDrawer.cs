using UnityEditor;
using UnityEngine;

namespace Drafts.Editor {
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelAttributeDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            label.text = ((LabelAttribute)attribute)!.Label;
            if(label.text == null) label = GUIContent.none;
            EditorGUI.PropertyField(position, property, label);
        }
    }
}