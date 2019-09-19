using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    public interface IUpdateCollection<TItem> : IUpdateCollection, IEnumerable<TItem>
    {
        new IUpdateQueue<TItem> Queue { get; }

        bool Contains(TItem item);
        void Add(TItem item);
        void Remove(TItem item);
    }
}
