using MelonLoader;
using DiscordRichPresense;
using UnityEngine;
using Normal.Realtime;
using System.Collections;
using System.IO;
using System.Linq;
using Harmony;
namespace bvrRPC
{
    public class bvrRPC : MelonMod
    {
        private const string DiscordAppID = "908155565690216468";
        public static readonly DiscordRpc.RichPresence Presence = new DiscordRpc.RichPresence();
        public const int PlacedBricks = 0;
        public static class BuildInfo
        {
            public const string Name = "BricksVR Discord RPC"; // Name of the Mod.  (MUST BE SET)
            public const string Description = "DiscordRPC client for BricksVR"; // Description for the Mod.  (Set as null if none)
            public const string Author = "BelugaTheAxolotl#2134"; // Author of the Mod.  (Set as null if none)
            public const string Company = null; // Company that made the Mod.  (Set as null if none)
            public const string Version = "0.0.2"; // Version of the Mod.  (MUST BE SET)
            public const string DownloadLink = "https://github.com/BricksVR-Modding/BVR-DiscordRPC/releases/"; // Download Link for the Mod.  (Set as null if none)
        }
        public IEnumerator richPresenceCheck()
        {
            while (true)
            {
                if(GameObject.Find("MetaObjects/NormalSessionManager").GetComponent<NormalSessionManager>().Connected())
                {
                    Presence.details = "Playing in room "+ GameObject.Find("MetaObjects/NormalSessionManager").GetComponent<NormalSessionManager>()._roomName;
                    Presence.startTimestamp = default(long);
                    Presence.largeImageKey = "bvrlogo";
                    Presence.largeImageText = "BricksVR";
                    if (GameObject.Find("MetaObjects/Realtime").GetComponent<RealtimeAvatarManager>().avatars.count == 1)
                    {
                        Presence.state = "Is alone.";
                    }
                    if((GameObject.Find("MetaObjects/Realtime").GetComponent<RealtimeAvatarManager>().avatars.count - 1) == 1)
                    {
                        Presence.state = "Is playing with " + (GameObject.Find("MetaObjects/Realtime").GetComponent<RealtimeAvatarManager>().avatars.count - 1) + " other.";
                    }
                    else if ((GameObject.Find("MetaObjects/Realtime").GetComponent<RealtimeAvatarManager>().avatars.count - 1) > 1)
                    {
                        Presence.state = "Is playing with " + (GameObject.Find("MetaObjects/Realtime").GetComponent<RealtimeAvatarManager>().avatars.count - 1) + " others.";
                    }
                    DiscordRpc.UpdatePresence(Presence);
                }
                else
                {
                    var handlers = new DiscordRpc.EventHandlers();
                    DiscordRpc.Initialize(DiscordAppID, ref handlers, false, string.Empty);
                    Presence.state = string.Empty;
                    Presence.details = "Playing BricksVR";
                    Presence.startTimestamp = default(long);
                    Presence.largeImageKey = "bvrlogo";
                    Presence.largeImageText = "BricksVR";
                    DiscordRpc.UpdatePresence(Presence);
                }
                yield return new WaitForSeconds(1f);
            }
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
        public override void OnLevelWasInitialized(int level)
        {
            MelonCoroutines.Start(richPresenceCheck());
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
    //[HarmonyPatch(typeof(PlacedBrickCreator), "CreateFromAttributes", MethodType.Normal)]
    //public static class Data_Patch
    //{
        //[HarmonyPostfix]
        //public static void Postfix(int matId, string type, Vector3 pos, Quaternion rot, string uuid, int color, bool usingNewColor, int headClientId, bool recalculateMesh = true)
        //{
            //if (GameObject.Find("MetaObjects/Realtime").GetComponent<Realtime>().clientID == -1)
            //{
                //MelonLogger.Msg("Brick Placed.");
                //var increment = 1;
                //var e = increment + bvrRPC.PlacedBricks;
                //bvrRPC.PlacedBricks = e;
            //}
        //}
    //}
}
