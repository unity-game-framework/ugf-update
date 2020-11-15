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
        public bool AnyQueued { get { return Add.Count > 0 || Remove.Count > 0; } }

        /// <summary>
        /// Gets the collection of the objects queued to add.
        /// </summary>
        public HashSet<TItem> Add { get; } = new HashSet<TItem>();

        /// <summary>
        /// Gets the collection of the objects queued to remove.
        /// </summary>
        public HashSet<TItem> Remove { get; } = new HashSet<TItem>();

        ICollection<TItem> IUpdateQueue<TItem>.Add { get { return Add; } }
        ICollection<TItem> IUpdateQueue<TItem>.Remove { get { return Remove; } }
        IEnumerable IUpdateQueue.Add { get { return Add; } }
        IEnumerable IUpdateQueue.Remove { get { return Remove; } }

        public bool Apply(ICollection<TItem> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            if (AnyQueued)
            {
                foreach (TItem item in Add)
                {
                    collection.Add(item);
                }

                foreach (TItem item in Remove)
                {
                    collection.Remove(item);
                }

                Add.Clear();
                Remove.Clear();

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
