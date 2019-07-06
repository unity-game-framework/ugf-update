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
        public void SetupPlayer()
        {
            PlayerLoopSystem playerLoop = PlayerLoop.GetDefaultPlayerLoop();

            var target = new Target();
            Type[] path = { typeof(PlayerLoops.Update), typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate) };

            UpdateUtility.TryAddUpdateFunction(playerLoop, path, target.Update);

            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        [Test]
        public void TryAddUpdateFunction()
        {
            var playerLoop = new PlayerLoopSystem
            {
                subSystemList = new[]
                {
                    new PlayerLoopSystem { type = typeof(PlayerLoops.PreUpdate) },
                    new PlayerLoopSystem
                    {
                        type = typeof(PlayerLoops.Update),
                        subSystemList = new[]
                        {
                            new PlayerLoopSystem { type = typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate) }
                        }
                    },
                    new PlayerLoopSystem { type = typeof(PlayerLoops.PostLateUpdate) }
                }
            };

            var target = new Target();
            PlayerLoopSystem.UpdateFunction function = target.Update;
            Type[] path = { typeof(PlayerLoops.Update), typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate) };

            bool result0 = UpdateUtility.TryAddUpdateFunction(playerLoop, path, function);

            Assert.True(result0);

            Delegate[] invocations = playerLoop.subSystemList[1].subSystemList[0].updateDelegate.GetInvocationList();

            Assert.Contains(function, invocations);

            Assert.Pass(playerLoop.Print());
        }

        [Test]
        public void TryRemoveUpdateFunction()
        {
            var playerLoop = new PlayerLoopSystem
            {
                subSystemList = new[]
                {
                    new PlayerLoopSystem { type = typeof(PlayerLoops.PreUpdate) },
                    new PlayerLoopSystem
                    {
                        type = typeof(PlayerLoops.Update),
                        subSystemList = new[]
                        {
                            new PlayerLoopSystem { type = typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate) }
                        }
                    },
                    new PlayerLoopSystem { type = typeof(PlayerLoops.PostLateUpdate) }
                }
            };

            var target = new Target();
            PlayerLoopSystem.UpdateFunction function = target.Update;
            Type[] path = { typeof(PlayerLoops.Update), typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate) };

            playerLoop.subSystemList[1].subSystemList[0].updateDelegate += function;

            bool result0 = UpdateUtility.TryRemoveUpdateFunction(playerLoop, path, function);

            Assert.True(result0);
            Assert.Null(playerLoop.subSystemList[1].subSystemList[0].updateDelegate);
            Assert.Pass(playerLoop.Print());
        }

        [Test]
        public void TryAddSubSystem()
        {
            var playerLoop = new PlayerLoopSystem
            {
                subSystemList = new[]
                {
                    new PlayerLoopSystem { type = typeof(PlayerLoops.PreUpdate) },
                    new PlayerLoopSystem
                    {
                        type = typeof(PlayerLoops.Update),
                        subSystemList = new[]
                        {
                            new PlayerLoopSystem { type = typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate) }
                        }
                    },
                    new PlayerLoopSystem { type = typeof(PlayerLoops.PostLateUpdate) }
                }
            };

            Type[] path = { typeof(PlayerLoops.Update), typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate) };

            bool result0 = UpdateUtility.TryAddSubSystem(playerLoop, path, typeof(PlayerLoops.FixedUpdate), 0);

            Assert.True(result0);

            PlayerLoopSystem[] subSystems = playerLoop.subSystemList[1].subSystemList[0].subSystemList;

            Assert.NotNull(subSystems);
            Assert.AreEqual(1, subSystems.Length);
            Assert.AreEqual(typeof(PlayerLoops.FixedUpdate), subSystems[0].type);
            Assert.Pass(playerLoop.Print());
        }

        [Test]
        public void TryRemoveSubSystem()
        {
            var playerLoop = new PlayerLoopSystem
            {
                subSystemList = new[]
                {
                    new PlayerLoopSystem { type = typeof(PlayerLoops.PreUpdate) },
                    new PlayerLoopSystem
                    {
                        type = typeof(PlayerLoops.Update),
                        subSystemList = new[]
                        {
                            new PlayerLoopSystem
                            {
                                type = typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate),
                                subSystemList = new[]
                                {
                                    new PlayerLoopSystem { type = typeof(PlayerLoops.FixedUpdate) }
                                }
                            }
                        }
                    },
                    new PlayerLoopSystem { type = typeof(PlayerLoops.PostLateUpdate) }
                }
            };

            Type[] path = { typeof(PlayerLoops.Update), typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate) };

            bool result0 = UpdateUtility.TryRemoveSubSystem(playerLoop, path, 0);

            Assert.True(result0);
            Assert.Null(playerLoop.subSystemList[1].subSystemList[0].subSystemList);
            Assert.Pass(playerLoop.Print());
        }

        [Test]
        public void AddSubSystem()
        {
            var playerLoop = new PlayerLoopSystem
            {
                subSystemList = new[]
                {
                    new PlayerLoopSystem { type = typeof(PlayerLoops.PreUpdate) },
                    new PlayerLoopSystem { type = typeof(PlayerLoops.Update) },
                    new PlayerLoopSystem { type = typeof(PlayerLoops.PostLateUpdate) }
                }
            };

            UpdateUtility.AddSubSystem(ref playerLoop, typeof(PlayerLoops.FixedUpdate), 1);

            Assert.AreEqual(4, playerLoop.subSystemList.Length);
            Assert.AreEqual(typeof(PlayerLoops.PreUpdate), playerLoop.subSystemList[0].type);
            Assert.AreEqual(typeof(PlayerLoops.FixedUpdate), playerLoop.subSystemList[1].type);
            Assert.AreEqual(typeof(PlayerLoops.Update), playerLoop.subSystemList[2].type);
            Assert.AreEqual(typeof(PlayerLoops.PostLateUpdate), playerLoop.subSystemList[3].type);
            Assert.Pass(playerLoop.Print());
        }

        [Test]
        public void RemoveSubSystem()
        {
            var playerLoop = new PlayerLoopSystem
            {
                subSystemList = new[]
                {
                    new PlayerLoopSystem { type = typeof(PlayerLoops.PreUpdate) },
                    new PlayerLoopSystem { type = typeof(PlayerLoops.FixedUpdate) },
                    new PlayerLoopSystem { type = typeof(PlayerLoops.Update) },
                    new PlayerLoopSystem { type = typeof(PlayerLoops.PostLateUpdate) }
                }
            };

            UpdateUtility.RemoveSubSystem(ref playerLoop, 1);

            Assert.AreEqual(3, playerLoop.subSystemList.Length);
            Assert.AreEqual(typeof(PlayerLoops.PreUpdate), playerLoop.subSystemList[0].type);
            Assert.AreEqual(typeof(PlayerLoops.Update), playerLoop.subSystemList[1].type);
            Assert.AreEqual(typeof(PlayerLoops.PostLateUpdate), playerLoop.subSystemList[2].type);
            Assert.Pass(playerLoop.Print());
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
