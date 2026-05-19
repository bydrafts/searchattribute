using UnityEngine;

namespace Drafts {
    public class LabelAttribute : PropertyAttribute {
        public string Label { get; }
        public LabelAttribute(string label) => Label = label;
    }
}