using System;
using System.Linq;
using NUnit.Framework;

namespace UGF.Update.Runtime.Tests
{
    public class TestUpdateGroup
    {
        private class Group : UpdateGroup<Target>
        {
            public string Name { get; }

            public Group(string name) : base(new UpdateSet<Target>())
            {
                if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

                Name = name;
            }
        }

        private class Target : IUpdateHandler
        {
            public int Counter { get; private set; }

            public void OnUpdate()
            {
                Counter++;
            }
        }

        [Test]
        public void Add()
        {
            var group = new Group("group_1");
            var group2 = new Group("group_2");

            group.Add(group2);

            Assert.True(group.SubGroups.Contains(group2));
        }

        [Test]
        public void Remove()
        {
            var group = new Group("group_1");
            var group2 = new Group("group_2");

            group.Add(group2);

            Assert.True(group.SubGroups.Contains(group2));

            group.Remove(group2);

            Assert.False(group.SubGroups.Contains(group2));
        }

        [Test]
        public void RemoveByName()
        {
            var group = new Group("group_1");
            var group2 = new Group("group_2");

            group.Add(group2);

            Assert.True(group.SubGroups.Contains(group2));

            group.Remove("group_2");

            Assert.False(group.SubGroups.Contains(group2));
        }

        [Test]
        public void Update()
        {
            var group = new Group("group_1");
            var group2 = new Group("group_2");
            var target = new Target();

            group.Add(group2);
            group2.Collection.Add(target);

            Assert.True(group.SubGroups.Contains(group2));

            group.Update();

            Assert.AreEqual(1, target.Counter);

            group.Enable = false;
            group.Update();

            Assert.AreEqual(1, target.Counter);

            group.Enable = true;
            group2.Enable = false;

            Assert.AreEqual(1, target.Counter);

            group2.Enable = true;
            group.Update();

            Assert.AreEqual(2, target.Counter);

            group2.Update();

            Assert.AreEqual(3, target.Counter);
        }

        [Test]
        public void UpdateRecursive()
        {
            var group = new UpdateGroup<IUpdateGroup>("group", new UpdateListHandler<IUpdateGroup>(item => item.Update()));

            Assert.Throws<ArgumentException>(() => group.Add(group));

            group.Collection.Add(group);

            Assert.Throws<InvalidOperationException>(() => group.Update());
        }

        [Test]
        public void GetSubGroupGeneric()
        {
            var group = new Group("group_1");
            var group2 = new Group("group_2");

            group.Add(group2);

            var result0 = group.GetSubGroup<IUpdateGroup>("group_2");
            var result1 = group.GetSubGroup<IUpdateGroup<Target>>("group_2");

            Assert.NotNull(result0);
            Assert.NotNull(result1);
            Assert.AreEqual(result0, result1);
        }

        [Test]
        public void GetSubGroup()
        {
            var group = new Group("group_1");
            var group2 = new Group("group_2");

            group.Add(group2);

            IUpdateGroup result0 = group.GetSubGroup("group_2");
            IUpdateGroup result1 = group.GetSubGroup("group_2");

            Assert.NotNull(result0);
            Assert.NotNull(result1);
            Assert.AreEqual(result0, result1);
        }

        [Test]
        public void TryGetSubGroupGeneric()
        {
            var group = new Group("group_1");
            var group2 = new Group("group_2");

            group.Add(group2);

            bool result0 = group.TryGetSubGroup("group_2", out IUpdateGroup result01);
            bool result1 = group.TryGetSubGroup("group_2", out IUpdateGroup<Target> result11);
            bool result2 = group.TryGetSubGroup("group_3", out IUpdateGroup<Target> result21);

            Assert.True(result0);
            Assert.True(result1);
            Assert.False(result2);
            Assert.NotNull(result01);
            Assert.NotNull(result11);
            Assert.Null(result21);
            Assert.AreEqual(result01, result11);
        }

        [Test]
        public void TryGetSubGroup()
        {
            var group = new Group("group_1");
            var group2 = new Group("group_2");

            group.Add(group2);

            bool result0 = group.TryGetSubGroup("group_2", out IUpdateGroup result01);
            bool result1 = group.TryGetSubGroup("group_2", out IUpdateGroup result11);
            bool result2 = group.TryGetSubGroup("group_3", out IUpdateGroup result21);

            Assert.True(result0);
            Assert.True(result1);
            Assert.False(result2);
            Assert.NotNull(result01);
            Assert.NotNull(result11);
            Assert.Null(result21);
            Assert.AreEqual(result01, result11);
        }
    }
}
