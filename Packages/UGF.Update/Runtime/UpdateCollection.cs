using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    public class UpdateCollection<THandler> : UpdateCollectionBase<THandler> where THandler : IUpdateHandler
    {
        public UpdateCollection(IEqualityComparer<THandler> comparer = null) : base(comparer)
        {
        }

        public override void Update()
        {
            foreach (THandler handler in this)
            {
                handler.OnUpdate();
            }
        }
    }
}
