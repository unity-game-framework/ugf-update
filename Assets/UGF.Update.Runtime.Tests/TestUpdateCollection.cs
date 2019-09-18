using System.Linq;
using NUnit.Framework;

namespace UGF.Update.Runtime.Tests
{
    public class TestUpdateCollectionBase
    {
        public class Target : IUpdateHandler
        {
            public int Counter { get; private set; }

            public void OnUpdate()
            {
                Counter++;
            }
        }

        public class Collection : UpdateSet<Target>
        {
        }

        [Test]
        public void HandlerType()
        {
            var collection = new Collection();

            Assert.AreEqual(typeof(Target), collection.HandlerType);
        }

        [Test]
        public void Count()
        {
            var collection = new Collection();

            collection.Add(new Target());
            collection.ApplyQueue();
            collection.Add(new Target());

            Assert.AreEqual(1, collection.Count);
        }

        [Test]
        public void AnyQueued()
        {
            var collection = new Collection();

            Assert.False(collection.AnyQueued);

            collection.Add(new Target());

            Assert.True(collection.AnyQueued);

            collection.Clear();
            collection.Remove(new Target());

            Assert.True(collection.AnyQueued);
        }

        [Test]
        public void QueueAddCount()
        {
            var collection = new Collection();

            Assert.AreEqual(0, collection.QueueAddCount);

            collection.Add(new Target());

            Assert.AreEqual(1, collection.QueueAddCount);
        }

        [Test]
        public void QueueRemoveCount()
        {
            var collection = new Collection();

            Assert.AreEqual(0, collection.QueueRemoveCount);

            collection.Remove(new Target());

            Assert.AreEqual(1, collection.QueueRemoveCount);
        }

        [Test]
        public void QueueAdd()
        {
            var collection = new Collection();

            collection.Add(new Target());

            Assert.True(collection.QueueAdd.Any());
        }

        [Test]
        public void QueueRemove()
        {
            var collection = new Collection();

            collection.Remove(new Target());

            Assert.True(collection.QueueRemove.Any());
        }

        [Test]
        public void Contains()
        {
            var target = new Target();
            var collection = new Collection();

            collection.Add(target);

            Assert.False(collection.Contains(target));

            collection.ApplyQueue();

            Assert.True(collection.Contains(target));

            collection.Remove(target);

            Assert.True(collection.Contains(target));

            collection.ApplyQueue();

            Assert.False(collection.Contains(target));
        }

        [Test]
        public void ContainsQueueAdd()
        {
            var target = new Target();
            var collection = new Collection();

            collection.Add(target);

            Assert.True(collection.ContainsQueueAdd(target));

            collection.ApplyQueue();

            Assert.False(collection.ContainsQueueAdd(target));
        }

        [Test]
        public void ContainsQueueRemove()
        {
            var target = new Target();
            var collection = new Collection();

            collection.Remove(target);

            Assert.True(collection.ContainsQueueRemove(target));

            collection.ApplyQueue();

            Assert.False(collection.ContainsQueueRemove(target));
        }

        [Test]
        public void Add()
        {
            var target = new Target();
            var collection = new Collection();

            bool result = collection.Add(target);

            collection.ApplyQueue();

            Assert.True(result);
            Assert.True(collection.Contains(target));
        }

        [Test]
        public void Remove()
        {
            var target = new Target();
            var collection = new Collection();

            collection.Add(target);
            collection.Remove(target);

            Assert.False(collection.Contains(target));
        }

        [Test]
        public void Update()
        {
            var target = new Target();
            var collection = new Collection();

            collection.Add(target);
            collection.ApplyQueue();
            collection.Update();

            Assert.AreEqual(1, target.Counter);
        }

        [Test]
        public void ApplyQueue()
        {
            var target = new Target();
            var collection = new Collection();

            collection.Add(target);

            Assert.AreEqual(0, collection.Count);
            Assert.AreEqual(1, collection.QueueAddCount);

            bool result = collection.ApplyQueue();

            Assert.True(result);
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(0, collection.QueueAddCount);
        }

        [Test]
        public void ApplyQueueAndUpdate()
        {
            var target = new Target();
            var collection = new Collection();

            collection.Add(target);

            Assert.AreEqual(0, collection.Count);
            Assert.AreEqual(1, collection.QueueAddCount);
            Assert.AreEqual(0, target.Counter);

            bool result = collection.ApplyQueueAndUpdate();

            Assert.True(result);
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(0, collection.QueueAddCount);
            Assert.AreEqual(1, target.Counter);
        }

        [Test]
        public void ClearQueue()
        {
            var collection = new Collection();

            collection.Add(new Target());
            collection.Remove(new Target());

            Assert.AreEqual(1, collection.QueueAddCount);
            Assert.AreEqual(1, collection.QueueRemoveCount);

            collection.ClearQueue();

            Assert.AreEqual(0, collection.QueueAddCount);
            Assert.AreEqual(0, collection.QueueRemoveCount);
        }

        [Test]
        public void Clear()
        {
            var collection = new Collection();

            collection.Add(new Target());
            collection.ApplyQueue();

            Assert.AreEqual(1, collection.Count);

            collection.Clear();

            Assert.AreEqual(0, collection.Count);
        }
    }
}
