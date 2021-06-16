using System;
using UnityEngine.LowLevel;

namespace UGF.Update.Runtime
{
    public readonly struct UpdateGroupFunction
    {
        public Type SubSystemType { get; }
        public PlayerLoopSystem.UpdateFunction UpdateFunction { get; }

        public UpdateGroupFunction(Type subSystemType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            SubSystemType = subSystemType ?? throw new ArgumentNullException(nameof(subSystemType));
            UpdateFunction = updateFunction ?? throw new ArgumentNullException(nameof(updateFunction));
        }
    }
}
