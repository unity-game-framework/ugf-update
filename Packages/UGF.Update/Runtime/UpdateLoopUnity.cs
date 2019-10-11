using UnityEngine.LowLevel;

namespace UGF.Update.Runtime
{
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

        public void Reset()
        {
            UpdateUtility.ResetPlayerLoopToDefault();
        }
    }
}