using System;
using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    public interface IUpdateProvider
    {
        IReadOnlyDictionary<string, IUpdateGroup> Groups { get; }

        void Add(Type subSystemType, IUpdateGroup updateGroup);
        void Remove(string groupName);
        Type GetSubSystemType(string groupName);
        bool TryGetGroup<T>(string groupName, out T updateGroup) where T : IUpdateGroup;
    }
}
