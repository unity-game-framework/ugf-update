using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    public class UpdateList<THandler> : UpdateCollectionBase<THandler> where THandler : IUpdateHandler
    {
        protected override ICollection<THandler> Collection { get { return m_handlers; } }

        private readonly List<THandler> m_handlers = new List<THandler>();

        public UpdateList(IEqualityComparer<THandler> comparer = null) : base(comparer)
        {
        }

        public override void Update()
        {
            for (int i = 0; i < m_handlers.Count; i++)
            {
                m_handlers[i].OnUpdate();
            }
        }

        public List<THandler>.Enumerator GetEnumerator()
        {
            return m_handlers.GetEnumerator();
        }
    }
}
