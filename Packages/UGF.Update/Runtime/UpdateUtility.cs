using System;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine.LowLevel;

namespace UGF.Update.Runtime
{
    /// <summary>
    /// Provides utilities to work with updates.
    /// </summary>
    public static class UpdateUtility
    {
        /// <summary>
        /// Resets player loop to default.
        /// </summary>
        public static void ResetPlayerLoopToDefault()
        {
            PlayerLoop.SetPlayerLoop(PlayerLoop.GetDefaultPlayerLoop());
        }

        /// <summary>
        /// Tries to add the specified update function into a found subsystem of the specified type from the player loop system.
        /// </summary>
        /// <param name="playerLoop">The player loop system to add into.</param>
        /// <param name="subSystemType">The type of the subsystem to find.</param>
        /// <param name="updateFunction">The update function to add.</param>
        public static bool TryAddUpdateFunction(PlayerLoopSystem playerLoop, Type subSystemType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            return InternalTryChangeUpdateFunction(playerLoop, subSystemType, updateFunction, true);
        }

        /// <summary>
        /// Tries to remove the specified update function from a found subsystem of the specified type from the player loop system.
        /// </summary>
        /// <param name="playerLoop">The player loop system to remove from.</param>
        /// <param name="subSystemType">The type of the subsystem to find.</param>
        /// <param name="updateFunction">The update function to remove.</param>
        public static bool TryRemoveUpdateFunction(PlayerLoopSystem playerLoop, Type subSystemType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            return InternalTryChangeUpdateFunction(playerLoop, subSystemType, updateFunction, false);
        }

        /// <summary>
        /// Determines whether the specified player loop system contains any subsystems with the specified type.
        /// </summary>
        /// <param name="playerLoop">The player loop system to check.</param>
        /// <param name="subSystemType">The type of the subsystem.</param>
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

        /// <summary>
        /// Tries to add a subsystem with the specified type, targeting to a subsystem with the specified type and use specified insertion mode.
        /// </summary>
        /// <param name="playerLoop">The player loop system to change.</param>
        /// <param name="targetSubSystemType">The type of the subsystem to find.</param>
        /// <param name="subSystemType">The type of the new subsystem.</param>
        /// <param name="insertion">The insertion mode used to create new subsystem relative to found target subsystem.</param>
        public static bool TryAddSubSystem(ref PlayerLoopSystem playerLoop, Type targetSubSystemType, Type subSystemType, UpdateSubSystemInsertion insertion)
        {
            if (targetSubSystemType == null) throw new ArgumentNullException(nameof(targetSubSystemType));
            if (subSystemType == null) throw new ArgumentNullException(nameof(subSystemType));

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
                                AddSubSystem(ref playerLoop, subSystemType, IntPtr.Zero, i);
                                break;
                            }
                            case UpdateSubSystemInsertion.After:
                            {
                                AddSubSystem(ref playerLoop, subSystemType, IntPtr.Zero, i + 1);
                                break;
                            }
                            case UpdateSubSystemInsertion.InsideTop:
                            {
                                AddSubSystem(ref subSystem, subSystemType, IntPtr.Zero, 0);

                                subSystems[i] = subSystem;
                                break;
                            }
                            case UpdateSubSystemInsertion.InsideBottom:
                            {
                                int index = subSystem.subSystemList?.Length ?? 0;

                                AddSubSystem(ref subSystem, subSystemType, IntPtr.Zero, index);

                                subSystems[i] = subSystem;
                                break;
                            }
                            default: throw new ArgumentOutOfRangeException(nameof(insertion), insertion, "Unknown insertion.");
                        }

                        return true;
                    }

                    if (TryAddSubSystem(ref subSystem, targetSubSystemType, subSystemType, insertion))
                    {
                        subSystems[i] = subSystem;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Tries to remove a subsystem with the specified type from the specified player loop system.
        /// </summary>
        /// <param name="playerLoop">The player loop system to change.</param>
        /// <param name="subSystemType">The type of the subsystem to remove.</param>
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

        /// <summary>
        /// Adds subsystem with the specified type at the specified index into player loop system.
        /// </summary>
        /// <param name="playerLoop">The player loop system to change.</param>
        /// <param name="subSystemType">The type of the subsystem to add.</param>
        /// <param name="index">The index of the subsystem.</param>
        public static void AddSubSystem(ref PlayerLoopSystem playerLoop, Type subSystemType, int index)
        {
            AddSubSystem(ref playerLoop, subSystemType, IntPtr.Zero, index);
        }

        /// <summary>
        /// Adds subsystem with the specified type at the specified index into player loop system.
        /// </summary>
        /// <param name="playerLoop">The player loop system to change.</param>
        /// <param name="subSystemType">The type of the subsystem to add.</param>
        /// <param name="updateFunction">The native update function of the subsystem to use.</param>
        /// <param name="index">The index of the subsystem.</param>
        public static void AddSubSystem(ref PlayerLoopSystem playerLoop, Type subSystemType, IntPtr updateFunction, int index)
        {
            if (subSystemType == null) throw new ArgumentNullException(nameof(subSystemType));
            if (index < 0 || playerLoop.subSystemList != null && index > playerLoop.subSystemList.Length) throw new ArgumentOutOfRangeException(nameof(index));

            PlayerLoopSystem[] subSystems = playerLoop.subSystemList;

            var subSystem = new PlayerLoopSystem
            {
                type = subSystemType,
                loopConditionFunction = IntPtr.Zero,
                updateFunction = updateFunction
            };

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

        /// <summary>
        /// Removes subsystem a the specified index from the player loop system.
        /// </summary>
        /// <param name="playerLoop">The player loop system.</param>
        /// <param name="index">The index of the subsystem.</param>
        public static void RemoveSubSystem(ref PlayerLoopSystem playerLoop, int index)
        {
            if (playerLoop.subSystemList == null) throw new ArgumentException("The specified player loop does not contains any subsystems.");
            if (index < 0 || index > playerLoop.subSystemList.Length) throw new ArgumentOutOfRangeException(nameof(index));

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

        /// <summary>
        /// Prints full hierarchy of the specified player loop system as string representation.
        /// </summary>
        /// <param name="playerLoop">The player loop system to print.</param>
        /// <param name="depth">The initial indent depth.</param>
        /// <param name="indent">The indent value used for nested nodes.</param>
        public static string PrintPlayerLoop(PlayerLoopSystem playerLoop, int depth = 0, int indent = 4)
        {
            var builder = new StringBuilder();

            PrintPlayerLoop(builder, playerLoop, depth, indent);

            return builder.ToString();
        }

        /// <summary>
        /// Prints full hierarchy of the specified player loop system as string representation.
        /// </summary>
        /// <param name="builder">The builder used to construct string.</param>
        /// <param name="playerLoop">The player loop system to print.</param>
        /// <param name="depth">The initial indent depth.</param>
        /// <param name="indent">The indent value used for nested nodes.</param>
        public static void PrintPlayerLoop(StringBuilder builder, PlayerLoopSystem playerLoop, int depth = 0, int indent = 4)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (indent < 0) throw new ArgumentOutOfRangeException(nameof(indent));

            Type type = playerLoop.type;

            string name = type != null
                ? !string.IsNullOrEmpty(type.Namespace) ? $"{type.Namespace}.{type.Name}" : type.Name
                : "PlayerLoopSystem";

            builder.Append(' ', depth * indent);
            builder.Append(name);
            builder.Append($" (condition: {playerLoop.loopConditionFunction}, updateFunction: {playerLoop.updateFunction})");
            builder.AppendLine();

            if (playerLoop.updateDelegate != null)
            {
                Delegate[] invocations = playerLoop.updateDelegate.GetInvocationList();

                if (invocations.Length > 0)
                {
                    builder.Append(' ', (depth + 1) * indent);
                    builder.Append("Invocations");
                    builder.AppendLine();

                    for (int i = 0; i < invocations.Length; i++)
                    {
                        Delegate invocation = invocations[i];
                        MethodInfo method = invocation.Method;
                        object target = invocation.Target;

                        builder.Append(' ', (depth + 2) * indent);
                        builder.Append($"{method.DeclaringType}::{method}");

                        if (target != null)
                        {
                            builder.Append($" ({target})");
                            builder.AppendLine();

                            if (target is IUpdateGroup group)
                            {
                                PrintUpdateGroup(builder, group, depth + 3, indent);
                            }
                        }
                        else
                        {
                            builder.AppendLine();
                        }
                    }
                }
            }

            if (playerLoop.subSystemList != null)
            {
                PlayerLoopSystem[] subSystems = playerLoop.subSystemList;

                if (subSystems.Length > 0)
                {
                    builder.Append(' ', (depth + 1) * indent);
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

        /// <summary>
        /// Prints full hierarchy of subgroups for the specified update group, items of the update collection, add and remove queue of items.
        /// </summary>
        /// <param name="group">The update group to print.</param>
        /// <param name="depth">The initial indent depth.</param>
        /// <param name="indent">The indent value used for nested nodes.</param>
        public static string PrintUpdateGroup(IUpdateGroup group, int depth = 0, int indent = 4)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));

            var builder = new StringBuilder();

            PrintUpdateGroup(builder, group, depth, indent);

            return builder.ToString();
        }

        /// <summary>
        /// Prints full hierarchy of subgroups for the specified update group, items of the update collection, add and remove queue of items.
        /// </summary>
        /// <param name="builder">The builder used to construct string.</param>
        /// <param name="group">The update group to print.</param>
        /// <param name="depth">The initial indent depth.</param>
        /// <param name="indent">The indent value used for nested nodes.</param>
        public static void PrintUpdateGroup(StringBuilder builder, IUpdateGroup group, int depth = 0, int indent = 4)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (group == null) throw new ArgumentNullException(nameof(group));
            if (indent < 0) throw new ArgumentOutOfRangeException(nameof(indent));

            builder.Append(' ', depth * indent);
            builder.Append($"{group.GetType()} (enabled: {group.Enable})");
            builder.AppendLine();

            if (group.Collection.Count > 0)
            {
                builder.Append(' ', (depth + 1) * indent);
                builder.Append($"Collection (count: {group.Collection.Count})");
                builder.AppendLine();

                foreach (object element in group.Collection)
                {
                    builder.Append(' ', (depth + 2) * indent);
                    builder.Append($"{element}");
                    builder.AppendLine();
                }
            }

            if (group.Collection.Queue.Add.Cast<object>().Any())
            {
                builder.Append(' ', (depth + 1) * indent);
                builder.Append($"Queue Add (count: {group.Collection.Queue.Add.Cast<object>().Count()})");
                builder.AppendLine();

                foreach (object element in group.Collection.Queue.Add)
                {
                    builder.Append(' ', (depth + 2) * indent);
                    builder.Append($"{element}");
                    builder.AppendLine();
                }
            }

            if (group.Collection.Queue.Remove.Cast<object>().Any())
            {
                builder.Append(' ', (depth + 1) * indent);
                builder.Append($"Queue Remove (count: {group.Collection.Queue.Remove.Cast<object>().Count()})");
                builder.AppendLine();

                foreach (object element in group.Collection.Queue.Remove)
                {
                    builder.Append(' ', (depth + 2) * indent);
                    builder.Append($"{element}");
                    builder.AppendLine();
                }
            }

            if (group.SubGroups.Count > 0)
            {
                builder.Append(' ', (depth + 1) * indent);
                builder.Append("SubGroups");
                builder.AppendLine();

                foreach (IUpdateGroup subGroup in group.SubGroups)
                {
                    PrintUpdateGroup(builder, subGroup, depth + 2, indent);
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
