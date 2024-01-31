using BepInEx;
using BepInEx.Logging;
using FirstLCMod.Patches;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        internal static RoundManager currentRound;

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
        }
    }
}
