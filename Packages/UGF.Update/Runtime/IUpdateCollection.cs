using System.Collections;

namespace UGF.Update.Runtime
{
    public interface IUpdateCollection : IEnumerable
    {
        int Count { get; }
        IUpdateQueue Queue { get; }

        void Update();
        bool ApplyQueue();
        bool ApplyQueueAndUpdate();
        void Clear();
    }
}
