using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update collection used with <see cref="IUpdateHandler"/> handlers.
    /// </summary>
    public class UpdateSet<THandler> : UpdateCollectionBase<THandler> where THandler : IUpdateHandler
    {
        protected override ICollection<THandler> Collection { get { return m_handlers; } }

        private readonly HashSet<THandler> m_handlers;

        /// <summary>
        /// Creates the update collection with the handler comparer, if specified.
        /// </summary>
        /// <param name="comparer">The handlers equality comparer.</param>
        public UpdateSet(IEqualityComparer<THandler> comparer = null) : base(comparer)
        {
            m_handlers = new HashSet<THandler>(comparer);
        }

        public override void Update()
        {
            foreach (THandler handler in this)
            {
                handler.OnUpdate();
            }
        }

        public HashSet<THandler>.Enumerator GetEnumerator()
        {
            return m_handlers.GetEnumerator();
        }
    }
}
