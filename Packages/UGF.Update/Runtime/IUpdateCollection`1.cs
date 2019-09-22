using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update collection with the specified type of the item.
    /// </summary>
    public interface IUpdateCollection<TItem> : IUpdateCollection, IEnumerable<TItem>
    {
        /// <summary>
        /// Gets the queue of the collection.
        /// </summary>
        new IUpdateQueue<TItem> Queue { get; }

        /// <summary>
        /// Determines whether collection contains the specified item.
        /// </summary>
        /// <param name="item">The item to check.</param>
        bool Contains(TItem item);

        /// <summary>
        /// Adds the specified item to collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        void Add(TItem item);

        /// <summary>
        /// Removes the specified item from collection.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        void Remove(TItem item);
    }
}
