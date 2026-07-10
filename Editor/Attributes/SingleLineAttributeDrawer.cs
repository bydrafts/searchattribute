using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Drafts.Editor
{
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

            var config = (property.managedReferenceValue as ISingleLineDrawer)?.DrawConfig;
            if (config == null) EqualWidth(property, contentRect);
            else DrawWithConfig(property, contentRect, config);

            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        private static void DrawWithConfig(SerializedProperty property, Rect contentRect, (string n, float w)[] config)
        {
            var child = property.Copy();
            var endProperty = child.GetEndProperty();
            if (!child.NextVisible(true)) return;

            var count = 0;
            var counter = child.Copy();

            do count++;
            while (counter.NextVisible(false) && !SerializedProperty.EqualContents(counter, endProperty));
            var sum = config.Sum(t => t.w);

            const float spacing = 2f;
            var width = contentRect.width - spacing * (count - 1);
            var rect = contentRect;
            var style = EditorStyles.label;

            for (var i = 0; i < count; i++)
            {
                var text = config[i].n == "" ? child.displayName : config[i].n;
                var content = new GUIContent(text);
                var weight = config[i].w;
                rect.width = weight / sum * width;
                EditorGUIUtility.labelWidth = style.CalcSize(content).x;
                EditorGUI.PropertyField(rect, child, content);
                rect.x += rect.width;
                child.NextVisible(false);
            }
        }

        private static void EqualWidth(SerializedProperty property, Rect contentRect)
        {
            var child = property.Copy();
            var endProperty = child.GetEndProperty();
            if (!child.NextVisible(true)) return;

            var count = 0;
            var counter = child.Copy();
            do count++;
            while (counter.NextVisible(false) && !SerializedProperty.EqualContents(counter, endProperty));

            const float spacing = 2f;
            var width = (contentRect.width - spacing * (count - 1)) / count;
            var currentRect = contentRect;
            currentRect.width = width;
            var style = EditorStyles.label;

            do
            {
                var content = new GUIContent(child.displayName);
                EditorGUIUtility.labelWidth = style.CalcSize(content).x;
                EditorGUI.PropertyField(currentRect, child);

                currentRect.x += currentRect.width + spacing;
            } while (child.NextVisible(false) && !SerializedProperty.EqualContents(child, endProperty));
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}