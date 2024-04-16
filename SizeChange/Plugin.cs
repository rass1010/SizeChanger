using BepInEx;
using BepInEx.Configuration;
using System.IO;
using UnityEngine;
using Utilla;

namespace SizeChange
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")] // Make sure to add Utilla 1.5.0 as a dependency!
    [ModdedGamemode]
    public class Plugin : BaseUnityPlugin
    {
        bool inAllowedRoom = false;
        Vector3 ogSize;
        public static ConfigEntry<float> SizeMultiplier;

        void Awake()
        {
            var customFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "Scale.cfg"), true);
            SizeMultiplier = customFile.Bind("Configuration", "SizeMultiplier", 2f, "SizeMultiplier");
        }

        [ModdedGamemodeJoin]
        private void RoomJoined(string gamemode)
        {
            // The room is modded. Enable mod stuff.
            inAllowedRoom = true;
            ogSize = GorillaLocomotion.Player.Instance.bodyCollider.transform.root.localScale;
            
            GorillaLocomotion.Player.Instance.bodyCollider.transform.root.Find("RigCache").localScale = GorillaLocomotion.Player.Instance.bodyCollider.transform.root.localScale / Plugin.SizeMultiplier.Value;
            GorillaLocomotion.Player.Instance.bodyCollider.transform.root.localScale = GorillaLocomotion.Player.Instance.bodyCollider.transform.root.localScale * Plugin.SizeMultiplier.Value;

        }

        [ModdedGamemodeLeave]
        private void RoomLeft(string gamemode)
        {
            GorillaLocomotion.Player.Instance.bodyCollider.transform.root.Find("RigCache").localScale = ogSize;
            GorillaLocomotion.Player.Instance.bodyCollider.transform.root.localScale = ogSize;
            // The room was left. Disable mod stuff.
            inAllowedRoom = false;
        }
    }
}