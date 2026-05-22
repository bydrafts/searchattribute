using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Drafts.Editor {
    [CustomPropertyDrawer(typeof(TwoColumnsAttribute), true)]
    [CustomPropertyDrawer(typeof(ITwoColumnsDrawer), true)]
    public class TwoColumnsAttributeDrawer : ColumnsAttributeDrawer {
        protected override int Columns => 2;
    }

    [CustomPropertyDrawer(typeof(ThreeColumnsAttribute), true)]
    [CustomPropertyDrawer(typeof(IThreeColumnsDrawer), true)]
    public class ThreeColumnsAttributeDrawer : ColumnsAttributeDrawer {
        protected override int Columns => 3;
    }

    [CustomPropertyDrawer(typeof(FourColumnsAttribute), true)]
    [CustomPropertyDrawer(typeof(IFourColumnsDrawer), true)]
    public class FourColumnsAttributeDrawer : ColumnsAttributeDrawer {
        protected override int Columns => 4;
    }

    public abstract class ColumnsAttributeDrawer : PropertyDrawer {
        protected abstract int Columns { get; }
        private static readonly Dictionary<System.Type, int> _countCache = new ();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (property.isArray) {
                EditorGUI.HelpBox(position, "ColumnsAttribute does not support arrays.", MessageType.Warning);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            position.height = EditorGUIUtility.singleLineHeight;
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true);

            if (property.isExpanded) {
                EditorGUI.indentLevel++;
                EditorGUIUtility.labelWidth /= Columns;

                var child = property.Copy();
                var endProperty = child.GetEndProperty();
                child.NextVisible(true);

                var columnSpacing = 2;
                var currentRect = EditorGUI.IndentedRect(position);
                currentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                var columnWidth = (currentRect.width - columnSpacing * (Columns - 1)) / Columns;

                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                var columnIndex = 0;
                while (!SerializedProperty.EqualContents(child, endProperty)) {
                    var drawRect = currentRect;
                    drawRect.width = columnWidth;
                    drawRect.x += columnIndex * (columnWidth + columnSpacing);

                    EditorGUI.PropertyField(drawRect, child);

                    columnIndex++;
                    if (columnIndex >= Columns) {
                        columnIndex = 0;
                        currentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    }

                    if (!child.NextVisible(false)) break;
                }

                EditorGUI.indentLevel = indent;

                EditorGUIUtility.labelWidth *= Columns;
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (property.isArray) return EditorGUIUtility.singleLineHeight * 2;
            if (!property.isExpanded) return EditorGUIUtility.singleLineHeight;
            var type = fieldInfo.FieldType;

            if (!_countCache.TryGetValue(type, out var count)) {
                count = 0;
                var child = property.Copy();
                var endProperty = child.GetEndProperty();
                if (child.NextVisible(true)) {
                    do count++;
                    while (child.NextVisible(false) && !SerializedProperty.EqualContents(child, endProperty));
                }
                _countCache[type] = count;
            }

            var rows = Mathf.CeilToInt((float)count / Columns);
            return EditorGUIUtility.singleLineHeight + rows * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
        }
    }
}