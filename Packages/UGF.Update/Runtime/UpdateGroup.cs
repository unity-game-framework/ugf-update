using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace UGF.Update.Runtime
{
    public class UpdateGroup : IUpdateGroup
    {
        public string Name { get; }
        public bool Enable { get; set; } = true;
        public IUpdateCollection Collection { get; }
        public IReadOnlyList<IUpdateGroup> SubGroups { get; }

        private readonly List<IUpdateGroup> m_subGroups = new List<IUpdateGroup>();
        private readonly Dictionary<string, IUpdateGroup> m_subGroupsByName = new Dictionary<string, IUpdateGroup>();

        public UpdateGroup(string name, IUpdateCollection collection)
        {
            Name = name;
            Collection = collection;
            SubGroups = new ReadOnlyCollection<IUpdateGroup>(m_subGroups);
        }

        public void Add(IUpdateGroup group)
        {
            m_subGroups.Add(group);
            m_subGroupsByName.Add(group.Name, group);
        }

        public void Remove(IUpdateGroup group)
        {
            if (m_subGroupsByName.Remove(group.Name))
            {
                m_subGroups.Remove(group);
            }
        }

        public void Insert(IUpdateGroup group, int index)
        {
            m_subGroups.Insert(index, group);
            m_subGroupsByName.Add(group.Name, group);
        }

        public void Update()
        {
            if (Enable)
            {
                Collection.ApplyQueueAndUpdate();

                for (int i = 0; i < m_subGroups.Count; i++)
                {
                    m_subGroups[i].Update();
                }
            }
        }

        public T GetSubGroup<T>(string name) where T : IUpdateGroup
        {
            return (T)m_subGroupsByName[name];
        }

        public IUpdateGroup GetSubGroup(string name)
        {
            return m_subGroupsByName[name];
        }

        public bool TryGetSubGroup<T>(string name, out T group) where T : IUpdateGroup
        {
            if (m_subGroupsByName.TryGetValue(name, out IUpdateGroup value) && value is T cast)
            {
                group = cast;
                return true;
            }

            group = default;
            return false;
        }

        public bool TryGetSubGroup(string name, out IUpdateGroup group)
        {
            return m_subGroupsByName.TryGetValue(name, out group);
        }

        public List<IUpdateGroup>.Enumerator GetEnumerator()
        {
            return m_subGroups.GetEnumerator();
        }
    }
}
