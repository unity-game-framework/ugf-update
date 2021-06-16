using System;
using Unity.Profiling;

namespace UGF.Update.Runtime
{
    public class UpdateGroup : IUpdateGroup
    {
        public bool Enable { get; set; } = true;
        public IUpdateCollection Collection { get; }
        public IUpdateCollection<IUpdateGroup> SubGroups { get; }

        private bool m_updating;
        private ProfilerMarker m_marker;

        public UpdateGroup(IUpdateCollection collection) : this(collection, new UpdateListHandler<IUpdateGroup>(item => item.Update()))
        {
        }

        public UpdateGroup(IUpdateCollection collection, IUpdateCollection<IUpdateGroup> subGroups)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
            SubGroups = subGroups ?? throw new ArgumentNullException(nameof(subGroups));

#if ENABLE_PROFILER
            m_marker = new ProfilerMarker($"{GetType()}");
#else
            m_marker = default;
#endif
        }

        public void Update()
        {
            if (m_updating) throw new InvalidOperationException("Update group already updating, possible recursive call.");

            if (Enable)
            {
                m_marker.Begin();
                m_updating = true;

                Collection.ApplyQueue();
                SubGroups.ApplyQueue();

                Collection.Update();
                SubGroups.Update();

                m_updating = false;
                m_marker.End();
            }
        }
    }
}
