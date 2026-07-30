using UnityEditor;
using UnityEngine;

namespace Drafts.Editor
{
    [CustomPropertyDrawer(typeof(LabelAttribute), true)]
    public class LabelAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = (LabelAttribute)attribute;
            var lw = EditorGUIUtility.labelWidth;

            if (attr.Fit)
            {
                label.text = attr.Label ?? label.text;
                EditorGUIUtility.labelWidth = EditorStyles.label.CalcSize(label).x;
            }
            else
            {
                label.text = attr.Label;
                label = attr.Label == null ? GUIContent.none : label;
            }
            
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUIUtility.labelWidth = lw;
        }
    }
}