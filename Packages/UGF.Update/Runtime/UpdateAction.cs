using System;

namespace UGF.Update.Runtime
{
    public struct UpdateAction : IUpdateHandler
    {
        public readonly Action Action;

        public UpdateAction(Action action)
        {
            Action = action;
        }

        public void OnUpdate()
        {
            Action.Invoke();
        }
    }
}
