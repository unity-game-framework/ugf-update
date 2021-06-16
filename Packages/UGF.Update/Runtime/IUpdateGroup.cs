namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update group with update collection and subgroups.
    /// </summary>
    public interface IUpdateGroup
    {
        /// <summary>
        /// Gets or sets the value to enable or disable this group.
        /// </summary>
        bool Enable { get; set; }

        /// <summary>
        /// Gets the update collection.
        /// </summary>
        IUpdateCollection Collection { get; }

        /// <summary>
        /// Gets the update collection of the subgroups.
        /// </summary>
        IUpdateCollection<IUpdateGroup> SubGroups { get; }

        /// <summary>
        /// Updates group.
        /// </summary>
        void Update();
    }
}
