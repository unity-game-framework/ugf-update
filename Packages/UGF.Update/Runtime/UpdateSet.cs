using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update collection as unordered set of the items where each of them implements 'IUpdateHandler' interface.
    /// </summary>
    public class UpdateSet<TItem> : UpdateCollection<TItem> where TItem : class, IUpdateHandler
    {
        private readonly HashSet<TItem> m_items;

        /// <summary>
        /// Creates update set with the specified update queue.
        /// </summary>
        /// <param name="queue">The update queue. (Default value is UpdateQueueSet)</param>
        public UpdateSet(IUpdateQueue<TItem> queue = null) : base(new HashSet<TItem>(), queue ?? new UpdateQueueSet<TItem>())
        {
            m_items = (HashSet<TItem>)Collection;
        }

        public override void Update()
        {
            foreach (TItem item in m_items)
            {
                item.OnUpdate();
            }
        }

        public HashSet<TItem>.Enumerator GetEnumerator()
        {
            return m_items.GetEnumerator();
        }
    }
}
