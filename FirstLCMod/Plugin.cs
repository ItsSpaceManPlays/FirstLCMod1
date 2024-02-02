using BepInEx;
using BepInEx.Logging;
using FirstLCMod.Patches;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstLCMod
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class FirstMod : BaseUnityPlugin
    {
        private const string modGUID = "iTappedSpace.FirstMod";
        private const string modName = "FirstLCMod";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static FirstMod Instance;

        internal static PlayerControllerB currentPlayer;

        internal static RoundManager currentRound;

        internal static bool isHost;
        internal static bool isServer;
        internal static bool commandsEnabled = true;

        internal static ManualLogSource mls { get; set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            mls.LogInfo("The test mod has woken from its slumber");

            harmony.PatchAll(typeof(FirstMod));
            harmony.PatchAll(typeof(PlayerControllerBPatch));
            harmony.PatchAll(typeof(GrabbableObjectPatch));
            harmony.PatchAll(typeof(RoundManagerPatch));
            harmony.PatchAll(typeof(HUDManagerPatch));
        }

        static public Item getItem(string itemName)
        {
            if (currentRound != null)
            {
                for (int i = 0; i < currentRound.currentLevel.spawnableScrap.Count; i++)
                {
                    try
                    {
                        Item scrap = currentRound.currentLevel.spawnableScrap[i].spawnableItem;
                        if (scrap.itemName.ToLower() == itemName.ToLower())
                        {
                            return scrap;
                        }
                    } 
                    catch (Exception e)
                    {
                        mls.LogError(e.Message);
                    }
                }
            }
            return null;
        }

        static public void spawnItem(Item item, Vector3 spawnPosition)
        {
            GameObject newItem = UnityEngine.Object.Instantiate(item.spawnPrefab, spawnPosition, Quaternion.identity, currentRound.spawnedScrapContainer);
            GrabbableObject component = newItem.GetComponent<GrabbableObject>();
            component.startFallingPosition = spawnPosition + new Vector3(0, 2, 0);
            component.targetFloorPosition = component.GetItemFloorPosition(spawnPosition);

            System.Random random = new System.Random((int)component.targetFloorPosition.x + (int)component.targetFloorPosition.y);

            component.SetScrapValue((int)((float)random.Next(item.minValue + 25, item.maxValue + 35) * RoundManager.Instance.scrapValueMultiplier));
            component.NetworkObject.Spawn();
        }
    }
}
