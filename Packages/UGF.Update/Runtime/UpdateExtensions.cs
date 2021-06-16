using UnityEngine.LowLevel;

namespace UGF.Update.Runtime
{
    public static class UpdateExtensions
    {
        /// <summary>
        /// Prints full hierarchy of the specified player loop system as string representation.
        /// </summary>
        /// <param name="playerLoopSystem">The player loop system to print.</param>
        /// <param name="depth">The initial indent depth.</param>
        /// <param name="indent">The indent value used for nested nodes.</param>
        public static string Print(this PlayerLoopSystem playerLoopSystem, int depth = 0, int indent = 4)
        {
            return UpdateUtility.PrintPlayerLoop(playerLoopSystem, depth, indent);
        }

        /// <summary>
        /// Prints full hierarchy of subgroups for the specified update group, items of the update collection, add and remove queue of items.
        /// </summary>
        /// <param name="group">The update group to print.</param>
        /// <param name="depth">The initial indent depth.</param>
        /// <param name="indent">The indent value used for nested nodes.</param>
        public static string Print(this IUpdateGroup group, int depth = 0, int indent = 4)
        {
            return UpdateUtility.PrintUpdateGroup(group, depth, indent);
        }
    }
}
