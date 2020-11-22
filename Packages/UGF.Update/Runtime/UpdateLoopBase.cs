using System;
using UnityEngine.LowLevel;

namespace UGF.Update.Runtime
{
    public abstract class UpdateLoopBase : IUpdateLoop
    {
        public bool Contains(Type systemType)
        {
            if (systemType == null) throw new ArgumentNullException(nameof(systemType));

            return OnContains(systemType);
        }

        public void Add(Type targetSystem, Type systemType, UpdateSubSystemInsertion insertion)
        {
            if (targetSystem == null) throw new ArgumentNullException(nameof(targetSystem));
            if (systemType == null) throw new ArgumentNullException(nameof(systemType));

            OnAdd(targetSystem, systemType, insertion);
        }

        public bool Remove(Type systemType)
        {
            if (systemType == null) throw new ArgumentNullException(nameof(systemType));

            return OnRemove(systemType);
        }

        public void AddFunction(Type systemType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            if (systemType == null) throw new ArgumentNullException(nameof(systemType));
            if (updateFunction == null) throw new ArgumentNullException(nameof(updateFunction));

            OnAddFunction(systemType, updateFunction);
        }

        public void RemoveFunction(Type systemType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            if (systemType == null) throw new ArgumentNullException(nameof(systemType));
            if (updateFunction == null) throw new ArgumentNullException(nameof(updateFunction));

            OnRemoveFunction(systemType, updateFunction);
        }

        public PlayerLoopSystem GetPlayerLoop()
        {
            return OnGetPlayerLoop();
        }

        public void SetPlayerLoop(PlayerLoopSystem playerLoop)
        {
            OnSetPlayerLoop(playerLoop);
        }

        public void Reset()
        {
            OnReset();
        }

        protected virtual bool OnContains(Type systemType)
        {
            PlayerLoopSystem playerLoop = GetPlayerLoop();

            return UpdateUtility.ContainsSubSystem(playerLoop, systemType);
        }

        protected virtual void OnAdd(Type targetSystem, Type systemType, UpdateSubSystemInsertion insertion)
        {
            PlayerLoopSystem playerLoop = GetPlayerLoop();

            if (UpdateUtility.TryAddSubSystem(ref playerLoop, targetSystem, systemType, insertion))
            {
                SetPlayerLoop(playerLoop);
            }
            else
            {
                throw new ArgumentException($"Adding system failed by the specified target system and system type: '{targetSystem}', '{systemType}'.");
            }
        }

        protected virtual bool OnRemove(Type systemType)
        {
            PlayerLoopSystem playerLoop = GetPlayerLoop();

            if (UpdateUtility.TryRemoveSubSystem(ref playerLoop, systemType))
            {
                SetPlayerLoop(playerLoop);
                return true;
            }

            return false;
        }

        protected virtual void OnAddFunction(Type systemType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            PlayerLoopSystem playerLoop = GetPlayerLoop();

            if (UpdateUtility.TryAddUpdateFunction(playerLoop, systemType, updateFunction))
            {
                SetPlayerLoop(playerLoop);
            }
            else
            {
                throw new ArgumentException($"Adding update function failed by the specified system type: '{systemType}'.");
            }
        }

        protected virtual void OnRemoveFunction(Type systemType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            PlayerLoopSystem playerLoop = GetPlayerLoop();

            if (UpdateUtility.TryRemoveUpdateFunction(playerLoop, systemType, updateFunction))
            {
                SetPlayerLoop(playerLoop);
            }
            else
            {
                throw new AggregateException($"Removing update function failed by the specified system type: '{systemType}'.");
            }
        }

        protected abstract PlayerLoopSystem OnGetPlayerLoop();
        protected abstract void OnSetPlayerLoop(PlayerLoopSystem playerLoop);
        protected abstract void OnReset();
    }
}
