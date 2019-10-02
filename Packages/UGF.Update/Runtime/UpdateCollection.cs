using System;
using System.Collections;
using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    public abstract class UpdateCollection<TItem> : IUpdateCollection<TItem>
    {
        public int Count { get { return Collection.Count; } }
        public IUpdateQueue<TItem> Queue { get; }

        protected ICollection<TItem> Collection { get; }

        IUpdateQueue IUpdateCollection.Queue { get { return Queue; } }

        protected UpdateCollection(ICollection<TItem> collection, IUpdateQueue<TItem> queue)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
            Queue = queue ?? throw new ArgumentNullException(nameof(queue));
        }

        public bool Contains(TItem item)
        {
            return Collection.Contains(item);
        }

        public void Add(TItem item)
        {
            Queue.Add.Add(item);
            Queue.Remove.Remove(item);
        }

        public void Remove(TItem item)
        {
            Queue.Add.Remove(item);
            Queue.Remove.Add(item);
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
