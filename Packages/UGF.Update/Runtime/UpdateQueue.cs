using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    public class UpdateQueue<TItem>
    {
        public HashSet<TItem> Add { get; }
        public HashSet<TItem> Remove { get; }
        public IEqualityComparer<TItem> Comparer { get { return Add.Comparer; } }
        public bool AnyQueued { get { return Add.Count > 0 || Remove.Count > 0; } }

        public UpdateQueue(IEqualityComparer<TItem> comparer = null)
        {
            Add = new HashSet<TItem>(comparer);
            Remove = new HashSet<TItem>(comparer);
        }

        public bool Apply(ICollection<TItem> collection)
        {
            if (Add.Count > 0 || Remove.Count > 0)
            {
                foreach (TItem item in Add)
                {
                    collection.Add(item);
                }

                foreach (TItem item in Remove)
                {
                    collection.Remove(item);
                }

                Add.Clear();
                Remove.Clear();

                return true;
            }

            return false;
        }

        public void Clear()
        {
            Add.Clear();
            Remove.Clear();
        }
    }
}
