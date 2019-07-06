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

            UpdateUtility.TryAddUpdateFunction(playerLoop, typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate), target.Update);

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

            bool result0 = UpdateUtility.TryAddUpdateFunction(playerLoop, typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate), function);

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

            playerLoop.subSystemList[1].subSystemList[0].updateDelegate += function;

            bool result0 = UpdateUtility.TryRemoveUpdateFunction(playerLoop, typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate), function);

            Assert.True(result0);
            Assert.Null(playerLoop.subSystemList[1].subSystemList[0].updateDelegate);
            Assert.Pass(playerLoop.Print());
        }

        [Test]
        public void ContainsSubSystem()
        {
            Assert.Ignore();
        }

        [Test]
        public void TryAddSubSystemBefore()
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

            Type subSystemType = typeof(PlayerLoops.FixedUpdate);
            Type targetSubSystemType = typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate);

            bool result0 = UpdateUtility.TryAddSubSystem(ref playerLoop, subSystemType, targetSubSystemType, UpdateSubSystemInsertion.Before);

            Assert.True(result0);

            PlayerLoopSystem[] subSystems = playerLoop.subSystemList[1].subSystemList;

            Assert.NotNull(subSystems);
            Assert.AreEqual(2, subSystems.Length);
            Assert.AreEqual(typeof(PlayerLoops.FixedUpdate), subSystems[0].type);
            Assert.Pass(playerLoop.Print());
        }

        [Test]
        public void TryAddSubSystemAfter()
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

            Type subSystemType = typeof(PlayerLoops.FixedUpdate);
            Type targetSubSystemType = typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate);

            bool result0 = UpdateUtility.TryAddSubSystem(ref playerLoop, subSystemType, targetSubSystemType, UpdateSubSystemInsertion.After);

            Assert.True(result0);

            PlayerLoopSystem[] subSystems = playerLoop.subSystemList[1].subSystemList;

            Assert.NotNull(subSystems);
            Assert.AreEqual(2, subSystems.Length);
            Assert.AreEqual(typeof(PlayerLoops.FixedUpdate), subSystems[1].type);
            Assert.Pass(playerLoop.Print());
        }

        [Test]
        public void TryAddSubSystemInside()
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

            Type subSystemType = typeof(PlayerLoops.FixedUpdate);
            Type targetSubSystemType = typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate);

            bool result0 = UpdateUtility.TryAddSubSystem(ref playerLoop, subSystemType, targetSubSystemType, UpdateSubSystemInsertion.InsideTop);

            Assert.True(result0);

            PlayerLoopSystem[] subSystems = playerLoop.subSystemList[1].subSystemList[0].subSystemList;

            Assert.NotNull(subSystems);
            Assert.AreEqual(1, subSystems.Length);
            Assert.AreEqual(typeof(PlayerLoops.FixedUpdate), subSystems[0].type);
            Assert.Pass(playerLoop.Print());
        }

        [Test]
        public void TryAddSubSystemInsideTop()
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
                            new PlayerLoopSystem { type = typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate) },
                            new PlayerLoopSystem { type = typeof(PlayerLoops.Update.ScriptRunDelayedTasks) },
                            new PlayerLoopSystem { type = typeof(PlayerLoops.Update.ScriptRunDelayedDynamicFrameRate) }
                        }
                    },
                    new PlayerLoopSystem { type = typeof(PlayerLoops.PostLateUpdate) }
                }
            };

            Type subSystemType = typeof(PlayerLoops.FixedUpdate);
            Type targetSubSystemType = typeof(PlayerLoops.Update);

            bool result0 = UpdateUtility.TryAddSubSystem(ref playerLoop, subSystemType, targetSubSystemType, UpdateSubSystemInsertion.InsideTop);

            Assert.True(result0);

            PlayerLoopSystem[] subSystems = playerLoop.subSystemList[1].subSystemList;

            Assert.NotNull(subSystems);
            Assert.AreEqual(4, subSystems.Length);
            Assert.AreEqual(typeof(PlayerLoops.FixedUpdate), subSystems[0].type);
            Assert.Pass(playerLoop.Print());
        }

        [Test]
        public void TryAddSubSystemInsideBottom()
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
                            new PlayerLoopSystem { type = typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate) },
                            new PlayerLoopSystem { type = typeof(PlayerLoops.Update.ScriptRunDelayedTasks) },
                            new PlayerLoopSystem { type = typeof(PlayerLoops.Update.ScriptRunDelayedDynamicFrameRate) }
                        }
                    },
                    new PlayerLoopSystem { type = typeof(PlayerLoops.PostLateUpdate) }
                }
            };

            Type subSystemType = typeof(PlayerLoops.FixedUpdate);
            Type targetSubSystemType = typeof(PlayerLoops.Update);

            bool result0 = UpdateUtility.TryAddSubSystem(ref playerLoop, subSystemType, targetSubSystemType, UpdateSubSystemInsertion.InsideBottom);

            Assert.True(result0);

            PlayerLoopSystem[] subSystems = playerLoop.subSystemList[1].subSystemList;

            Assert.NotNull(subSystems);
            Assert.AreEqual(4, subSystems.Length);
            Assert.AreEqual(typeof(PlayerLoops.FixedUpdate), subSystems[3].type);
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

            bool result0 = UpdateUtility.TryRemoveSubSystem(ref playerLoop, typeof(PlayerLoops.FixedUpdate));

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
