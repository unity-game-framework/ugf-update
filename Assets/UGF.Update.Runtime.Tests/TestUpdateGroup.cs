using System.Linq;
using NUnit.Framework;

namespace UGF.Update.Runtime.Tests
{
    public class TestUpdateGroup
    {
        private class Group : UpdateGroup
        {
            public Group(string name) : base(name, new UpdateSet<Target>())
            {
            }
        }

        private class Target : IUpdateHandler
        {
            public void OnUpdate()
            {
            }
        }

        [Test]
        public void Insert()
        {
            var group = new Group("group_1");
            var group2 = new Group("group_2");

            group.Insert(group2, 0);

            Assert.AreEqual(1, group.SubGroups.Count);
            Assert.True(group.SubGroups.Contains(group2));
        }

        [Test]
        public void Add()
        {
        }

        [Test]
        public void Remove()
        {
        }

        [Test]
        public void Update()
        {
        }

        [Test]
        public void GetSubGroupGeneric()
        {
        }

        [Test]
        public void GetSubGroup()
        {
        }

        [Test]
        public void TryGetSubGroupGeneric()
        {
        }

        [Test]
        public void TryGetSubGroup()
        {
        }
    }
}
