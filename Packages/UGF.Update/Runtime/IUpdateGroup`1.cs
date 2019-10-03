namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update group with the specified type of the update collection.
    /// </summary>
    public interface IUpdateGroup<TItem> : IUpdateGroup
    {
        /// <summary>
        /// Gets the update collection.
        /// </summary>
        new IUpdateCollection<TItem> Collection { get; }
    }
}
