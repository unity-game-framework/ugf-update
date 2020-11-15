using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update collection with the specified type of the item.
    /// </summary>
    public interface IUpdateCollection<TItem> : IUpdateCollection, IEnumerable<TItem> where TItem : class
    {
        /// <summary>
        /// Gets the queue of the collection.
        /// </summary>
        new IUpdateQueue<TItem> Queue { get; }

        /// <summary>
        /// Determines whether the specified item exists and executed in collection.
        /// </summary>
        /// <param name="item">The item to check.</param>
        bool Contains(TItem item);

        /// <summary>
        /// Adds the specified item to add queue.
        /// </summary>
        /// <param name="item">The item to add.</param>
        void Add(TItem item);

        /// <summary>
        /// Adds the specified item to remove queue.
        /// </summary>
        /// <param name="item">The item to add.</param>
        void Remove(TItem item);
    }
}
