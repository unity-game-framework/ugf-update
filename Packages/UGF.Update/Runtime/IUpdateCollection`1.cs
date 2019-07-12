using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents collection with explicit typed items which need to be handle update.
    /// </summary>
    public interface IUpdateCollection<THandler> : IUpdateCollection, IEnumerable<THandler>
    {
        /// <summary>
        /// Gets the enumerable of the all handlers queued to add into the collection of active handlers.
        /// </summary>
        IEnumerable<THandler> QueueAdd { get; }

        /// <summary>
        /// Gets the enumerable of the all handlers queued to remove from the collection of active handlers.
        /// </summary>
        IEnumerable<THandler> QueueRemove { get; }

        /// <summary>
        /// Gets the comparer of the handlers.
        /// </summary>
        IEqualityComparer<THandler> Comparer { get; }

        /// <summary>
        /// Determines whether the collection of active handlers contains the specified handler.
        /// </summary>
        /// <param name="handler">The handler to check.</param>
        bool Contains(THandler handler);

        /// <summary>
        /// Determines whether the queue to add contains the specified handler.
        /// </summary>
        /// <param name="handler">The handler to check.</param>
        bool ContainsQueueAdd(THandler handler);

        /// <summary>
        /// Determines whether the queue to remove contains the specified handler.
        /// </summary>
        /// <param name="handler">The handler to check.</param>
        bool ContainsQueueRemove(THandler handler);

        /// <summary>
        /// Adds the specified handler to add queue.
        /// </summary>
        /// <param name="handler">The handler to add.</param>
        bool Add(THandler handler);

        /// <summary>
        /// Add the specified handler to remove queue.
        /// </summary>
        /// <param name="handler">The handler to add.</param>
        bool Remove(THandler handler);
    }
}
