using UnityEngine.LowLevel;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update loop access of the Unity player loop system.
    /// </summary>
    public class UpdateLoopUnity : UpdateLoopBase
    {
        protected override PlayerLoopSystem OnGetPlayerLoop()
        {
            return PlayerLoop.GetCurrentPlayerLoop();
        }

        protected override void OnSetPlayerLoop(PlayerLoopSystem playerLoop)
        {
            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        protected override void OnReset()
        {
            UpdateUtility.ResetPlayerLoopToDefault();
        }
    }
}
