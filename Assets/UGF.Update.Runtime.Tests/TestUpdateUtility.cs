using System;
using NUnit.Framework;
using UnityEngine.Experimental.LowLevel;
using PlayerLoops = UnityEngine.Experimental.PlayerLoop;

namespace UGF.Update.Runtime.Tests
{
    public class TestUpdateUtility
    {
        public class Target
        {
            public void Update()
            {
            }
        }

        [Test]
        public void TryAddUpdateFunction()
        {
            Assert.Ignore();
        }

        [Test]
        public void TrySetPlayerLoopSystem()
        {
            PlayerLoopSystem playerLoop = PlayerLoop.GetDefaultPlayerLoop();
            Type[] path0 = { typeof(PlayerLoops.Update), typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate) };
            Type[] path1 = { typeof(PlayerLoops.Update), typeof(Target) };

            bool result0 = UpdateUtility.TryGetPlayerLoopSystem(playerLoop, path0, out PlayerLoopSystem playerLoopSystem0);

            Assert.True(result0);
            Assert.AreEqual(typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate), playerLoopSystem0.type);

            playerLoopSystem0.type = typeof(Target);

            bool result1 = UpdateUtility.TrySetPlayerLoopSystem(playerLoop, path0, playerLoopSystem0);

            Assert.True(result1);

            bool result2 = UpdateUtility.TryGetPlayerLoopSystem(playerLoop, path1, out PlayerLoopSystem playerLoopSystem1);

            Assert.True(result2);
            Assert.AreEqual(typeof(Target), playerLoopSystem1.type);
            Assert.Pass(playerLoop.Print());
        }

        [Test]
        public void TryGetPlayerLoopSystem()
        {
            PlayerLoopSystem playerLoop = PlayerLoop.GetDefaultPlayerLoop();
            Type[] path = { typeof(PlayerLoops.Update), typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate) };

            bool result = UpdateUtility.TryGetPlayerLoopSystem(playerLoop, path, out PlayerLoopSystem playerLoopSystem);

            Assert.True(result);
            Assert.AreEqual(typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate), playerLoopSystem.type);
        }

        [Test]
        public void PrintPlayerLoop()
        {
            PlayerLoopSystem playerLoopSystem = PlayerLoop.GetDefaultPlayerLoop();

            string result = UpdateUtility.PrintPlayerLoop(playerLoopSystem);

            Assert.Pass(result);
        }
    }
}
