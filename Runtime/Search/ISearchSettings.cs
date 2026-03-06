using System.Collections;
using System.Collections.Generic;

namespace Drafts
{
    public interface ISearchSettings
    {
        string Title { get; }
        IEnumerable GetItems(object target);
        string GetName(object item);
    }

    public interface ISearchSettings<T> : ISearchSettings
    {
        new IEnumerable<T> GetItems(object target);
        string GetName(T obj);

        IEnumerable ISearchSettings.GetItems(object target) => GetItems(target);
        string ISearchSettings.GetName(object item) => GetName((T)item);
    }
}