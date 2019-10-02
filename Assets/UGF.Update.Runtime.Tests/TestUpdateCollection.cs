using NUnit.Framework;

namespace UGF.Update.Runtime.Tests
{
    public interface ITestTarget
    {
        int Counter { get; }
    }

    public class TestTarget : ITestTarget, IUpdateHandler
    {
        public int Counter { get; private set; }

        public void OnUpdate()
        {
            Counter++;
        }
    }

    public abstract class TestUpdateCollection<TTarget> where TTarget : ITestTarget
    {
        protected abstract IUpdateCollection<TTarget> CreateCollection();
        protected abstract TTarget CreateTarget();

        [Test]
        public void Count()
        {
            IUpdateCollection<TTarget> collection = CreateCollection();
            TTarget target = CreateTarget();

            Assert.AreEqual(0, collection.Count);

            collection.Add(target);

            Assert.AreEqual(1, collection.Count);

            collection.Remove(target);

            Assert.AreEqual(0, collection.Count);

            collection.Add(target);
            collection.ApplyQueue();

            Assert.AreEqual(1, collection.Count);

            collection.Remove(target);

            Assert.AreEqual(0, collection.Count);

            collection.ApplyQueue();

            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void Contains()
        {
            IUpdateCollection<TTarget> collection = CreateCollection();
            TTarget target = CreateTarget();

            bool result0 = collection.Contains(target);

            Assert.False(result0);

            collection.Add(target);

            bool result1 = collection.Contains(target);

            Assert.True(result1);

            collection.ApplyQueue();

            bool result2 = collection.Contains(target);

            Assert.True(result2);

            collection.Remove(target);

            bool result3 = collection.Contains(target);

            Assert.False(result3);
        }

        [Test]
        public void Add()
        {
            IUpdateCollection<TTarget> collection = CreateCollection();
            TTarget target = CreateTarget();

            collection.Add(target);

            Assert.True(collection.Contains(target));
            Assert.True(collection.Queue.AnyQueued);
            Assert.True(collection.Queue.Add.Contains(target));
        }

        [Test]
        public void Remove()
        {
            IUpdateCollection<TTarget> collection = CreateCollection();
            TTarget target = CreateTarget();

            collection.Add(target);

            Assert.True(collection.Contains(target));
            Assert.True(collection.Queue.AnyQueued);
            Assert.True(collection.Queue.Add.Contains(target));

            collection.Remove(target);

            Assert.True(collection.Contains(target));
            Assert.True(collection.Queue.AnyQueued);
            Assert.True(collection.Queue.Add.Contains(target));
            Assert.True(collection.Queue.Remove.Contains(target));
        }

        [Test]
        public void Update()
        {
            IUpdateCollection<TTarget> collection = CreateCollection();
            TTarget target = CreateTarget();

            collection.Add(target);
            collection.ApplyQueue();
            collection.Update();

            Assert.AreEqual(1, target.Counter);

            collection.Update();

            Assert.AreEqual(2, target.Counter);
        }

        [Test]
        public void ApplyQueue()
        {
            IUpdateCollection<TTarget> collection = CreateCollection();
            TTarget target = CreateTarget();

            collection.Add(target);
            collection.ApplyQueue();

            Assert.False(collection.Queue.AnyQueued);
            Assert.True(collection.Contains(target));
        }

        [Test]
        public void ApplyQueueAndUpdate()
        {
            IUpdateCollection<TTarget> collection = CreateCollection();
            TTarget target = CreateTarget();

            collection.Add(target);
            collection.ApplyQueueAndUpdate();

            Assert.AreEqual(1, target.Counter);
        }

        [Test]
        public void Clear()
        {
            IUpdateCollection<TTarget> collection = CreateCollection();
            TTarget target = CreateTarget();

            collection.Add(target);

            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(1, collection.Queue.Add.Count);

            collection.Clear();

            Assert.AreEqual(0, collection.Count);
            Assert.AreEqual(0, collection.Queue.Add.Count);
        }
    }
}
