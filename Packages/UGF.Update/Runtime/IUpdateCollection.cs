using System;
using System.Collections;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents collection with items which need to be handle update.
    /// </summary>
    public interface IUpdateCollection : IEnumerable
    {
        /// <summary>
        /// Gets the type of the handlers in collection.
        /// </summary>
        Type HandlerType { get; }

        /// <summary>
        /// Gets the count of active handlers in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the value that determines whether collection contains any queued handlers.
        /// </summary>
        bool AnyQueued { get; }

        /// <summary>
        /// Gets the count of the handlers queued to add into the collection of active handlers.
        /// </summary>
        int QueueAddCount { get; }

        /// <summary>
        /// Gets the count of the handlers queued to remove from the collection of active handlers.
        /// </summary>
        int QueueRemoveCount { get; }

        /// <summary>
        /// Updates all handlers in collection of active handlers.
        /// </summary>
        void Update();

        /// <summary>
        /// Applies all queued handlers into collection of active handlers and clears all queues.
        /// <para>
        /// Returns true if the were any applied handler, otherwise false.
        /// </para>
        /// </summary>
        bool ApplyQueue();

        /// <summary>
        /// Applies all queued handlers if required and than forces update.
        /// <para>
        /// Returns true if the were any applied handler, otherwise false.
        /// </para>
        /// </summary>
        bool ApplyQueueAndUpdate();

        /// <summary>
        /// Clears all queued handlers from add and remove queues.
        /// </summary>
        void ClearQueue();

        /// <summary>
        /// Clears all active handlers and all queues.
        /// </summary>
        void Clear();
    }
}
