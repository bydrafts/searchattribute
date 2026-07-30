using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Drafts
{
    [CustomPropertyDrawer(typeof(UnityEventBase), true)]
    public class UnityEventDrawer : PropertyDrawer
    {
        private readonly UnityEditorInternal.UnityEventDrawer _drawer = new();

        private bool _expand ;
        
        private static bool HasPersistentListeners(SerializedProperty property)
        {
            var calls = property.FindPropertyRelative("m_PersistentCalls.m_Calls");
            return calls != null && calls.arraySize > 0;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            
            return !HasPersistentListeners(property) && !_expand
                ? EditorGUIUtility.singleLineHeight
                : _drawer.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var hasListeners = HasPersistentListeners(property);
            if (!hasListeners && !_expand)
            {
                if (GUI.Button(position, label))
                    _expand = true;
                return;
            }
            _drawer.OnGUI(position, property, label);
        }
    }
}