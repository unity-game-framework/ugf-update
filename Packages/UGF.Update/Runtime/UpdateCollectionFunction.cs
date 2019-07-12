using System;
using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    public class UpdateCollectionFunction<THandler> : UpdateCollectionBase<THandler>
    {
        public Action<THandler> UpdateFunction { get; }

        public UpdateCollectionFunction(Action<THandler> updateFunction, IEqualityComparer<THandler> comparer = null) : base(comparer)
        {
            UpdateFunction = updateFunction ?? throw new ArgumentNullException(nameof(updateFunction));
        }

        public override void Update()
        {
            foreach (THandler handler in this)
            {
                UpdateFunction(handler);
            }
        }
    }
}
