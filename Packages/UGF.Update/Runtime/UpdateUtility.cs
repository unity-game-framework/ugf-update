using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Experimental.LowLevel;

namespace UGF.Update.Runtime
{
    public static class UpdateUtility
    {
        public static bool TryAddUpdateFunction(PlayerLoopSystem playerLoopSystem, PlayerLoopSystem.UpdateFunction updateFunction, IEnumerable<Type> path)
        {
            if (updateFunction == null) throw new ArgumentNullException(nameof(updateFunction));
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (!path.Any()) throw new InvalidOperationException("The specified path is empty.");

            return false;
        }

        public static bool TrySetPlayerLoopSystem(PlayerLoopSystem playerLoopSystem, IEnumerable<Type> path, PlayerLoopSystem target)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (!path.Any()) throw new InvalidOperationException("The specified path is empty.");

            PlayerLoopSystem[] subSystemList = playerLoopSystem.subSystemList;

            if (subSystemList != null)
            {
                Type type = path.First();

                for (int i = 0; i < subSystemList.Length; i++)
                {
                    PlayerLoopSystem subSystem = subSystemList[i];

                    if (subSystem.type != null && subSystem.type == type)
                    {
                        if (path.Count() > 1)
                        {
                            return TrySetPlayerLoopSystem(subSystem, path.Skip(1), target);
                        }

                        subSystemList[i] = target;
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool TryGetPlayerLoopSystem(PlayerLoopSystem playerLoopSystem, IEnumerable<Type> path, out PlayerLoopSystem result)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (!path.Any()) throw new InvalidOperationException("The specified path is empty.");

            PlayerLoopSystem[] subSystemList = playerLoopSystem.subSystemList;

            if (subSystemList != null)
            {
                Type type = path.First();

                for (int i = 0; i < subSystemList.Length; i++)
                {
                    PlayerLoopSystem subSystem = subSystemList[i];

                    if (subSystem.type != null && subSystem.type == type)
                    {
                        if (path.Count() > 1)
                        {
                            return TryGetPlayerLoopSystem(subSystem, path.Skip(1), out result);
                        }

                        result = subSystem;
                        return true;
                    }
                }
            }

            result = default;
            return false;
        }

        public static string PrintPlayerLoop(PlayerLoopSystem playerLoopSystem, int depth = 0, string indent = "    ")
        {
            var builder = new StringBuilder();

            PrintPlayerLoop(builder, playerLoopSystem, depth, indent);

            return builder.ToString();
        }

        public static void PrintPlayerLoop(StringBuilder builder, PlayerLoopSystem playerLoopSystem, int depth = 0, string indent = "    ")
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            Type type = playerLoopSystem.type;

            string name = type != null
                ? !string.IsNullOrEmpty(type.Namespace) ? $"{type.Namespace}.{type.Name}" : type.Name
                : "Unknown";

            builder.Append(string.Concat(Enumerable.Repeat(indent, depth)));
            builder.Append(name);
            builder.AppendLine();

            if (playerLoopSystem.updateDelegate != null)
            {
                Delegate[] invocationList = playerLoopSystem.updateDelegate.GetInvocationList();

                if (invocationList.Length > 0)
                {
                    builder.Append(string.Concat(Enumerable.Repeat(indent, depth + 1)));
                    builder.Append("Invocations");
                    builder.AppendLine();

                    for (int i = 0; i < invocationList.Length; i++)
                    {
                        Delegate invocation = invocationList[i];

                        builder.Append(string.Concat(Enumerable.Repeat(indent, depth + 2)));
                        builder.Append($"T:[{invocation.Target}], M:[{invocation.Method}];");
                        builder.AppendLine();
                    }
                }
            }

            if (playerLoopSystem.subSystemList != null)
            {
                for (int i = 0; i < playerLoopSystem.subSystemList.Length; i++)
                {
                    PlayerLoopSystem subSystem = playerLoopSystem.subSystemList[i];

                    PrintPlayerLoop(builder, subSystem, depth + 1, indent);
                }
            }
        }
    }
}
