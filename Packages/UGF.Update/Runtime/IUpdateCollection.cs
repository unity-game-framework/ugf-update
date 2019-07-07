using System;
using System.Collections;

namespace UGF.Update.Runtime
{
    public interface IUpdateCollection : IEnumerable
    {
        Type HandlerType { get; }
        int Count { get; }
        bool AnyQueued { get; }

        void Update();
        void ApplyQueue();
        void ApplyQueueAndUpdate();
        void ClearQueue();
        void Clear();
    }
}
