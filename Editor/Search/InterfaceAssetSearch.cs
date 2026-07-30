using System;
using UnityEditor;
#if UNITY_EDITOR
using Drafts.Editor;
using UnityEngine;
#endif

namespace Drafts
{
    public class InterfaceAssetSearch
    {
#if UNITY_EDITOR
        private readonly InterfaceAssetSearchSettingsBase _settings = new(null);
        private readonly GenericMenu _menu;
        private Vector2 _pos;
        private Action<UnityEngine.Object> _onSelect;

        public InterfaceAssetSearch()
        {
            _menu = new GenericMenu();
            foreach (AssetSearchScope scope in Enum.GetValues(typeof(AssetSearchScope)))
                _menu.AddItem(new(scope.ToString()), false, Select, scope);
        }

        private void Select(object o) => SearchProvider.Open(_pos, _settings, o, _onSelect);

        public void Search(Type interfaceType, Action<UnityEngine.Object> onSelect)
        {
            _pos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            _settings.Type = interfaceType;
            _onSelect = onSelect;
            _menu.ShowAsContext();
        }
#endif
    }
}