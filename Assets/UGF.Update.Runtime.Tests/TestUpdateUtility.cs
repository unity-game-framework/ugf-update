using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine.LowLevel;
using UnityEngine.TestTools;
using PlayerLoops = UnityEngine.PlayerLoop;

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

        public class Target2
        {
            public int Counter { get; private set; }

            public void Update()
            {
                Counter++;
            }
        }

        private class TestPlayerLoopScope : IDisposable
        {
            public void Dispose()
            {
                UpdateUtility.ResetPlayerLoopToDefault();
            }
        }

        [Test]
        public void SetupPlayer()
        {
            PlayerLoopSystem playerLoop = PlayerLoop.GetDefaultPlayerLoop();

            var target = new Target();

            bool result = UpdateUtility.TryAddUpdateFunction(playerLoop, typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate), target.Update);

            PlayerLoop.SetPlayerLoop(playerLoop);

            Assert.True(result);
        }

        [UnityTest]
        public IEnumerator PlayerLoopInitialization()
        {
            using (new TestPlayerLoopScope())
            {
                yield return TestPlayerLoopUpdate(typeof(PlayerLoops.Initialization));
            }
        }

        [UnityTest]
        public IEnumerator PlayerLoopUpdate()
        {
            using (new TestPlayerLoopScope())
            {
                yield return TestPlayerLoopUpdate(typeof(PlayerLoops.Update));
            }
        }

        [UnityTest]
        public IEnumerator PlayerLoopEarlyUpdate()
        {
            using (new TestPlayerLoopScope())
            {
                yield return TestPlayerLoopUpdate(typeof(PlayerLoops.EarlyUpdate));
            }
        }

        [UnityTest]
        public IEnumerator PlayerLoopFixedUpdate()
        {
            using (new TestPlayerLoopScope())
            {
                yield return TestPlayerLoopUpdate(typeof(PlayerLoops.FixedUpdate));
            }
        }

        [UnityTest]
        public IEnumerator PlayerLoopPreUpdate()
        {
            using (new TestPlayerLoopScope())
            {
                yield return TestPlayerLoopUpdate(typeof(PlayerLoops.PreUpdate));
            }
        }

        [UnityTest]
        public IEnumerator PlayerLoopPostLateUpdate()
        {
            using (new TestPlayerLoopScope())
            {
                yield return TestPlayerLoopUpdate(typeof(PlayerLoops.PostLateUpdate), 2);
            }
        }

        [UnityTest]
        public IEnumerator PlayerLoopPreLateUpdate()
        {
            using (new TestPlayerLoopScope())
            {
                yield return TestPlayerLoopUpdate(typeof(PlayerLoops.PreLateUpdate), 2);
            }
        }

        [UnityTest, Ignore("Not supported by Unity.")]
        public IEnumerator PlayerLoopNested()
        {
            using (new TestPlayerLoopScope())
            {
                yield return TestPlayerLoopUpdate(typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate), 2);
            }
        }

        [UnityTest]
        public IEnumerator PlayerLoopCustom()
        {
            yield return null;

            using (new TestPlayerLoopScope())
            {
                PlayerLoopSystem playerLoop = PlayerLoop.GetDefaultPlayerLoop();

                var target = new Target2();

                bool result1 = UpdateUtility.TryAddSubSystem(ref playerLoop, typeof(PlayerLoops.Update), typeof(Target2), UpdateSubSystemInsertion.InsideBottom);
                bool result2 = UpdateUtility.TryAddUpdateFunction(playerLoop, typeof(Target2), target.Update);

                Assert.True(result1);
                Assert.True(result2);

                PlayerLoop.SetPlayerLoop(playerLoop);

                yield return null;
                yield return null;

                Assert.AreEqual(1, target.Counter);
            }
        }

        [Test, Ignore("Require Unity 2019.3")]
        public void ChangePlayerLoop()
        {
            using (new TestPlayerLoopScope())
            {
                PlayerLoopSystem playerLoop = PlayerLoop.GetDefaultPlayerLoop();

                bool result1 = UpdateUtility.TryAddSubSystem(ref playerLoop, typeof(PlayerLoops.Update), typeof(Target), UpdateSubSystemInsertion.InsideBottom);

                Assert.True(result1);

                PlayerLoop.SetPlayerLoop(playerLoop);

                playerLoop = PlayerLoop.GetDefaultPlayerLoop();

                bool result2 = UpdateUtility.ContainsSubSystem(playerLoop, typeof(Target));

                Assert.True(result2);
            }
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

            bool result = UpdateUtility.ContainsSubSystem(playerLoop, typeof(PlayerLoops.Update.ScriptRunBehaviourUpdate));

            Assert.True(result);
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

            bool result0 = UpdateUtility.TryAddSubSystem(ref playerLoop, targetSubSystemType, subSystemType, UpdateSubSystemInsertion.Before);

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

            bool result0 = UpdateUtility.TryAddSubSystem(ref playerLoop, targetSubSystemType, subSystemType, UpdateSubSystemInsertion.After);

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

            bool result0 = UpdateUtility.TryAddSubSystem(ref playerLoop, targetSubSystemType, subSystemType, UpdateSubSystemInsertion.InsideTop);

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

            bool result0 = UpdateUtility.TryAddSubSystem(ref playerLoop, targetSubSystemType, subSystemType, UpdateSubSystemInsertion.InsideTop);

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

            bool result0 = UpdateUtility.TryAddSubSystem(ref playerLoop, targetSubSystemType, subSystemType, UpdateSubSystemInsertion.InsideBottom);

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

            UpdateUtility.AddSubSystem(ref playerLoop, typeof(PlayerLoops.FixedUpdate), IntPtr.Zero, 1);

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

        private static IEnumerator TestPlayerLoopUpdate(Type subSystemType, int waitFrames = 1, bool waitForStart = true)
        {
            if (waitForStart)
            {
                yield return null;
            }

            PlayerLoopSystem playerLoop = PlayerLoop.GetDefaultPlayerLoop();

            var target = new Target2();

            bool result = UpdateUtility.TryAddUpdateFunction(playerLoop, subSystemType, target.Update);

            Assert.True(result);

            PlayerLoop.SetPlayerLoop(playerLoop);

            for (int i = 0; i < waitFrames; i++)
            {
                yield return null;
            }

            Assert.AreEqual(1, target.Counter);
        }
    }
}
