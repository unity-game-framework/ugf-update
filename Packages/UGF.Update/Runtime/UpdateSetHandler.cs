using System.Collections;
using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    public class UpdateSetHandler<TItem> : IUpdateCollection<TItem>
    {
        public int Count { get { return m_items.Count; } }
        public UpdateQueueSet<TItem> Queue { get; } = new UpdateQueueSet<TItem>();

        IUpdateQueue<TItem> IUpdateCollection<TItem>.Queue { get { return Queue; } }
        IUpdateQueue IUpdateCollection.Queue { get { return Queue; } }

        private readonly UpdateHandler<TItem> m_item;
        private readonly HashSet<TItem> m_items = new HashSet<TItem>();

        public UpdateSetHandler(UpdateHandler<TItem> item)
        {
            m_item = item;
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
                m_item(item);
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
