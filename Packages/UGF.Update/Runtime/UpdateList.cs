using System.Collections;
using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    public class UpdateList<TItem> : IUpdateCollection<TItem> where TItem : IUpdateHandler
    {
        public int Count { get { return m_items.Count; } }
        public TItem this[int index] { get { return m_items[index]; } }
        public UpdateQueueSet<TItem> Queue { get; } = new UpdateQueueSet<TItem>();

        IUpdateQueue<TItem> IUpdateCollection<TItem>.Queue { get { return Queue; } }
        IUpdateQueue IUpdateCollection.Queue { get { return Queue; } }

        private readonly List<TItem> m_items = new List<TItem>();

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
                m_items[i].OnUpdate();
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
