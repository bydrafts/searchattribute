using UnityEditor;
using UnityEngine;

namespace Drafts.Editor
{
    [CustomPropertyDrawer(typeof(SingleLineAttribute), true)]
    [CustomPropertyDrawer(typeof(ISingleLineDrawer), true)]
    public class SingleLineAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            var contentRect = EditorGUI.PrefixLabel(position, label);
            var indent = EditorGUI.indentLevel;
            var labelWidth = EditorGUIUtility.labelWidth;
            EditorGUI.indentLevel = 0;

            var child = property.Copy();
            var endProperty = child.GetEndProperty();

            var count = 0;
            if (child.NextVisible(true))
            {
                var counter = child.Copy();
                do count++;
                while (counter.NextVisible(false) && !SerializedProperty.EqualContents(counter, endProperty));

                var spacing = 2f;
                var width = (contentRect.width - spacing * (count - 1)) / count;
                var currentRect = contentRect;
                currentRect.width = width;

                do
                {
                    EditorGUIUtility.labelWidth = width * 0.4f;
                    EditorGUI.PropertyField(currentRect, child);

                    currentRect.x += currentRect.width + spacing;
                } while (child.NextVisible(false) && !SerializedProperty.EqualContents(child, endProperty));
            }

            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}