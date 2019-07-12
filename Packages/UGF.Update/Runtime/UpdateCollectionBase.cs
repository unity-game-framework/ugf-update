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
        public int Count { get { return m_handlers.Count; } }
        public bool AnyQueued { get { return m_queueAdd.Count > 0 || m_queueRemove.Count > 0; } }
        public int QueueAddCount { get { return m_queueAdd.Count; } }
        public int QueueRemoveCount { get { return m_queueRemove.Count; } }
        public IEqualityComparer<THandler> Comparer { get { return m_handlers.Comparer; } }

        /// <summary>
        /// Gets the enumerable of the all handlers queued to add into the collection of active handlers.
        /// </summary>
        public QueueEnumerable QueueAdd { get { return m_queueAddEnumerable ?? (m_queueAddEnumerable = new QueueEnumerable(m_queueAdd)); } }

        /// <summary>
        /// Gets the enumerable of the all handlers queued to remove from the collection of active handlers.
        /// </summary>
        public QueueEnumerable QueueRemove { get { return m_queueRemoveEnumerable ?? (m_queueRemoveEnumerable = new QueueEnumerable(m_queueRemove)); } }

        IEnumerable<THandler> IUpdateCollection<THandler>.QueueAdd { get { return m_queueAdd; } }
        IEnumerable<THandler> IUpdateCollection<THandler>.QueueRemove { get { return m_queueRemove; } }

        private readonly HashSet<THandler> m_handlers;
        private readonly HashSet<THandler> m_queueAdd;
        private readonly HashSet<THandler> m_queueRemove;
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
            m_handlers = new HashSet<THandler>(comparer);
            m_queueAdd = new HashSet<THandler>(comparer);
            m_queueRemove = new HashSet<THandler>(comparer);
        }

        public abstract void Update();

        public bool Contains(THandler handler)
        {
            return m_handlers.Contains(handler);
        }

        public bool ContainsQueueAdd(THandler handler)
        {
            return m_queueAdd.Contains(handler);
        }

        public bool ContainsQueueRemove(THandler handler)
        {
            return m_queueRemove.Contains(handler);
        }

        public bool Add(THandler handler)
        {
            return m_queueAdd.Add(handler);
        }

        public bool Remove(THandler handler)
        {
            return m_queueRemove.Add(handler);
        }

        public bool ApplyQueue()
        {
            bool changed = false;

            foreach (THandler handler in m_queueAdd)
            {
                m_handlers.Add(handler);

                changed = true;
            }

            foreach (THandler handler in m_queueRemove)
            {
                m_handlers.Remove(handler);

                changed = true;
            }

            ClearQueue();

            return changed;
        }

        public bool ApplyQueueAndUpdate()
        {
            bool changed = false;

            if (m_queueAdd.Count > 0 || m_queueRemove.Count > 0)
            {
                changed = ApplyQueue();
            }

            Update();

            return changed;
        }

        public void ClearQueue()
        {
            m_queueAdd.Clear();
            m_queueRemove.Clear();
        }

        public void Clear()
        {
            m_handlers.Clear();

            ClearQueue();
        }

        public HashSet<THandler>.Enumerator GetEnumerator()
        {
            return m_handlers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)m_handlers).GetEnumerator();
        }

        IEnumerator<THandler> IEnumerable<THandler>.GetEnumerator()
        {
            return ((IEnumerable<THandler>)m_handlers).GetEnumerator();
        }
    }
}
