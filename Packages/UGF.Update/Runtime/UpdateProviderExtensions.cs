using System;

namespace UGF.Update.Runtime
{
    public static class UpdateProviderExtensions
    {
        public static void Add(this IUpdateProvider provider, Type subSystemType, IUpdateGroup updateGroup)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (subSystemType == null) throw new ArgumentNullException(nameof(subSystemType));
            if (updateGroup == null) throw new ArgumentNullException(nameof(updateGroup));

            var function = new UpdateGroupFunction(subSystemType, updateGroup.Update);

            provider.Add(updateGroup, function);
        }
    }
}
