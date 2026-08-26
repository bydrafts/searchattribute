#if UNITY_EDITOR
using UnityEditor;

namespace Drafts.Editor
{
    [CustomEditor(typeof(GuidComponent))]
    public class GuidComponentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("guid", ((GuidComponent)target).Guid.ToString());
        }
    }
     
}
#endif