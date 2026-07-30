using UnityEngine;

namespace Drafts
{
    public class SingleLineAttribute : PropertyAttribute
    {
        public (string n, float w)[] DrawConfig { get; protected set; }
    }

    public interface ISingleLineDrawer
    {
        public (string n, float w)[] DrawConfig => null;
    }
}