using UnityEngine;

namespace BlueGravity
{
    public class PrefabAttribute : PropertyAttribute
    {
        public bool Lock { get; }
        public string Folder { get; }

        public PrefabAttribute(string folder = "Assets", bool @lock = false)
        {
            Folder = folder;
            Lock = @lock;
        }
        
        // public PrefabAttribute(Type type, string folder = "Assets", bool @lock = false)
        //     : base(new PrefabSearchSettings(type, folder), @lock) { }
    }
}