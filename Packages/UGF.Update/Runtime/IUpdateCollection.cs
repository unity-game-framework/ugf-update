using System.Collections;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update collection with add and remove queue.
    /// </summary>
    public interface IUpdateCollection : IEnumerable
    {
        /// <summary>
        /// Gets the count of the items in collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the queue of the collection.
        /// </summary>
        IUpdateQueue Queue { get; }

        /// <summary>
        /// Updates collection.
        /// </summary>
        void Update();

        /// <summary>
        /// Applies queue to collection.
        /// </summary>
        bool ApplyQueue();

        /// <summary>
        /// Applies queue and updates collection.
        /// </summary>
        bool ApplyQueueAndUpdate();

        /// <summary>
        /// Clears collection.
        /// </summary>
        void Clear();
    }
}
