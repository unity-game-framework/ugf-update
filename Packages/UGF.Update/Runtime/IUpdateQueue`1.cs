using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    public interface IUpdateQueue<TItem> : IUpdateQueue
    {
        new ICollection<TItem> Add { get; }
        new ICollection<TItem> Remove { get; }

        bool Apply(ICollection<TItem> collection);
    }
}
