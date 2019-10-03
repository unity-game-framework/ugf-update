using System.Collections.Generic;
using NUnit.Framework;

namespace UGF.Update.Runtime.Tests
{
    public class TestUpdateQueueSet
    {
        [Test]
        public void AnyQueued()
        {
            var queue = new UpdateQueueSet<int>();

            Assert.False(queue.AnyQueued);

            bool result0 = queue.Add.Add(0);

            Assert.True(result0);
            Assert.True(queue.AnyQueued);

            bool result1 = queue.Remove.Add(1);

            Assert.True(result1);
            Assert.True(queue.AnyQueued);

            bool result2 = queue.Add.Remove(0);

            Assert.True(result2);
            Assert.True(queue.AnyQueued);

            bool result3 = queue.Remove.Remove(1);

            Assert.True(result3);
            Assert.False(queue.AnyQueued);
        }

        [Test]
        public void Apply()
        {
            var queue = new UpdateQueueSet<int>();
            var collection = new List<int>();

            queue.Add.Add(0);
            queue.Add.Add(1);

            bool result0 = queue.Apply(collection);

            Assert.True(result0);
            Assert.False(queue.AnyQueued);
            Assert.AreEqual(0, queue.Add.Count);
            Assert.Contains(0, collection);
            Assert.Contains(1, collection);

            queue.Remove.Add(0);
            queue.Remove.Add(1);

            bool result1 = queue.Apply(collection);

            Assert.True(result1);
            Assert.False(queue.AnyQueued);
            Assert.AreEqual(0, queue.Add.Count);
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void Clear()
        {
            var queue = new UpdateQueueSet<int>();

            queue.Add.Add(0);
            queue.Remove.Add(1);

            Assert.True(queue.AnyQueued);

            queue.Clear();

            Assert.False(queue.AnyQueued);
            Assert.AreEqual(0, queue.Add.Count);
            Assert.AreEqual(0, queue.Remove.Count);
        }
    }
}
