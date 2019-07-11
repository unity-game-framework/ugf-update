using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    public class UpdateCollection<THandler> : UpdateCollectionBase<THandler> where THandler : IUpdateHandler
    {
        public UpdateCollection(IEqualityComparer<THandler> comparer = null) : base(comparer)
        {
        }

        protected override void OnUpdate(THandler handler)
        {
            handler.OnUpdate();
        }
    }
}
