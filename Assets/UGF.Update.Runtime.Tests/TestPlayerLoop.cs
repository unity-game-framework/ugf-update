using UnityEngine;
using UnityEngine.Experimental.LowLevel;
using PlayerLoops = UnityEngine.Experimental.PlayerLoop;

namespace UGF.Update.Runtime.Tests
{
    public class TestPlayerLoop : MonoBehaviour
    {
        private void Start()
        {
            PlayerLoopSystem playerLoop = PlayerLoop.GetDefaultPlayerLoop();

            UpdateUtility.TryAddUpdateFunction(playerLoop, typeof(PlayerLoops.Update), () => Debug.LogWarning("UPDATE"));

            PlayerLoop.SetPlayerLoop(playerLoop);

            Debug.Log(playerLoop.Print());
        }
    }
}
