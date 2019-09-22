using System;
using System.Collections;
using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update collection as ordered list with the specified handler to update each item.
    /// </summary>
    public class UpdateListHandler<TItem> : IUpdateCollection<TItem>
    {
        public int Count { get { return m_items.Count; } }

        /// <summary>
        /// Gets the item by the specified index.
        /// </summary>
        /// <param name="index">The index of the item in collection.</param>
        public TItem this[int index] { get { return m_items[index]; } }

        /// <summary>
        /// Gets the queue of the collection.
        /// </summary>
        public UpdateQueueSet<TItem> Queue { get; } = new UpdateQueueSet<TItem>();

        IUpdateQueue<TItem> IUpdateCollection<TItem>.Queue { get { return Queue; } }
        IUpdateQueue IUpdateCollection.Queue { get { return Queue; } }

        private readonly UpdateHandler<TItem> m_handler;
        private readonly List<TItem> m_items = new List<TItem>();

        /// <summary>
        /// Creates collection with the specified handler to update each item.
        /// </summary>
        /// <param name="handler">The handler used to update each item in collection.</param>
        public UpdateListHandler(UpdateHandler<TItem> handler)
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
            for (int i = 0; i < m_items.Count; i++)
            {
                m_handler(m_items[i]);
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

        public List<TItem>.Enumerator GetEnumerator()
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
