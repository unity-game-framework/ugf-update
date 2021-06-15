using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine.LowLevel;
using UnityEngine.TestTools;
using PlayerLoops = UnityEngine.PlayerLoop;

namespace UGF.Update.Runtime.Tests
{
    public class TestUpdateProvider
    {
        private class Loop : UpdateLoopBase
        {
            private PlayerLoopSystem m_playerLoop;

            public Loop()
            {
                Reset();
            }

            protected override PlayerLoopSystem OnGetPlayerLoop()
            {
                return m_playerLoop;
            }

            protected override void OnSetPlayerLoop(PlayerLoopSystem playerLoop)
            {
                m_playerLoop = playerLoop;
            }

            protected override void OnReset()
            {
                m_playerLoop = new PlayerLoopSystem
                {
                    subSystemList = new[]
                    {
                        new PlayerLoopSystem { type = typeof(PlayerLoops.PreUpdate) },
                        new PlayerLoopSystem { type = typeof(PlayerLoops.Update) },
                        new PlayerLoopSystem { type = typeof(PlayerLoops.PostLateUpdate) }
                    }
                };
            }
        }

        private class Group : UpdateGroup<object>
        {
            public string Name { get; }

            public Group(string name) : base(new UpdateListHandler<object>(item => { }))
            {
                if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

                Name = name;
            }
        }

        private class Target : IUpdateHandler
        {
            public int Counter { get; protected set; }

            public void OnUpdate()
            {
                Counter++;
            }
        }

        [Test]
        public void Add()
        {
            var provider = new UpdateProvider(new Loop());
            var group = new Group("group");

            provider.Add(typeof(PlayerLoops.Update), group);

            Assert.True(provider.Entries.ContainsKey(group));
            Assert.NotNull(provider.UpdateLoop.GetPlayerLoop().subSystemList[1].updateDelegate);
            Assert.True(provider.UpdateLoop.GetPlayerLoop().subSystemList[1].updateDelegate.GetInvocationList().Any(x => x.Target == group));
        }

        [UnityTest]
        public IEnumerator AddUnity()
        {
            var provider = new UpdateProvider();
            var group = new UpdateGroup<IUpdateHandler>(new UpdateList<IUpdateHandler>());
            var target = new Target();

            provider.Add(typeof(PlayerLoops.Update), group);
            group.Collection.Add(target);

            Assert.True(provider.Entries.ContainsKey(group));
            Assert.NotNull(provider.UpdateLoop.GetPlayerLoop().subSystemList[5].updateDelegate);
            Assert.True(provider.UpdateLoop.GetPlayerLoop().subSystemList[5].updateDelegate.GetInvocationList().Any(x => x.Target == group));

            yield return null;

            Assert.AreEqual(1, target.Counter);

            provider.Remove(group);

            Assert.False(provider.Entries.ContainsKey(group));
            Assert.Null(provider.UpdateLoop.GetPlayerLoop().subSystemList[5].updateDelegate);
        }

        [Test]
        public void Remove()
        {
            var provider = new UpdateProvider(new Loop());
            var group = new Group("group");

            provider.Add(typeof(PlayerLoops.Update), group);

            Assert.True(provider.Entries.ContainsKey(group));
            Assert.NotNull(provider.UpdateLoop.GetPlayerLoop().subSystemList[1].updateDelegate);
            Assert.True(provider.UpdateLoop.GetPlayerLoop().subSystemList[1].updateDelegate.GetInvocationList().Any(x => x.Target == group));

            provider.Remove(group);

            Assert.False(provider.Entries.ContainsKey(group));
            Assert.AreEqual(0, provider.Entries.Count);
            Assert.Null(provider.UpdateLoop.GetPlayerLoop().subSystemList[1].updateDelegate);
        }

        [Test]
        public void Clear()
        {
            var provider = new UpdateProvider(new Loop());
            var group1 = new Group("group1");
            var group2 = new Group("group2");

            provider.Add(typeof(PlayerLoops.PreUpdate), group1);
            provider.Add(typeof(PlayerLoops.Update), group2);

            Assert.AreEqual(2, provider.Entries.Count);
            Assert.True(provider.Entries.ContainsKey(group1));
            Assert.True(provider.Entries.ContainsKey(group2));
            Assert.NotNull(provider.UpdateLoop.GetPlayerLoop().subSystemList[0].updateDelegate);
            Assert.NotNull(provider.UpdateLoop.GetPlayerLoop().subSystemList[1].updateDelegate);
            Assert.True(provider.UpdateLoop.GetPlayerLoop().subSystemList[0].updateDelegate.GetInvocationList().Any(x => x.Target == group1));
            Assert.True(provider.UpdateLoop.GetPlayerLoop().subSystemList[1].updateDelegate.GetInvocationList().Any(x => x.Target == group2));

            provider.Clear();

            Assert.AreEqual(0, provider.Entries.Count);
            Assert.False(provider.Entries.ContainsKey(group1));
            Assert.False(provider.Entries.ContainsKey(group2));
            Assert.Null(provider.UpdateLoop.GetPlayerLoop().subSystemList[0].updateDelegate);
            Assert.Null(provider.UpdateLoop.GetPlayerLoop().subSystemList[1].updateDelegate);
        }
    }
}
