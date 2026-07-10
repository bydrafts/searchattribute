using UnityEditor;
using UnityEngine;

namespace Drafts.Editor
{
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = (LabelAttribute)attribute;
            var lw = EditorGUIUtility.labelWidth;

            if (attr.Fit)
            {
                label.text = attr.Label ?? label.text;
                EditorGUIUtility.labelWidth = EditorStyles.label.CalcSize(label).x;
            }
            else if (label.text == null) label = GUIContent.none;
            
            EditorGUI.PropertyField(position, property, label);
            EditorGUIUtility.labelWidth = lw;
        }
    }
}