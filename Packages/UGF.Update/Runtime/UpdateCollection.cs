using System;
using System.Collections;
using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents abstract implementation of the update collection.
    /// </summary>
    public abstract class UpdateCollection<TItem> : IUpdateCollection<TItem> where TItem : class
    {
        public int Count { get { return Collection.Count; } }
        public IUpdateQueue<TItem> Queue { get; }

        /// <summary>
        /// Gets internal collection with items.
        /// </summary>
        protected ICollection<TItem> Collection { get; }

        IUpdateQueue IUpdateCollection.Queue { get { return Queue; } }

        /// <summary>
        /// Creates update collection with the specified items collection and update queue.
        /// </summary>
        /// <param name="collection">The collection used to store and update items.</param>
        /// <param name="queue">The update queue used to safely change collection during update.</param>
        protected UpdateCollection(ICollection<TItem> collection, IUpdateQueue<TItem> queue)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
            Queue = queue ?? throw new ArgumentNullException(nameof(queue));
        }

        public bool Contains(TItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return Collection.Contains(item);
        }

        public void Add(TItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            Queue.Add.Add(item);
            Queue.Remove.Remove(item);
        }

        public bool Remove(TItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (Queue.Add.Remove(item))
            {
                Queue.Remove.Add(item);
                return true;
            }

            return false;
        }

        public void Clear()
        {
            Collection.Clear();
            Queue.Clear();
        }

        public abstract void Update();

        public bool ApplyQueue()
        {
            return Queue.Apply(Collection);
        }

        public bool ApplyQueueAndUpdate()
        {
            bool changed = ApplyQueue();

            Update();

            return changed;
        }

        bool IUpdateCollection.Contains(object item)
        {
            return Contains((TItem)item);
        }

        void IUpdateCollection.Add(object item)
        {
            Add((TItem)item);
        }

        bool IUpdateCollection.Remove(object item)
        {
            return Remove((TItem)item);
        }

        IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        {
            return Collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Collection).GetEnumerator();
        }
    }
}
