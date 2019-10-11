using UnityEngine.LowLevel;

namespace UGF.Update.Runtime
{
    public interface IUpdateLoop
    {
        PlayerLoopSystem GetPlayerLoop();
        void SetPlayerLoop(PlayerLoopSystem playerLoop);
    }
}
