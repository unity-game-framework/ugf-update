using System;

namespace UGF.Update.Runtime
{
    public static class UpdateProviderExtensions
    {
        public static void AddWithSubSystemType(this IUpdateProvider provider, IUpdateGroup updateGroup, Type subSystemType)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (updateGroup == null) throw new ArgumentNullException(nameof(updateGroup));
            if (subSystemType == null) throw new ArgumentNullException(nameof(subSystemType));

            var function = new UpdateGroupFunction(subSystemType, updateGroup.Update);

            provider.Add(updateGroup, function);
        }
    }
}
