using System;
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
        public void Update()
        {
            var group = new Group("group_1");
            var group2 = new Group("group_2");
            var target = new Target();

            group.SubGroups.Add(group2);
            group.SubGroups.ApplyQueue();
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
            var group = new UpdateGroup<IUpdateGroup>(new UpdateListHandler<IUpdateGroup>(item => item.Update()));

            group.Collection.Add(group);

            Assert.Throws<InvalidOperationException>(() => group.Update());
        }
    }
}
