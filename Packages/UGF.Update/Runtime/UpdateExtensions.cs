using UnityEngine.Experimental.LowLevel;

namespace UGF.Update.Runtime
{
    public static class UpdateExtensions
    {
        public static string Print(this PlayerLoopSystem playerLoopSystem, int depth = 0, string indent = "    ")
        {
            return UpdateUtility.PrintPlayerLoop(playerLoopSystem, depth, indent);
        }
    }
}
