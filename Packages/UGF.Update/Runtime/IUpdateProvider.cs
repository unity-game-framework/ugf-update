using System;
using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents provider of the update groups.
    /// </summary>
    public interface IUpdateProvider
    {
        /// <summary>
        /// Gets update loop used to register groups.
        /// </summary>
        IUpdateLoop UpdateLoop { get; }

        /// <summary>
        /// Gets collection of the update groups.
        /// </summary>
        IReadOnlyDictionary<string, IUpdateGroup> Groups { get; }

        /// <summary>
        /// Adds group to the player loop subsystem of the specified type.
        /// </summary>
        /// <param name="subSystemType">The type of the player loop subsystem.</param>
        /// <param name="updateGroup">The update group to add.</param>
        void Add(Type subSystemType, IUpdateGroup updateGroup);

        /// <summary>
        /// Removes update group by the specified name.
        /// </summary>
        /// <param name="groupName">The name of the group.</param>
        void Remove(string groupName);

        /// <summary>
        /// Clears and unregister all groups from update loop.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets type of the player loop subsystem for the group of the specified name.
        /// </summary>
        /// <param name="groupName">The name of the group.</param>
        Type GetSubSystemType(string groupName);

        /// <summary>
        /// Tries to get type of the player loop subsystem for the group of the specified name.
        /// </summary>
        /// <param name="groupName">The name of the group.</param>
        /// <param name="type">The found type.</param>
        bool TryGetSubSystemType(string groupName, out Type type);

        /// <summary>
        /// Tries to get group by the specified name.
        /// </summary>
        /// <param name="groupName">The name of the group.</param>
        /// <param name="updateGroup">The found group.</param>
        bool TryGetGroup<T>(string groupName, out T updateGroup) where T : IUpdateGroup;
    }
}
