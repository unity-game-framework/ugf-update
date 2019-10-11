using UnityEngine.LowLevel;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents access to the player loop system.
    /// </summary>
    public interface IUpdateLoop
    {
        /// <summary>
        /// Gets current player loop.
        /// </summary>
        PlayerLoopSystem GetPlayerLoop();

        /// <summary>
        /// Sets the specified player loop as current.
        /// </summary>
        /// <param name="playerLoop">The player loop to set.</param>
        void SetPlayerLoop(PlayerLoopSystem playerLoop);
    }
}
