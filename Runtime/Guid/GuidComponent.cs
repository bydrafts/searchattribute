using System;
using System.Collections.Generic;
using UnityEngine;

namespace Drafts
{
    /// <summary>Creates unique Guid for a GameObject, can be accessed cross scene if both cenes are loaded</summary>
    [ExecuteAlways, DisallowMultipleComponent]
    public class GuidComponent : MonoBehaviour, ISerializationCallbackReceiver
    {
        private static readonly Dictionary<Guid, GameObject> Instances = new();
        public static IReadOnlyDictionary<Guid, GameObject> Loaded => Instances;
        public static GameObject Get(Guid guid) => Instances.GetValueOrDefault(guid);
        public static T Get<T>(Guid guid) where T : Component => Get(guid)?.GetComponent<T>();

        private Guid _guid = Guid.Empty;
        public Guid Guid => _guid;
        [SerializeField, HideInInspector] private byte[] serializedGuid;

        private void Awake()
        {
            CreateGuidIfInvalid();
            if (_guid == Guid.Empty) return;
            if (Instances.TryAdd(_guid, gameObject)) return;
            Debug.LogError("Duplicated GUID detected", gameObject);
            Debug.LogError("Duplicated GUID detected", Instances[_guid]);
        }

        public void OnDestroy() => Instances.Remove(_guid);
        private void OnValidate() => CreateGuidIfInvalid();

        public void OnBeforeSerialize()
        {
            if (_guid != Guid.Empty)
                serializedGuid = _guid.ToByteArray();
        }

        public void OnAfterDeserialize()
        {
            if (serializedGuid is { Length: 16 })
                _guid = new Guid(serializedGuid);
        }

        private void CreateGuidIfInvalid()
        {
            if (serializedGuid is { Length: 16 } && _guid != Guid.Empty)
                return;

            _guid = Guid.NewGuid();
            serializedGuid = _guid.ToByteArray();
        }

        public Guid GetGuid()
        {
            if (_guid == Guid.Empty && serializedGuid is { Length: 16 })
                _guid = new Guid(serializedGuid);

            return _guid;
        }
    }
}