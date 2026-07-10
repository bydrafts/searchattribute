using UnityEngine;

namespace Drafts
{
    public class LabelAttribute : PropertyAttribute
    {
        public string Label { get; }
        public bool Fit { get; }

        public LabelAttribute(bool fit) => Fit = fit;

        public LabelAttribute(string label = null, bool fit = false)
        {
            Label = label;
            Fit = fit;
        }
    }
}