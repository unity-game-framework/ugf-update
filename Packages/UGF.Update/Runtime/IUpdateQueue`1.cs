using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents an update queue with the specified type of the item.
    /// </summary>
    public interface IUpdateQueue<TItem> : IUpdateQueue
    {
        /// <summary>
        /// Gets the collection of the objects queued to add.
        /// </summary>
        new ICollection<TItem> Add { get; }

        /// <summary>
        /// Gets the collection of the objects queued to remove.
        /// </summary>
        new ICollection<TItem> Remove { get; }

        /// <summary>
        /// Applies queue to the specified collection.
        /// </summary>
        /// <param name="collection">The collection to change.</param>
        bool Apply(ICollection<TItem> collection);
    }
}
