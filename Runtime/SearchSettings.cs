using System;
using System.Collections.Generic;

namespace BlueGravity
{
    public class SearchSettings<T> : ISearchSettings<T>
    {
        public string Title { get; }
        protected Func<object, IEnumerable<T>> _getItens { get; }
        protected Func<T, string> _getName { get; } = o => o.ToString();

        public IEnumerable<T> GetItens(object target) => _getItens(target);
        public string GetName(T obj) => _getName(obj);

        public SearchSettings(string title, Func<object, IEnumerable<T>> getItens, Func<T, string> getName = null)
        {
            Title = title;
            _getItens = getItens;

            if (getName == null)
                _getName = o => o.ToString();
            else
                _getName = o => getName((T)o);
        }
    }
}