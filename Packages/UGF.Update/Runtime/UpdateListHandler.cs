using System;
using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update collection as ordered list with the specified handler to update each item.
    /// </summary>
    public class UpdateListHandler<TItem> : UpdateCollection<TItem> where TItem : class
    {
        public UpdateHandler<TItem> Handler { get; }

        private readonly List<TItem> m_items;

        /// <summary>
        /// Creates update list with the specified handler and UpdateQueueSet queue.
        /// </summary>
        /// <param name="handler">The handler to update each item.</param>
        public UpdateListHandler(UpdateHandler<TItem> handler) : this(handler, new UpdateQueueSet<TItem>())
        {
        }

        /// <summary>
        /// Creates update list with the specified handler and update queue.
        /// </summary>
        /// <param name="handler">The handler to update each item.</param>
        /// <param name="queue">The update queue.</param>
        public UpdateListHandler(UpdateHandler<TItem> handler, IUpdateQueue<TItem> queue) : base(new List<TItem>(), queue)
        {
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));

            m_items = (List<TItem>)Collection;
        }

        public override void Update()
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                Handler(m_items[i]);
            }
        }

        public List<TItem>.Enumerator GetEnumerator()
        {
            return m_items.GetEnumerator();
        }
    }
}
