using UnityEngine.LowLevel;

namespace UGF.Update.Runtime
{
    public interface IUpdateLoop
    {
        PlayerLoopSystem GetCurrentPlayerLoop();
        void SetPlayerLoop(PlayerLoopSystem playerLoop);
    }
}
