using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.Profiling;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update group with the specified update collection and collection of the subgroup.
    /// </summary>
    /// <remarks>
    /// This update group stores and updates subgroup in an ordered collection.
    ///
    /// All subgroups stored by the group name which must be unique.
    /// </remarks>
    public class UpdateGroup<TItem> : IUpdateGroup<TItem>
    {
        public string Name { get; }
        public bool Enable { get; set; } = true;
        public IUpdateCollection<TItem> Collection { get; }
        public IReadOnlyList<IUpdateGroup> SubGroups { get; }

        IUpdateCollection IUpdateGroup.Collection { get { return Collection; } }

        private readonly List<IUpdateGroup> m_subGroups = new List<IUpdateGroup>();
        private readonly Dictionary<string, IUpdateGroup> m_subGroupsByName = new Dictionary<string, IUpdateGroup>();
        private ProfilerMarker m_marker;

        /// <summary>
        /// Creates update group with the specified name and update collection.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="collection">The update collection.</param>
        public UpdateGroup(string name, IUpdateCollection<TItem> collection)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

            Name = name;
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
            SubGroups = new ReadOnlyCollection<IUpdateGroup>(m_subGroups);

#if ENABLE_PROFILER
            m_marker = new ProfilerMarker($"UpdateGroup.{Name}");
#endif
        }

        /// <summary>
        /// Inserts the specified group at the specified index.
        /// </summary>
        /// <remarks>
        /// The name of the group must be unique.
        /// </remarks>
        /// <param name="group">The group to insert.</param>
        /// <param name="index">The index to insert at.</param>
        public void Insert(IUpdateGroup group, int index)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));

            if (m_subGroupsByName.ContainsKey(group.Name))
            {
                throw new ArgumentException($"A group with the same name already exists: '{group.Name}'.", nameof(group));
            }

            m_subGroups.Insert(index, group);
            m_subGroupsByName.Add(group.Name, group);
        }

        public void Add(IUpdateGroup group)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));

            if (m_subGroupsByName.ContainsKey(group.Name))
            {
                throw new ArgumentException($"A group with the same name already exists: '{group.Name}'.", nameof(group));
            }

            m_subGroups.Add(group);
            m_subGroupsByName.Add(group.Name, group);
        }

        public void Remove(IUpdateGroup group)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));

            if (m_subGroupsByName.Remove(group.Name))
            {
                m_subGroups.Remove(group);
            }
        }

        /// <summary>
        /// Removes subgroup by the specified name.
        /// </summary>
        /// <param name="groupName">The name of the group to remove.</param>
        public void Remove(string groupName)
        {
            if (string.IsNullOrEmpty(groupName)) throw new ArgumentException("Value cannot be null or empty.", nameof(groupName));

            if (m_subGroupsByName.TryGetValue(groupName, out IUpdateGroup group))
            {
                m_subGroupsByName.Remove(groupName);
                m_subGroups.Remove(group);
            }
        }

        public void Update()
        {
            if (Enable)
            {
                m_marker.Begin();

                Collection.ApplyQueueAndUpdate();

                for (int i = 0; i < m_subGroups.Count; i++)
                {
                    m_subGroups[i].Update();
                }

                m_marker.End();
            }
        }

        public T GetSubGroup<T>(string name) where T : IUpdateGroup
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

            if (!m_subGroupsByName.TryGetValue(name, out IUpdateGroup group))
            {
                throw new ArgumentException($"A group by the specified name not found: '{name}'.");
            }

            return (T)group;
        }

        public IUpdateGroup GetSubGroup(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

            if (!m_subGroupsByName.TryGetValue(name, out IUpdateGroup group))
            {
                throw new ArgumentException($"A group by the specified name not found: '{name}'.");
            }

            return group;
        }

        public bool TryGetSubGroup<T>(string name, out T group) where T : IUpdateGroup
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

            if (m_subGroupsByName.TryGetValue(name, out IUpdateGroup value) && value is T cast)
            {
                group = cast;
                return true;
            }

            group = default;
            return false;
        }

        public bool TryGetSubGroup(string name, out IUpdateGroup group)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

            return m_subGroupsByName.TryGetValue(name, out group);
        }

        public List<IUpdateGroup>.Enumerator GetEnumerator()
        {
            return m_subGroups.GetEnumerator();
        }
    }
}
