using NUnit.Framework;
using UnityEngine.LowLevel;

namespace UGF.Update.Runtime.Tests
{
    public class TestUpdateExtensions
    {
        private class Loop : IUpdateLoop
        {
            private PlayerLoopSystem m_playerLoop = new PlayerLoopSystem
            {
                subSystemList = new[]
                {
                    new PlayerLoopSystem { type = typeof(UnityEngine.PlayerLoop.Update) }
                }
            };

            public PlayerLoopSystem GetPlayerLoop()
            {
                return m_playerLoop;
            }

            public void SetPlayerLoop(PlayerLoopSystem playerLoop)
            {
                m_playerLoop = playerLoop;
            }

            public void Reset()
            {
            }
        }

        private class Group : UpdateGroup<object>
        {
            public Group(string name) : base(name, new UpdateListHandler<object>(item => { }))
            {
            }
        }

        private class Provider : UpdateProvider
        {
            public Provider() : base(new Loop())
            {
            }
        }

        [Test]
        public void ProviderTryFindCollection()
        {
            var provider = new Provider();
            Group group = CreateGroups();

            provider.Add(typeof(UnityEngine.PlayerLoop.Update), group);

            string path0 = "root/one/two/three";
            string path1 = "root/one/two";
            string path2 = "root/one";
            string path3 = "root";
            string path4 = "group";

            bool result0 = provider.TryFindCollection(path0, out IUpdateCollection result01);
            bool result1 = provider.TryFindCollection(path1, out IUpdateCollection result11);
            bool result2 = provider.TryFindCollection(path2, out IUpdateCollection result21);
            bool result3 = provider.TryFindCollection(path3, out IUpdateCollection result31);
            bool result4 = provider.TryFindCollection(path4, out IUpdateCollection result41);

            Assert.True(result0);
            Assert.True(result1);
            Assert.True(result2);
            Assert.True(result3);
            Assert.False(result4);
            Assert.NotNull(result01);
            Assert.NotNull(result11);
            Assert.NotNull(result21);
            Assert.NotNull(result31);
            Assert.Null(result41);
        }

        [Test]
        public void GroupTryFindCollection()
        {
            Group group = CreateGroups();

            string path0 = "one/two/three";
            string path1 = "one/two";
            string path2 = "one";
            string path3 = "root";

            bool result0 = group.TryFindCollection(path0, out IUpdateCollection result01);
            bool result1 = group.TryFindCollection(path1, out IUpdateCollection result11);
            bool result2 = group.TryFindCollection(path2, out IUpdateCollection result21);
            bool result3 = group.TryFindCollection(path3, out IUpdateCollection result31);

            Assert.True(result0);
            Assert.True(result1);
            Assert.True(result2);
            Assert.False(result3);
            Assert.NotNull(result01);
            Assert.NotNull(result11);
            Assert.NotNull(result21);
            Assert.Null(result31);
        }

        [Test]
        public void ProviderTryFindGroup()
        {
            var provider = new Provider();
            Group group = CreateGroups();

            provider.Add(typeof(UnityEngine.PlayerLoop.Update), group);

            string path0 = "root/one/two/three";
            string path1 = "root/one/two";
            string path2 = "root/one";
            string path3 = "root";
            string path4 = "group";

            bool result0 = provider.TryFindGroup(path0, out IUpdateGroup result01);
            bool result1 = provider.TryFindGroup(path1, out IUpdateGroup result11);
            bool result2 = provider.TryFindGroup(path2, out IUpdateGroup result21);
            bool result3 = provider.TryFindGroup(path3, out IUpdateGroup result31);
            bool result4 = provider.TryFindGroup(path4, out IUpdateGroup result41);

            Assert.True(result0);
            Assert.True(result1);
            Assert.True(result2);
            Assert.True(result3);
            Assert.False(result4);
            Assert.NotNull(result01);
            Assert.NotNull(result11);
            Assert.NotNull(result21);
            Assert.NotNull(result31);
            Assert.Null(result41);
            Assert.AreEqual("three", result01.Name);
            Assert.AreEqual("two", result11.Name);
            Assert.AreEqual("one", result21.Name);
            Assert.AreEqual("root", result31.Name);
        }

        [Test]
        public void GroupTryFindGroup()
        {
            Group group = CreateGroups();

            string path0 = "one/two/three";
            string path1 = "one/two";
            string path2 = "one";
            string path3 = "root";

            bool result0 = group.TryFindGroup(path0, out IUpdateGroup result01);
            bool result1 = group.TryFindGroup(path1, out IUpdateGroup result11);
            bool result2 = group.TryFindGroup(path2, out IUpdateGroup result21);
            bool result3 = group.TryFindGroup(path3, out IUpdateGroup result31);

            Assert.True(result0);
            Assert.True(result1);
            Assert.True(result2);
            Assert.False(result3);
            Assert.NotNull(result01);
            Assert.NotNull(result11);
            Assert.NotNull(result21);
            Assert.Null(result31);
            Assert.AreEqual("three", result01.Name);
            Assert.AreEqual("two", result11.Name);
            Assert.AreEqual("one", result21.Name);
        }

        private Group CreateGroups()
        {
            var root = new Group("root");
            var one = new Group("one");
            var two = new Group("two");
            var three = new Group("three");

            root.Add(one);
            one.Add(two);
            two.Add(three);

            return root;
        }
    }
}
