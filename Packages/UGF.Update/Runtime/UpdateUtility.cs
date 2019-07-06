using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine.Experimental.LowLevel;

namespace UGF.Update.Runtime
{
    public static class UpdateUtility
    {
        public static bool TryAddUpdateFunction(PlayerLoopSystem playerLoop, IReadOnlyList<Type> path, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            return InternalTryChangeUpdateFunction(playerLoop, path, updateFunction, 0, true);
        }

        public static bool TryRemoveUpdateFunction(PlayerLoopSystem playerLoop, IReadOnlyList<Type> path, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            return InternalTryChangeUpdateFunction(playerLoop, path, updateFunction, 0, false);
        }

        public static bool TryAddSubSystem(PlayerLoopSystem playerLoop, IReadOnlyList<Type> path, Type subSystemType, int index)
        {
            return InternalTryChangeSubSystem(playerLoop, path, subSystemType, index, 0, true);
        }

        public static bool TryRemoveSubSystem(PlayerLoopSystem playerLoop, IReadOnlyList<Type> path, int index)
        {
            return InternalTryChangeSubSystem(playerLoop, path, null, index, 0, false);
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
            if (index > playerLoop.subSystemList.Length) throw new ArgumentException("The specified index more than length of the subsystems.");

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
                : "PlayerLoopSystem (No type)";

            builder.Append(string.Concat(Enumerable.Repeat(indent, depth)));
            builder.Append(name);
            builder.AppendLine();

            if (playerLoop.updateFunction != IntPtr.Zero)
            {
                builder.Append(string.Concat(Enumerable.Repeat(indent, depth + 1)));
                builder.Append("updateFunction: ");
                builder.Append(playerLoop.updateFunction.ToString());
                builder.AppendLine();
            }

            if (playerLoop.loopConditionFunction != IntPtr.Zero)
            {
                builder.Append(string.Concat(Enumerable.Repeat(indent, depth + 1)));
                builder.Append("loopConditionFunction: ");
                builder.Append(playerLoop.loopConditionFunction.ToString());
                builder.AppendLine();
            }

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

        private static bool InternalTryChangeUpdateFunction(PlayerLoopSystem playerLoop, IReadOnlyList<Type> path, PlayerLoopSystem.UpdateFunction updateFunction, int pathIndex, bool isAdding)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (path.Count == 0) throw new InvalidOperationException("The specified path is empty.");
            if (updateFunction == null) throw new ArgumentNullException(nameof(updateFunction));
            if (pathIndex < 0) throw new ArgumentException("The specified path index less than zero.");
            if (pathIndex >= path.Count) throw new ArgumentException("The specified path index more or equal than path count.");

            PlayerLoopSystem[] subSystems = playerLoop.subSystemList;

            if (subSystems != null)
            {
                Type type = path[pathIndex];

                for (int i = 0; i < subSystems.Length; i++)
                {
                    PlayerLoopSystem subSystem = subSystems[i];

                    if (subSystem.type != null && subSystem.type == type)
                    {
                        if (pathIndex == path.Count - 2)
                        {
                            return InternalTryChangeUpdateFunction(subSystem, path, updateFunction, pathIndex + 1, isAdding);
                        }

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
                }
            }

            return false;
        }

        private static bool InternalTryChangeSubSystem(PlayerLoopSystem playerLoop, IReadOnlyList<Type> path, Type subSystemType, int index, int pathIndex, bool isAdding)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (path.Count == 0) throw new InvalidOperationException("The specified path is empty.");
            if (isAdding && subSystemType == null) throw new ArgumentNullException(nameof(subSystemType));
            if (pathIndex < 0) throw new ArgumentException("The specified path index less than zero.");
            if (pathIndex >= path.Count) throw new ArgumentException("The specified path index more or equal than path count.");

            PlayerLoopSystem[] subSystems = playerLoop.subSystemList;

            if (subSystems != null)
            {
                Type type = path[pathIndex];

                for (int i = 0; i < subSystems.Length; i++)
                {
                    PlayerLoopSystem subSystem = subSystems[i];

                    if (subSystem.type != null && subSystem.type == type)
                    {
                        if (pathIndex == path.Count - 2)
                        {
                            return InternalTryChangeSubSystem(subSystem, path, subSystemType, index, pathIndex + 1, isAdding);
                        }

                        if (isAdding)
                        {
                            AddSubSystem(ref subSystem, subSystemType, index);
                        }
                        else
                        {
                            RemoveSubSystem(ref subSystem, index);
                        }

                        subSystems[i] = subSystem;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
