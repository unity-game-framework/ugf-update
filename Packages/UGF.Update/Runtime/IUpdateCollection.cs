using System;
using System.Collections;

namespace UGF.Update.Runtime
{
    public interface IUpdateCollection : IEnumerable
    {
        Type HandlerType { get; }
        int Count { get; }
        bool AnyQueued { get; }
        int QueueAddCount { get; }
        int QueueRemoveCount { get; }

        void Update();
        bool ApplyQueue();
        bool ApplyQueueAndUpdate();
        void ClearQueue();
        void Clear();
    }
}
