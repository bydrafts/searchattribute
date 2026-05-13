using UnityEditor;
using UnityEngine;

namespace Drafts.Editor {
    [CustomPropertyDrawer(typeof(SeparatorAttribute), true)]
    public class SeparatorAttributeDrawer : DecoratorDrawer {
        public override void OnGUI(Rect position) {
            position.y += EditorGUIUtility.standardVerticalSpacing * 2;
            position.height = 1f;
            EditorGUI.DrawRect(position, Color.gray);
        }

        public override float GetHeight() {
            return EditorGUIUtility.singleLineHeight * 0.5f;
        }
    }
}