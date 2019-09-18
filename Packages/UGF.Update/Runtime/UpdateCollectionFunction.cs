using System;
using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update collection that use update function to handle updates.
    /// </summary>
    public class UpdateSetFunction<THandler> : UpdateCollectionBase<THandler>
    {
        /// <summary>
        /// Gets the update function.
        /// </summary>
        public Action<THandler> UpdateFunction { get; }

        protected override ICollection<THandler> Collection { get { return m_handlers; } }

        private readonly HashSet<THandler> m_handlers;

        /// <summary>
        /// Creates the update collection with the specified update function and the handler comparer, if specified.
        /// </summary>
        /// <param name="updateFunction">The update function.</param>
        /// <param name="comparer">The handlers equality comparer.</param>
        public UpdateSetFunction(Action<THandler> updateFunction, IEqualityComparer<THandler> comparer = null) : base(comparer)
        {
            m_handlers = new HashSet<THandler>(comparer);

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
