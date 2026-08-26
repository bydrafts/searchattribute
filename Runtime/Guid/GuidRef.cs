using System;
using System.IO;
using Drafts.SaveData;
using UnityEngine;

namespace Drafts
{
    public interface IGuidRef
    {
        Guid Guid { get; set; }
        Type ComponentType { get; }
    }

    [Serializable]
    public class GuidRef<T> : IGuidRef, IBinarySave where T : Component
    {
        private Guid? _guid;
        private T _value;
        public T Value => _value ??= GuidComponent.Get<T>(Guid);
        Type IGuidRef.ComponentType => typeof(T);

        public Guid Guid
        {
            get => _guid ??= serializedGuid is { Length: 16 }
                ? new Guid(serializedGuid)
                : Guid.Empty;
            set {
                _guid = value;
                _value = null;
                serializedGuid = _guid?.ToByteArray();
            }
        }

        [SerializeField, HideInInspector] private byte[] serializedGuid;
        public void Save(BinaryWriter writer) => writer.Write(serializedGuid);
        public void Load(BinaryReader reader)
        {
            _guid = null;
            _value = null;
            serializedGuid = reader.ReadBytes(16);
        }
    }
}