using System;
using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update collection as unordered set with the specified handler to update each item.
    /// </summary>
    public class UpdateSetHandler<TItem> : UpdateCollection<TItem> where TItem : class
    {
        public UpdateHandler<TItem> Handler { get; }

        private readonly HashSet<TItem> m_items;

        /// <summary>
        /// Creates update set with the specified handler and UpdateQueueSet queue.
        /// </summary>
        /// <param name="handler">The handler to update each item.</param>
        public UpdateSetHandler(UpdateHandler<TItem> handler) : this(handler, new UpdateQueueSet<TItem>())
        {
        }

        /// <summary>
        /// Creates update set with the specified handler and update queue.
        /// </summary>
        /// <param name="handler">The handler to update each item.</param>
        /// <param name="queue">The update queue.</param>
        public UpdateSetHandler(UpdateHandler<TItem> handler, IUpdateQueue<TItem> queue) : base(new HashSet<TItem>(), queue)
        {
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));

            m_items = (HashSet<TItem>)Collection;
        }

        public override void Update()
        {
            foreach (TItem item in m_items)
            {
                Handler(item);
            }
        }

        public HashSet<TItem>.Enumerator GetEnumerator()
        {
            return m_items.GetEnumerator();
        }
    }
}
