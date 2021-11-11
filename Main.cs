using MelonLoader;
using DiscordRichPresense;
using HarmonyLib;
namespace bvrRPC
{
    public class bvrRPC : MelonMod
    {
        private const string DiscordAppID = "908155565690216468";
        public static readonly DiscordRpc.RichPresence Presence = new DiscordRpc.RichPresence();
        public static class BuildInfo
        {
            public const string Name = "BricksVR Discord RPC"; // Name of the Mod.  (MUST BE SET)
            public const string Description = "DiscordRPC client for BricksVR"; // Description for the Mod.  (Set as null if none)
            public const string Author = "BelugaTheAxolotl#2134"; // Author of the Mod.  (Set as null if none)
            public const string Company = null; // Company that made the Mod.  (Set as null if none)
            public const string Version = "1.0.1"; // Version of the Mod.  (MUST BE SET)
            public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
        }
        public override void OnApplicationStart()
        {
            var i = new Harmony.HarmonyInstance("bvr-rpc");
            Hooks.ApplyHooks(i);
            var handlers = new DiscordRpc.EventHandlers();
            DiscordRpc.Initialize(DiscordAppID, ref handlers, false, string.Empty);
            Presence.state = string.Empty;
            Presence.details = "Playing BricksVR";
            Presence.startTimestamp = default(long);
            Presence.largeImageKey = "bvrlogo";
            Presence.largeImageText = "BricksVR";
            DiscordRpc.UpdatePresence(Presence);
        }
        public override void OnUpdate()
        {
            DiscordRpc.RunCallbacks();
        }

        public override void OnApplicationQuit()
        {
            DiscordRpc.Shutdown();
        }

    }
}
