using UnityEditor;
using UnityEngine;

namespace Drafts.Editor {
    [CustomPropertyDrawer(typeof(TwoColumnsAttribute), true)]
    public class TwoColumnsAttributeDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            
            position.height = EditorGUIUtility.singleLineHeight;
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true);

            if (property.isExpanded) {
                EditorGUI.indentLevel++;
                
                var child = property.Copy();
                var endProperty = child.GetEndProperty();
                child.NextVisible(true);

                var isLeft = true;
                var currentRect = position;
                currentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                var halfWidth = (position.width - EditorGUIUtility.standardVerticalSpacing) / 2f;

                while (!SerializedProperty.EqualContents(child, endProperty)) {
                    var drawRect = currentRect;
                    drawRect.width = halfWidth;
                    if (!isLeft) {
                        drawRect.x += halfWidth + EditorGUIUtility.standardVerticalSpacing;
                    }

                    EditorGUI.PropertyField(drawRect, child);

                    if (!isLeft) {
                        currentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    }
                    
                    isLeft = !isLeft;
                    if (!child.NextVisible(false)) break;
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!property.isExpanded) {
                return EditorGUIUtility.singleLineHeight;
            }

            var count = 0;
            var child = property.Copy();
            var endProperty = child.GetEndProperty();
            child.NextVisible(true);
            while (!SerializedProperty.EqualContents(child, endProperty)) {
                count++;
                if (!child.NextVisible(false)) break;
            }

            var rows = Mathf.CeilToInt(count / 2f);
            return EditorGUIUtility.singleLineHeight + (rows * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing)) + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
