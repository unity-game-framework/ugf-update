using System.Collections.Generic;

namespace UGF.Update.Runtime
{
    public interface IUpdateGroup
    {
        string Name { get; }
        bool Enable { get; set; }
        IUpdateCollection Collection { get; }
        IReadOnlyList<IUpdateGroup> SubGroups { get; }

        void Add(IUpdateGroup group);
        void Remove(IUpdateGroup group);
        void Insert(IUpdateGroup group, int index);
        void Update();
        T GetCollection<T>() where T : IUpdateCollection;
        bool TryGetCollection<T>(out T collection) where T : IUpdateCollection;
        T GetSubGroup<T>(string name) where T : IUpdateGroup;
        IUpdateGroup GetSubGroup(string name);
        bool TryGetSubGroup<T>(string name, out T group) where T : IUpdateGroup;
        bool TryGetSubGroup(string name, out IUpdateGroup group);
    }
}
