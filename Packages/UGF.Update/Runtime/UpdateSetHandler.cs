using System;
using System.Collections;
using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update collection as unordered set with the specified handler to update each item.
    /// </summary>
    public class UpdateSetHandler<TItem> : IUpdateCollection<TItem>
    {
        public int Count { get { return m_items.Count; } }

        /// <summary>
        /// Gets the queue of the collection.
        /// </summary>
        public UpdateQueueSet<TItem> Queue { get; } = new UpdateQueueSet<TItem>();

        IUpdateQueue<TItem> IUpdateCollection<TItem>.Queue { get { return Queue; } }
        IUpdateQueue IUpdateCollection.Queue { get { return Queue; } }

        private readonly UpdateHandler<TItem> m_handler;
        private readonly HashSet<TItem> m_items = new HashSet<TItem>();

        /// <summary>
        /// Creates collection with the specified handler to update each item.
        /// </summary>
        /// <param name="handler">The handler used to update each item in collection.</param>
        public UpdateSetHandler(UpdateHandler<TItem> handler)
        {
            m_handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public bool Contains(TItem item)
        {
            return m_items.Contains(item);
        }

        public void Add(TItem item)
        {
            m_items.Add(item);
        }

        public void Remove(TItem item)
        {
            m_items.Remove(item);
        }

        public void Clear()
        {
            Queue.Clear();

            m_items.Clear();
        }

        public void Update()
        {
            foreach (TItem item in m_items)
            {
                m_handler(item);
            }
        }

        public bool ApplyQueue()
        {
            return Queue.Apply(m_items);
        }

        public bool ApplyQueueAndUpdate()
        {
            bool changed = ApplyQueue();

            Update();

            return changed;
        }

        public HashSet<TItem>.Enumerator GetEnumerator()
        {
            return m_items.GetEnumerator();
        }

        IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        {
            return ((IEnumerable<TItem>)m_items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)m_items).GetEnumerator();
        }
    }
}
