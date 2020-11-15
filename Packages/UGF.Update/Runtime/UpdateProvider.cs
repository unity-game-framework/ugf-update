using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.LowLevel;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents default implementation of the update provider.
    /// </summary>
    public class UpdateProvider : IUpdateProvider
    {
        public IUpdateLoop UpdateLoop { get; }
        public IReadOnlyDictionary<string, IUpdateGroup> Groups { get; }

        private readonly Dictionary<string, IUpdateGroup> m_groups = new Dictionary<string, IUpdateGroup>();
        private readonly Dictionary<string, GroupInfo> m_infos = new Dictionary<string, GroupInfo>();

        private readonly struct GroupInfo
        {
            public Type SubSystemType { get; }
            public PlayerLoopSystem.UpdateFunction UpdateFunction { get; }

            public GroupInfo(Type subSystemType, PlayerLoopSystem.UpdateFunction updateFunction)
            {
                SubSystemType = subSystemType ?? throw new ArgumentNullException(nameof(subSystemType));
                UpdateFunction = updateFunction ?? throw new ArgumentNullException(nameof(updateFunction));
            }
        }

        /// <summary>
        /// Creates provider with the specified update loop.
        /// </summary>
        /// <param name="updateLoop">The update loop to use.</param>
        public UpdateProvider(IUpdateLoop updateLoop)
        {
            UpdateLoop = updateLoop ?? throw new ArgumentNullException(nameof(updateLoop));
            Groups = new ReadOnlyDictionary<string, IUpdateGroup>(m_groups);
        }

        public void Add(Type subSystemType, IUpdateGroup updateGroup)
        {
            if (subSystemType == null) throw new ArgumentNullException(nameof(subSystemType));
            if (updateGroup == null) throw new ArgumentNullException(nameof(updateGroup));
            if (m_groups.ContainsKey(updateGroup.Name)) throw new ArgumentException($"A group with the same name already exists: '{updateGroup.Name}'.", nameof(updateGroup));

            PlayerLoopSystem playerLoop = UpdateLoop.GetPlayerLoop();
            PlayerLoopSystem.UpdateFunction updateFunction = updateGroup.Update;

            if (!UpdateUtility.TryAddUpdateFunction(playerLoop, subSystemType, updateFunction))
            {
                throw new ArgumentException($"Change update loop failed with the specified subsystem type: '{subSystemType}'.");
            }

            UpdateLoop.SetPlayerLoop(playerLoop);

            var info = new GroupInfo(subSystemType, updateFunction);

            m_groups.Add(updateGroup.Name, updateGroup);
            m_infos.Add(updateGroup.Name, info);
        }

        public void Remove(IUpdateGroup updateGroup)
        {
            if (updateGroup == null) throw new ArgumentNullException(nameof(updateGroup));

            Remove(updateGroup.Name);
        }

        public void Remove(string groupName)
        {
            if (string.IsNullOrEmpty(groupName)) throw new ArgumentException("Value cannot be null or empty.", nameof(groupName));

            if (m_groups.Remove(groupName))
            {
                PlayerLoopSystem playerLoop = UpdateLoop.GetPlayerLoop();
                GroupInfo info = m_infos[groupName];

                if (UpdateUtility.TryRemoveUpdateFunction(playerLoop, info.SubSystemType, info.UpdateFunction))
                {
                    UpdateLoop.SetPlayerLoop(playerLoop);
                }

                m_infos.Remove(groupName);
            }
        }

        public void Clear()
        {
            PlayerLoopSystem playerLoop = UpdateLoop.GetPlayerLoop();
            bool changed = false;

            foreach (KeyValuePair<string, GroupInfo> pair in m_infos)
            {
                if (UpdateUtility.TryRemoveUpdateFunction(playerLoop, pair.Value.SubSystemType, pair.Value.UpdateFunction))
                {
                    changed = true;
                }
            }

            if (changed)
            {
                UpdateLoop.SetPlayerLoop(playerLoop);
            }

            m_groups.Clear();
            m_infos.Clear();
        }

        public Type GetSubSystemType(string groupName)
        {
            if (string.IsNullOrEmpty(groupName)) throw new ArgumentException("Value cannot be null or empty.", nameof(groupName));

            return TryGetSubSystemType(groupName, out Type type) ? type : throw new ArgumentException($"Group not found by the specified name: '{groupName}'.", nameof(groupName));
        }

        public bool TryGetSubSystemType(string groupName, out Type type)
        {
            if (string.IsNullOrEmpty(groupName)) throw new ArgumentException("Value cannot be null or empty.", nameof(groupName));

            if (m_infos.TryGetValue(groupName, out GroupInfo info))
            {
                type = info.SubSystemType;
                return true;
            }

            type = null;
            return false;
        }

        public T GetGroup<T>(string groupName) where T : IUpdateGroup
        {
            if (string.IsNullOrEmpty(groupName)) throw new ArgumentException("Value cannot be null or empty.", nameof(groupName));

            return TryGetGroup(groupName, out T group) ? group : throw new ArgumentException($"Group not found by the specified name: '{group}'.");
        }

        public bool TryGetGroup<T>(string groupName, out T updateGroup) where T : IUpdateGroup
        {
            if (string.IsNullOrEmpty(groupName)) throw new ArgumentException("Value cannot be null or empty.", nameof(groupName));

            if (m_groups.TryGetValue(groupName, out IUpdateGroup value))
            {
                updateGroup = (T)value;
                return true;
            }

            updateGroup = default;
            return false;
        }

        public Dictionary<string, IUpdateGroup>.Enumerator GetEnumerator()
        {
            return m_groups.GetEnumerator();
        }
    }
}
