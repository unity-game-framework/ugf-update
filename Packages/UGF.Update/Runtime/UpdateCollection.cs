using System;
using System.Collections;
using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    public class UpdateCollection<THandler> : IUpdateCollection<THandler> where THandler : IUpdateHandler
    {
        public Type HandlerType { get; } = typeof(THandler);
        public int Count { get { return m_handlers.Count; } }
        public bool AnyQueued { get { return m_queueAdd.Count > 0 || m_queueRemove.Count > 0; } }
        public IEqualityComparer<THandler> Comparer { get { return m_handlers.Comparer; } }
        public HashSet<THandler>.Enumerator QueueAdd { get { return m_queueAdd.GetEnumerator(); } }
        public HashSet<THandler>.Enumerator QueueRemove { get { return m_queueRemove.GetEnumerator(); } }

        IEnumerable<THandler> IUpdateCollection<THandler>.QueueAdd { get { return m_queueAdd; } }
        IEnumerable<THandler> IUpdateCollection<THandler>.QueueRemove { get { return m_queueRemove; } }

        private readonly HashSet<THandler> m_handlers;
        private readonly HashSet<THandler> m_queueAdd;
        private readonly HashSet<THandler> m_queueRemove;

        public UpdateCollection(IEqualityComparer<THandler> comparer = null)
        {
            m_handlers = new HashSet<THandler>(comparer);
            m_queueAdd = new HashSet<THandler>(comparer);
            m_queueRemove = new HashSet<THandler>(comparer);
        }

        public bool Contains(THandler handler)
        {
            return m_handlers.Contains(handler) || m_queueAdd.Contains(handler);
        }

        public bool Add(THandler handler)
        {
            return m_queueAdd.Add(handler);
        }

        public bool Remove(THandler handler)
        {
            return m_queueRemove.Add(handler);
        }

        public void Update()
        {
            foreach (THandler handler in m_handlers)
            {
                handler.OnUpdate();
            }
        }

        public void ApplyQueue()
        {
            foreach (THandler handler in m_queueAdd)
            {
                m_handlers.Add(handler);
            }

            foreach (THandler handler in m_queueRemove)
            {
                m_handlers.Remove(handler);
            }

            ClearQueue();
        }

        public void ApplyQueueAndUpdate()
        {
            if (m_queueAdd.Count > 0 || m_queueRemove.Count > 0)
            {
                ApplyQueue();
            }

            Update();
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
