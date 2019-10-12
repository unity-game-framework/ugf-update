namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents generic implementation of the UpdateGroup.
    /// </summary>
    public class UpdateGroup<TItem> : UpdateGroup, IUpdateGroup<TItem>
    {
        public new IUpdateCollection<TItem> Collection { get; }

        /// <summary>
        /// Creates update group with the specified name and update collection.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="collection">The update collection.</param>
        public UpdateGroup(string name, IUpdateCollection<TItem> collection) : base(name, collection)
        {
            Collection = collection;
        }
    }
}
