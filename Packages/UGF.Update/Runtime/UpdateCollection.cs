using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update collection used with <see cref="IUpdateHandler"/> handlers.
    /// </summary>
    public class UpdateCollection<THandler> : UpdateCollectionBase<THandler> where THandler : IUpdateHandler
    {
        /// <summary>
        /// Creates the update collection with the handler comparer, if specified.
        /// </summary>
        /// <param name="comparer">The handlers equality comparer.</param>
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
