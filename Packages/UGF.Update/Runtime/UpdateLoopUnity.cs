using UnityEngine.LowLevel;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update loop access of the Unity player loop system.
    /// </summary>
    public class UpdateLoopUnity : IUpdateLoop
    {
        public PlayerLoopSystem GetPlayerLoop()
        {
            return PlayerLoop.GetCurrentPlayerLoop();
        }

        public void SetPlayerLoop(PlayerLoopSystem playerLoop)
        {
            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        /// <summary>
        /// Resets Unity player loop to default.
        /// </summary>
        public void Reset()
        {
            UpdateUtility.ResetPlayerLoopToDefault();
        }
    }
}
