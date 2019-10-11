using System;
using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    public interface IUpdateProvider
    {
        IReadOnlyDictionary<string, IUpdateGroup> Groups { get; }

        void Add(Type subSystemType, IUpdateGroup updateGroup);
        void Remove(string groupName);
        void Clear();
        Type GetSubSystemType(string groupName);
        bool TryGetSubSystemType(string groupName, out Type type);
        bool TryGetGroup<T>(string groupName, out T updateGroup) where T : IUpdateGroup;
    }
}
