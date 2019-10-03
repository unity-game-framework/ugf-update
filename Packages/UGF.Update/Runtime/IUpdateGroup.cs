using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update group with update collection and subgroups.
    /// </summary>
    public interface IUpdateGroup
    {
        /// <summary>
        /// Gets the name of this group.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the value to enable or disable this group.
        /// </summary>
        bool Enable { get; set; }

        /// <summary>
        /// Gets the update collection.
        /// </summary>
        IUpdateCollection Collection { get; }

        /// <summary>
        /// Gets the collection of the subgroups.
        /// </summary>
        IReadOnlyList<IUpdateGroup> SubGroups { get; }

        /// <summary>
        /// Adds the specified group as subgroup.
        /// </summary>
        /// <param name="group">The group to add.</param>
        void Add(IUpdateGroup group);

        /// <summary>
        /// Removes the specified group from subgroups.
        /// </summary>
        /// <param name="group">The group to remove.</param>
        void Remove(IUpdateGroup group);

        /// <summary>
        /// Updates group.
        /// </summary>
        void Update();

        /// <summary>
        /// Gets subgroup by the specified name.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        T GetSubGroup<T>(string name) where T : IUpdateGroup;

        /// <summary>
        /// Gets subgroup by the specified name.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        IUpdateGroup GetSubGroup(string name);

        /// <summary>
        /// Tries to get subgroup with the specified name.
        /// </summary>
        /// <param name="name">The name of the subgroup.</param>
        /// <param name="group">The found subgroup.</param>
        bool TryGetSubGroup<T>(string name, out T group) where T : IUpdateGroup;

        /// <summary>
        /// Tries to get subgroup with the specified name.
        /// </summary>
        /// <param name="name">The name of the subgroup.</param>
        /// <param name="group">The found subgroup.</param>
        bool TryGetSubGroup(string name, out IUpdateGroup group);
    }
}
