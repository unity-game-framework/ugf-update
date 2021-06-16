using System;

namespace UGF.Update.Runtime
{
    public static class UpdateCollectionExtensions
    {
        public static bool ApplyQueueAndUpdate(this IUpdateCollection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            bool changed = collection.ApplyQueue();

            collection.Update();

            return changed;
        }
    }
}
