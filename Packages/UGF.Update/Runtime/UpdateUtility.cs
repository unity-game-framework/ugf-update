using System;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine.Experimental.LowLevel;

namespace UGF.Update.Runtime
{
    public static class UpdateUtility
    {
        public static bool TryAddUpdateFunction(PlayerLoopSystem playerLoop, Type subSystemType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            return InternalTryChangeUpdateFunction(playerLoop, subSystemType, updateFunction, true);
        }

        public static bool TryRemoveUpdateFunction(PlayerLoopSystem playerLoop, Type subSystemType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            return InternalTryChangeUpdateFunction(playerLoop, subSystemType, updateFunction, false);
        }

        public static bool ContainsSubSystem(PlayerLoopSystem playerLoop, Type subSystemType)
        {
            if (subSystemType == null) throw new ArgumentNullException(nameof(subSystemType));

            PlayerLoopSystem[] subSystems = playerLoop.subSystemList;

            if (subSystems != null)
            {
                for (int i = 0; i < subSystems.Length; i++)
                {
                    PlayerLoopSystem subSystem = subSystems[i];

                    if (subSystem.type != null && subSystem.type == subSystemType)
                    {
                        return true;
                    }

                    if (ContainsSubSystem(subSystem, subSystemType))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool TryAddSubSystem(ref PlayerLoopSystem playerLoop, Type subSystemType, Type targetSubSystemType, UpdateSubSystemInsertion insertion)
        {
            if (subSystemType == null) throw new ArgumentNullException(nameof(subSystemType));
            if (targetSubSystemType == null) throw new ArgumentNullException(nameof(targetSubSystemType));

            PlayerLoopSystem[] subSystems = playerLoop.subSystemList;

            if (subSystems != null)
            {
                for (int i = 0; i < subSystems.Length; i++)
                {
                    PlayerLoopSystem subSystem = subSystems[i];

                    if (subSystem.type != null && subSystem.type == targetSubSystemType)
                    {
                        switch (insertion)
                        {
                            case UpdateSubSystemInsertion.Before:
                            {
                                AddSubSystem(ref playerLoop, subSystemType, i);
                                break;
                            }
                            case UpdateSubSystemInsertion.After:
                            {
                                AddSubSystem(ref playerLoop, subSystemType, i + 1);
                                break;
                            }
                            case UpdateSubSystemInsertion.InsideTop:
                            {
                                AddSubSystem(ref subSystem, subSystemType, 0);

                                subSystems[i] = subSystem;
                                break;
                            }
                            case UpdateSubSystemInsertion.InsideBottom:
                            {
                                int index = subSystem.subSystemList?.Length ?? 0;

                                AddSubSystem(ref subSystem, subSystemType, index);

                                subSystems[i] = subSystem;
                                break;
                            }
                            default: throw new ArgumentOutOfRangeException(nameof(insertion), insertion, "Unknown insertion.");
                        }

                        return true;
                    }

                    if (TryAddSubSystem(ref subSystem, subSystemType, targetSubSystemType, insertion))
                    {
                        subSystems[i] = subSystem;
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool TryRemoveSubSystem(ref PlayerLoopSystem playerLoop, Type subSystemType)
        {
            if (subSystemType == null) throw new ArgumentNullException(nameof(subSystemType));

            PlayerLoopSystem[] subSystems = playerLoop.subSystemList;

            if (subSystems != null)
            {
                for (int i = 0; i < subSystems.Length; i++)
                {
                    PlayerLoopSystem subSystem = subSystems[i];

                    if (subSystem.type != null && subSystem.type == subSystemType)
                    {
                        RemoveSubSystem(ref playerLoop, i);
                        return true;
                    }

                    if (TryRemoveSubSystem(ref subSystem, subSystemType))
                    {
                        subSystems[i] = subSystem;
                        return true;
                    }
                }
            }

            return false;
        }

        public static void AddSubSystem(ref PlayerLoopSystem playerLoop, Type subSystemType, int index)
        {
            if (subSystemType == null) throw new ArgumentNullException(nameof(subSystemType));
            if (index < 0) throw new ArgumentException("The specified index less than zero.");
            if (playerLoop.subSystemList != null && index > playerLoop.subSystemList.Length) throw new ArgumentException("The specified index more than length of the subsystems.");

            PlayerLoopSystem[] subSystems = playerLoop.subSystemList;
            var subSystem = new PlayerLoopSystem { type = subSystemType };

            if (subSystems == null)
            {
                playerLoop.subSystemList = new[] { subSystem };
            }
            else
            {
                var array = new PlayerLoopSystem[subSystems.Length + 1];

                Array.Copy(subSystems, 0, array, 0, index);
                Array.Copy(subSystems, index, array, index + 1, subSystems.Length - index);

                array[index] = subSystem;

                playerLoop.subSystemList = array;
            }
        }

        public static void RemoveSubSystem(ref PlayerLoopSystem playerLoop, int index)
        {
            if (index < 0) throw new ArgumentException("The specified index less than zero.");
            if (playerLoop.subSystemList == null) throw new ArgumentException("The specified player loop does not contains any subsystems.");
            if (index >= playerLoop.subSystemList.Length) throw new ArgumentException("The specified index more than length of the subsystems.");

            PlayerLoopSystem[] subSystems = playerLoop.subSystemList;

            if (subSystems.Length == 1)
            {
                playerLoop.subSystemList = null;
            }
            else
            {
                var array = new PlayerLoopSystem[subSystems.Length - 1];

                Array.Copy(subSystems, 0, array, 0, index);
                Array.Copy(subSystems, index + 1, array, index, subSystems.Length - index - 1);

                playerLoop.subSystemList = array;
            }
        }

        public static string PrintPlayerLoop(PlayerLoopSystem playerLoop, int depth = 0, string indent = "    ")
        {
            var builder = new StringBuilder();

            PrintPlayerLoop(builder, playerLoop, depth, indent);

            return builder.ToString();
        }

        public static void PrintPlayerLoop(StringBuilder builder, PlayerLoopSystem playerLoop, int depth = 0, string indent = "    ")
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            Type type = playerLoop.type;

            string name = type != null
                ? !string.IsNullOrEmpty(type.Namespace) ? $"{type.Namespace}.{type.Name}" : type.Name
                : "PlayerLoopSystem";

            builder.Append(string.Concat(Enumerable.Repeat(indent, depth)));
            builder.Append(name);
            builder.AppendLine();

            if (playerLoop.updateDelegate != null)
            {
                Delegate[] invocations = playerLoop.updateDelegate.GetInvocationList();

                if (invocations.Length > 0)
                {
                    builder.Append(string.Concat(Enumerable.Repeat(indent, depth + 1)));
                    builder.Append("Invocations");
                    builder.AppendLine();

                    for (int i = 0; i < invocations.Length; i++)
                    {
                        Delegate invocation = invocations[i];
                        MethodInfo method = invocation.Method;
                        object target = invocation.Target;

                        builder.Append(string.Concat(Enumerable.Repeat(indent, depth + 2)));
                        builder.Append($"{method.DeclaringType}::{method}");

                        if (target != null)
                        {
                            builder.Append($" ({target})");
                        }

                        builder.AppendLine();
                    }
                }
            }

            if (playerLoop.subSystemList != null)
            {
                PlayerLoopSystem[] subSystems = playerLoop.subSystemList;

                if (subSystems.Length > 0)
                {
                    builder.Append(string.Concat(Enumerable.Repeat(indent, depth + 1)));
                    builder.Append("SubSystems");
                    builder.AppendLine();

                    for (int i = 0; i < playerLoop.subSystemList.Length; i++)
                    {
                        PlayerLoopSystem subSystem = playerLoop.subSystemList[i];

                        PrintPlayerLoop(builder, subSystem, depth + 2, indent);
                    }
                }
            }
        }

        private static bool InternalTryChangeUpdateFunction(PlayerLoopSystem playerLoop, Type subSystemType, PlayerLoopSystem.UpdateFunction updateFunction, bool isAdding)
        {
            if (subSystemType == null) throw new ArgumentNullException(nameof(subSystemType));
            if (updateFunction == null) throw new ArgumentNullException(nameof(updateFunction));

            PlayerLoopSystem[] subSystems = playerLoop.subSystemList;

            if (subSystems != null)
            {
                for (int i = 0; i < subSystems.Length; i++)
                {
                    PlayerLoopSystem subSystem = subSystems[i];

                    if (subSystem.type != null && subSystem.type == subSystemType)
                    {
                        if (isAdding)
                        {
                            subSystem.updateDelegate += updateFunction;
                        }
                        else
                        {
                            subSystem.updateDelegate -= updateFunction;
                        }

                        subSystems[i] = subSystem;
                        return true;
                    }

                    if (InternalTryChangeUpdateFunction(subSystem, subSystemType, updateFunction, isAdding))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
