using System;
using System.Collections;
using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update queue set.
    /// </summary>
    public class UpdateQueueSet<TItem> : IUpdateQueue<TItem>
    {
        public bool AnyQueued { get { return m_add.Count > 0 || m_remove.Count > 0; } }

        /// <summary>
        /// Gets the collection of the objects queued to add.
        /// </summary>
        public HashSet<TItem> Add { get { return m_add; } }

        /// <summary>
        /// Gets the collection of the objects queued to remove.
        /// </summary>
        public HashSet<TItem> Remove { get { return m_remove; } }

        ICollection<TItem> IUpdateQueue<TItem>.Add { get { return Add; } }
        ICollection<TItem> IUpdateQueue<TItem>.Remove { get { return Remove; } }
        IEnumerable IUpdateQueue.Add { get { return Add; } }
        IEnumerable IUpdateQueue.Remove { get { return Remove; } }

        private readonly HashSet<TItem> m_add = new HashSet<TItem>();
        private readonly HashSet<TItem> m_remove = new HashSet<TItem>();

        public bool Apply(ICollection<TItem> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            if (AnyQueued)
            {
                foreach (TItem item in m_add)
                {
                    collection.Add(item);
                }

                foreach (TItem item in m_remove)
                {
                    collection.Remove(item);
                }

                m_add.Clear();
                m_remove.Clear();

                return true;
            }

            return false;
        }

        public void Clear()
        {
            Add.Clear();
            Remove.Clear();
        }

        bool IUpdateQueue.Apply(ICollection collection)
        {
            return Apply((ICollection<TItem>)collection);
        }
    }
}
