using System.Collections;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents an update queue.
    /// </summary>
    public interface IUpdateQueue
    {
        /// <summary>
        /// Gets the value that determines whether anythings queued.
        /// </summary>
        bool AnyQueued { get; }

        /// <summary>
        /// Gets the collection of the objects queued to add.
        /// </summary>
        IEnumerable Add { get; }

        /// <summary>
        /// Gets the collection of the objects queued to remove.
        /// </summary>
        IEnumerable Remove { get; }

        /// <summary>
        /// Applies queue to the specified collection.
        /// </summary>
        /// <param name="collection">The collection to change.</param>
        bool Apply(ICollection collection);

        /// <summary>
        /// Clears queue.
        /// </summary>
        void Clear();
    }
}
