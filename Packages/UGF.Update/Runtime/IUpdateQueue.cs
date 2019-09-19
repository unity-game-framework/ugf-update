using System.Collections;

namespace UGF.Update.Runtime
{
    public interface IUpdateQueue
    {
        bool AnyQueued { get; }
        IEnumerable Add { get; }
        IEnumerable Remove { get; }

        bool Apply(ICollection collection);
        void Clear();
    }
}
