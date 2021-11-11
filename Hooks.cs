using Harmony;
using System.Reflection;
using System;
using DiscordRichPresense;

namespace bvrRPC
{
    internal static class Hooks
    {
        public static void ApplyHooks(HarmonyInstance instance)
        {
            instance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
