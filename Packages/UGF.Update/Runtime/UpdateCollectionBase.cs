using System;
using System.Collections;
using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents an abstract base collection with items which need to be handle update.
    /// </summary>
    public abstract class UpdateCollectionBase<THandler> : IUpdateCollection<THandler>
    {
        public Type HandlerType { get; } = typeof(THandler);
        public int Count { get { return Collection.Count; } }
        public bool AnyQueued { get { return m_queue.AnyQueued; } }
        public int QueueAddCount { get { return m_queue.Add.Count; } }
        public int QueueRemoveCount { get { return m_queue.Remove.Count; } }
        public IEqualityComparer<THandler> Comparer { get { return m_queue.Comparer; } }

        /// <summary>
        /// Gets the enumerable of the all handlers queued to add into the collection of active handlers.
        /// </summary>
        public QueueEnumerable QueueAdd { get { return m_queueAddEnumerable ?? (m_queueAddEnumerable = new QueueEnumerable(m_queue.Add)); } }

        /// <summary>
        /// Gets the enumerable of the all handlers queued to remove from the collection of active handlers.
        /// </summary>
        public QueueEnumerable QueueRemove { get { return m_queueRemoveEnumerable ?? (m_queueRemoveEnumerable = new QueueEnumerable(m_queue.Remove)); } }

        protected UpdateQueue<THandler> Queue { get { return m_queue; } }
        protected abstract ICollection<THandler> Collection { get; }

        IEnumerable<THandler> IUpdateCollection<THandler>.QueueAdd { get { return m_queueAddEnumerable; } }
        IEnumerable<THandler> IUpdateCollection<THandler>.QueueRemove { get { return m_queueRemoveEnumerable; } }

        private readonly UpdateQueue<THandler> m_queue;
        private QueueEnumerable m_queueAddEnumerable;
        private QueueEnumerable m_queueRemoveEnumerable;

        public sealed class QueueEnumerable : IEnumerable<THandler>
        {
            private readonly HashSet<THandler> m_collection;

            public QueueEnumerable(HashSet<THandler> collection)
            {
                m_collection = collection ?? throw new ArgumentNullException(nameof(collection));
            }

            public HashSet<THandler>.Enumerator GetEnumerator()
            {
                return m_collection.GetEnumerator();
            }

            IEnumerator<THandler> IEnumerable<THandler>.GetEnumerator()
            {
                return ((IEnumerable<THandler>)m_collection).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)m_collection).GetEnumerator();
            }
        }

        /// <summary>
        /// Creates the update collection with the handler comparer, if specified.
        /// </summary>
        /// <param name="comparer">The handlers equality comparer.</param>
        protected UpdateCollectionBase(IEqualityComparer<THandler> comparer = null)
        {
            m_queue = new UpdateQueue<THandler>(comparer);
        }

        public abstract void Update();

        public virtual bool Contains(THandler handler)
        {
            return Collection.Contains(handler);
        }

        public bool ContainsQueueAdd(THandler handler)
        {
            return m_queue.Add.Contains(handler);
        }

        public bool ContainsQueueRemove(THandler handler)
        {
            return m_queue.Remove.Contains(handler);
        }

        public virtual bool Add(THandler handler)
        {
            return m_queue.Add.Add(handler);
        }

        public virtual bool Remove(THandler handler)
        {
            return m_queue.Remove.Add(handler);
        }

        public virtual bool ApplyQueue()
        {
            return Queue.Apply(Collection);
        }

        public bool ApplyQueueAndUpdate()
        {
            bool changed = false;

            if (m_queue.AnyQueued)
            {
                changed = ApplyQueue();
            }

            Update();

            return changed;
        }

        public void ClearQueue()
        {
            m_queue.Clear();
        }

        public virtual void Clear()
        {
            Collection.Clear();

            ClearQueue();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Collection.GetEnumerator();
        }

        IEnumerator<THandler> IEnumerable<THandler>.GetEnumerator()
        {
            return Collection.GetEnumerator();
        }
    }
}
