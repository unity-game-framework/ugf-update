using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    public interface IUpdateCollection<THandler> : IUpdateCollection, IEnumerable<THandler>
    {
        IEnumerable<THandler> QueueAdd { get; }
        IEnumerable<THandler> QueueRemove { get; }
        IEqualityComparer<THandler> Comparer { get; }

        bool Contains(THandler handler);
        bool ContainsQueueAdd(THandler handler);
        bool ContainsQueueRemove(THandler handler);
        bool Add(THandler handler);
        bool Remove(THandler handler);
    }
}
