using System;
using System.Collections.Generic;
using UGF.RuntimeTools.Runtime.Providers;

namespace UGF.Update.Runtime
{
    public class UpdateProvider : Provider<IUpdateGroup, UpdateGroupFunction>, IUpdateProvider
    {
        public IUpdateLoop UpdateLoop { get; }

        public UpdateProvider(IUpdateLoop updateLoop)
        {
            UpdateLoop = updateLoop ?? throw new ArgumentNullException(nameof(updateLoop));
        }

        protected override void OnAdd(IUpdateGroup id, UpdateGroupFunction entry)
        {
            base.OnAdd(id, entry);

            UpdateLoop.AddFunction(entry.SubSystemType, entry.UpdateFunction);
        }

        protected override bool OnRemove(IUpdateGroup id, UpdateGroupFunction entry)
        {
            if (base.OnRemove(id, entry))
            {
                UpdateLoop.RemoveFunction(entry.SubSystemType, entry.UpdateFunction);
                return true;
            }

            return false;
        }

        protected override void OnClear()
        {
            foreach (KeyValuePair<IUpdateGroup, UpdateGroupFunction> pair in this)
            {
                UpdateLoop.RemoveFunction(pair.Value.SubSystemType, pair.Value.UpdateFunction);
            }

            base.OnClear();
        }
    }
}
