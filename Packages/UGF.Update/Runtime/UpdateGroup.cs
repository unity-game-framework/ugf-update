using System;
using System.Collections.Generic;
using UnityEngine.Profiling;

namespace UGF.Update.Runtime
{
    public class UpdateGroup : UpdateCollectionBase<IUpdateGroup>, IUpdateGroup
    {
        public string Name { get; }
        public bool Enable { get; set; }

        protected override ICollection<IUpdateGroup> Collection { get { return m_groups.Values; } }

        private readonly Dictionary<string, IUpdateGroup> m_groups;

        public UpdateGroup(string name, IEqualityComparer<IUpdateGroup> comparer = null) : base(comparer)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            m_groups = new Dictionary<string, IUpdateGroup>();

            Name = name;
        }

        public override bool Contains(IUpdateGroup handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            return m_groups.ContainsKey(handler.Name);
        }

        public override bool Add(IUpdateGroup handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            if (m_groups.ContainsKey(handler.Name)) throw new ArgumentException("The group with the specified name already exists.", nameof(handler));

            return base.Add(handler);
        }

        public override void Update()
        {
            if (Enable)
            {
                foreach (KeyValuePair<string, IUpdateGroup> pair in m_groups)
                {
                    Profiler.BeginSample(pair.Key);

                    pair.Value.ApplyQueueAndUpdate();

                    Profiler.EndSample();
                }
            }
        }

        public override bool ApplyQueue()
        {
            if (Queue.AnyQueued)
            {
                foreach (IUpdateGroup updateGroup in Queue.Add)
                {
                    m_groups.Add(updateGroup.Name, updateGroup);
                }

                foreach (IUpdateGroup updateGroup in Queue.Remove)
                {
                    m_groups.Remove(updateGroup.Name);
                }

                Queue.Clear();

                return true;
            }

            return false;
        }
    }
}
