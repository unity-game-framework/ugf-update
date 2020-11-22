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
            public Group(string name) : base(name, new UpdateListHandler<object>(item => { }))
            {
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

            Assert.True(provider.Groups.ContainsKey("group"));
            Assert.NotNull(provider.UpdateLoop.GetPlayerLoop().subSystemList[1].updateDelegate);
            Assert.True(provider.UpdateLoop.GetPlayerLoop().subSystemList[1].updateDelegate.GetInvocationList().Any(x => x.Target == group));
        }

        [UnityTest]
        public IEnumerator AddUnity()
        {
            var provider = new UpdateProvider();
            var group = new UpdateGroup<IUpdateHandler>("group", new UpdateList<IUpdateHandler>());
            var target = new Target();

            provider.Add(typeof(PlayerLoops.Update), group);
            group.Collection.Add(target);

            Assert.True(provider.Groups.ContainsKey("group"));
            Assert.NotNull(provider.UpdateLoop.GetPlayerLoop().subSystemList[5].updateDelegate);
            Assert.True(provider.UpdateLoop.GetPlayerLoop().subSystemList[5].updateDelegate.GetInvocationList().Any(x => x.Target == group));

            yield return null;

            Assert.AreEqual(1, target.Counter);

            provider.Remove(group);

            Assert.False(provider.Groups.ContainsKey("group"));
            Assert.Null(provider.UpdateLoop.GetPlayerLoop().subSystemList[5].updateDelegate);
        }

        [Test]
        public void Remove()
        {
            var provider = new UpdateProvider(new Loop());
            var group = new Group("group");

            provider.Add(typeof(PlayerLoops.Update), group);

            Assert.True(provider.Groups.ContainsKey("group"));
            Assert.NotNull(provider.UpdateLoop.GetPlayerLoop().subSystemList[1].updateDelegate);
            Assert.True(provider.UpdateLoop.GetPlayerLoop().subSystemList[1].updateDelegate.GetInvocationList().Any(x => x.Target == group));

            provider.Remove("group");

            Assert.False(provider.Groups.ContainsKey("group"));
            Assert.AreEqual(0, provider.Groups.Count);
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

            Assert.AreEqual(2, provider.Groups.Count);
            Assert.True(provider.Groups.ContainsKey("group1"));
            Assert.True(provider.Groups.ContainsKey("group2"));
            Assert.NotNull(provider.UpdateLoop.GetPlayerLoop().subSystemList[0].updateDelegate);
            Assert.NotNull(provider.UpdateLoop.GetPlayerLoop().subSystemList[1].updateDelegate);
            Assert.True(provider.UpdateLoop.GetPlayerLoop().subSystemList[0].updateDelegate.GetInvocationList().Any(x => x.Target == group1));
            Assert.True(provider.UpdateLoop.GetPlayerLoop().subSystemList[1].updateDelegate.GetInvocationList().Any(x => x.Target == group2));

            provider.Clear();

            Assert.AreEqual(0, provider.Groups.Count);
            Assert.False(provider.Groups.ContainsKey("group1"));
            Assert.False(provider.Groups.ContainsKey("group2"));
            Assert.Null(provider.UpdateLoop.GetPlayerLoop().subSystemList[0].updateDelegate);
            Assert.Null(provider.UpdateLoop.GetPlayerLoop().subSystemList[1].updateDelegate);
        }

        [Test]
        public void GetSubSystemType()
        {
            var provider = new UpdateProvider(new Loop());
            var group = new Group("group");

            provider.Add(typeof(PlayerLoops.Update), group);

            Type result = provider.GetSubSystemType("group");

            Assert.NotNull(result);
            Assert.AreEqual(typeof(PlayerLoops.Update), result);
        }

        [Test]
        public void TryGetSubSystemType()
        {
            var provider = new UpdateProvider(new Loop());
            var group = new Group("group");

            provider.Add(typeof(PlayerLoops.Update), group);

            bool result0 = provider.TryGetSubSystemType("group", out Type result01);
            bool result1 = provider.TryGetSubSystemType("group1", out Type result11);

            Assert.True(result0);
            Assert.False(result1);
            Assert.NotNull(result01);
            Assert.Null(result11);
            Assert.AreEqual(typeof(PlayerLoops.Update), result01);
        }

        [Test]
        public void TryGetGroup()
        {
            var provider = new UpdateProvider(new Loop());
            var group = new Group("group");

            provider.Add(typeof(PlayerLoops.Update), group);

            bool result0 = provider.TryGetGroup("group", out Group result01);
            bool result1 = provider.TryGetGroup("group", out IUpdateGroup result11);
            bool result2 = provider.TryGetGroup("group2", out IUpdateGroup result21);

            Assert.True(result0);
            Assert.True(result1);
            Assert.False(result2);
            Assert.NotNull(result01);
            Assert.NotNull(result11);
            Assert.Null(result21);
            Assert.AreEqual(group, result01);
            Assert.AreEqual(group, result11);
        }
    }
}
