using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Drafts
{
    public class GuidSearchSettings : ISearchSettings<KeyValuePair<Guid, GameObject>>
    {
        public string Title => "Loaded Guids";

        private static readonly KeyValuePair<Guid, GameObject> Null = new(Guid.Empty, null);

        public IEnumerable<KeyValuePair<Guid, GameObject>> GetItems(object type)
        {
            if (type is not Type t) return GuidComponent.Loaded.Prepend(Null);
            return GuidComponent.Loaded.Where(p => p.Value.GetComponent(t)).Prepend(Null);
        }

        public string GetName(KeyValuePair<Guid, GameObject> pair) => pair.Value?.name ?? "Null";
    }
}