using UnityEngine;

namespace Drafts
{
    public class TypeInstanceAttribute : PropertyAttribute
    {
        public bool SingleLine { get; }
        public float LabelWidthMult { get; }

        public TypeInstanceAttribute(bool singleLine = false, float labelWidthMult = 1f)
        {
            LabelWidthMult = labelWidthMult;
            SingleLine = singleLine;
        }
    }
}