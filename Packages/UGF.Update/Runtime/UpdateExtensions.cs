using System;
using System.Collections.Generic;
using UnityEngine.LowLevel;

namespace UGF.Update.Runtime
{
    public static class UpdateExtensions
    {
        private static readonly char[] m_pathSeparatorDefault = { '.' };

        public static bool TryFindCollection<T>(this IUpdateProvider updateProvider, string path, out T collection) where T : IUpdateCollection
        {
            if (updateProvider == null) throw new ArgumentNullException(nameof(updateProvider));
            if (string.IsNullOrEmpty(path)) throw new ArgumentException("Value cannot be null or empty.", nameof(path));

            if (TryFindGroup(updateProvider, path, out IUpdateGroup group) && group.Collection is T cast)
            {
                collection = cast;
                return true;
            }

            collection = default;
            return false;
        }

        public static bool TryFindCollection<T>(this IUpdateGroup updateGroup, string path, out T collection) where T : IUpdateCollection
        {
            if (updateGroup == null) throw new ArgumentNullException(nameof(updateGroup));
            if (string.IsNullOrEmpty(path)) throw new ArgumentException("Value cannot be null or empty.", nameof(path));

            if (TryFindGroup(updateGroup, path, out IUpdateGroup group) && group.Collection is T cast)
            {
                collection = cast;
                return true;
            }

            collection = default;
            return false;
        }

        public static bool TryFindGroup<T>(this IUpdateProvider updateProvider, string path, out T group) where T : IUpdateGroup
        {
            if (updateProvider == null) throw new ArgumentNullException(nameof(updateProvider));
            if (string.IsNullOrEmpty(path)) throw new ArgumentException("Value cannot be null or empty.", nameof(path));

            string[] split = path.Split(m_pathSeparatorDefault);

            if (split.Length > 0 && updateProvider.Groups.TryGetValue(split[0], out IUpdateGroup updateGroup))
            {
                if (split.Length > 1)
                {
                    if (TryFindGroup(updateGroup, split, 1, out IUpdateGroup result) && result is T cast)
                    {
                        group = cast;
                        return true;
                    }
                }
                else if (updateGroup is T cast)
                {
                    group = cast;
                    return true;
                }
            }

            group = default;
            return false;
        }

        public static bool TryFindGroup<T>(this IUpdateGroup updateGroup, string path, out T group) where T : IUpdateGroup
        {
            if (updateGroup == null) throw new ArgumentNullException(nameof(updateGroup));
            if (string.IsNullOrEmpty(path)) throw new ArgumentException("Value cannot be null or empty.", nameof(path));

            string[] split = path.Split(m_pathSeparatorDefault);

            if (split.Length > 0 && TryFindGroup(updateGroup, split, 0, out IUpdateGroup result) && result is T cast)
            {
                group = cast;
                return true;
            }

            group = default;
            return false;
        }

        /// <summary>
        /// Prints full hierarchy of the specified player loop system as string representation.
        /// </summary>
        /// <param name="playerLoopSystem">The player loop system to print.</param>
        /// <param name="depth">The initial indent depth.</param>
        /// <param name="indent">The indent value used for nested nodes.</param>
        public static string Print(this PlayerLoopSystem playerLoopSystem, int depth = 0, string indent = "    ")
        {
            return UpdateUtility.PrintPlayerLoop(playerLoopSystem, depth, indent);
        }

        private static bool TryFindGroup(IUpdateGroup updateGroup, IReadOnlyList<string> path, int index, out IUpdateGroup group)
        {
            if (updateGroup == null) throw new ArgumentNullException(nameof(updateGroup));
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (path.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(path));
            if (index < 0 || index >= path.Count) throw new ArgumentOutOfRangeException(nameof(index));

            for (int i = 0; i < updateGroup.SubGroups.Count; i++)
            {
                IUpdateGroup subGroup = updateGroup.SubGroups[i];

                if (subGroup.Name == path[index])
                {
                    if (index == path.Count - 1)
                    {
                        group = subGroup;
                        return true;
                    }

                    return TryFindGroup(subGroup, path, ++index, out group);
                }
            }

            group = null;
            return false;
        }
    }
}
