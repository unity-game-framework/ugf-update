using System.Collections;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update collection with add and remove queue.
    /// </summary>
    public interface IUpdateCollection : IEnumerable
    {
        /// <summary>
        /// Gets the count of the items executed in update collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the queue of the collection.
        /// </summary>
        IUpdateQueue Queue { get; }

        /// <summary>
        /// Determines whether the specified item exists and executed in collection.
        /// </summary>
        /// <param name="item">The item to check.</param>
        bool Contains(object item);

        /// <summary>
        /// Adds the specified item to add queue.
        /// </summary>
        /// <param name="item">The item to add.</param>
        void Add(object item);

        /// <summary>
        /// Adds the specified item to remove queue.
        /// </summary>
        /// <param name="item">The item to add.</param>
        bool Remove(object item);

        /// <summary>
        /// Updates collection.
        /// </summary>
        void Update();

        /// <summary>
        /// Applies queue to collection.
        /// </summary>
        bool ApplyQueue();

        /// <summary>
        /// Clears queue and collection.
        /// </summary>
        void Clear();
    }
}
