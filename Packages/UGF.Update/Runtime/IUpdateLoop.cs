using System;
using UnityEngine.LowLevel;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents access to the player loop system.
    /// </summary>
    public interface IUpdateLoop
    {
        bool Contains(Type systemType);
        void Add(Type targetSystem, Type systemType, UpdateSubSystemInsertion insertion);
        bool Remove(Type systemType);
        void AddFunction(Type systemType, PlayerLoopSystem.UpdateFunction updateFunction);
        void RemoveFunction(Type systemType, PlayerLoopSystem.UpdateFunction updateFunction);

        /// <summary>
        /// Gets current player loop.
        /// </summary>
        PlayerLoopSystem GetPlayerLoop();

        /// <summary>
        /// Sets the specified player loop as current.
        /// </summary>
        /// <param name="playerLoop">The player loop to set.</param>
        void SetPlayerLoop(PlayerLoopSystem playerLoop);

        /// <summary>
        /// Resets player loop to default.
        /// </summary>
        void Reset();
    }
}
