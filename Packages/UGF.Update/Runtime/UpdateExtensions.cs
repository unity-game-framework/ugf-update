using UnityEngine.Experimental.LowLevel;

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
        public static string Print(this PlayerLoopSystem playerLoopSystem, int depth = 0, string indent = "    ")
        {
            return UpdateUtility.PrintPlayerLoop(playerLoopSystem, depth, indent);
        }
    }
}
