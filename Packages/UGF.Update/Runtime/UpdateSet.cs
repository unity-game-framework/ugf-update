using System.Collections;
using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update collection as unordered set of the items where each of them implements 'IUpdateHandler' interface.
    /// </summary>
    public class UpdateSet<TItem> : IUpdateCollection<TItem> where TItem : IUpdateHandler
    {
        public int Count { get { return m_items.Count; } }

        /// <summary>
        /// Gets the queue of the collection.
        /// </summary>
        public UpdateQueueSet<TItem> Queue { get; } = new UpdateQueueSet<TItem>();

        IUpdateQueue<TItem> IUpdateCollection<TItem>.Queue { get { return Queue; } }
        IUpdateQueue IUpdateCollection.Queue { get { return Queue; } }

        private readonly HashSet<TItem> m_items = new HashSet<TItem>();

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
                item.OnUpdate();
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
