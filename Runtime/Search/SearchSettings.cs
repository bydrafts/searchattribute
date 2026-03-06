using System;
using System.Collections.Generic;

namespace Drafts
{
    public class SearchSettings<T> : ISearchSettings<T>
    {
        public string Title { get; }
        private Func<object, IEnumerable<T>> _getItems { get; }
        private Func<T, string> _getName { get; }

        public IEnumerable<T> GetItems(object target) => _getItems(target);
        public string GetName(T obj) => _getName(obj);

        public SearchSettings(string title, Func<object, IEnumerable<T>> getItems, Func<T, string> getName = null)
        {
            Title = title;
            _getItems = getItems;
            _getName = getName ?? (o => o.ToString());
        }
    }
}