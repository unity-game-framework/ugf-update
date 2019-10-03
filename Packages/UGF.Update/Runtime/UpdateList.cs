using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update collection as ordered list of the items where each of them implements 'IUpdateHandler' interface.
    /// </summary>
    public class UpdateList<TItem> : UpdateCollection<TItem> where TItem : IUpdateHandler
    {
        private readonly List<TItem> m_items;

        /// <summary>
        /// Creates update list with the specified update queue.
        /// </summary>
        /// <param name="queue">The update queue. (Default value is UpdateQueueSet)</param>
        public UpdateList(IUpdateQueue<TItem> queue = null) : base(new List<TItem>(), queue ?? new UpdateQueueSet<TItem>())
        {
            m_items = (List<TItem>)Collection;
        }

        public override void Update()
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                m_items[i].OnUpdate();
            }
        }

        public List<TItem>.Enumerator GetEnumerator()
        {
            return m_items.GetEnumerator();
        }
    }
}
