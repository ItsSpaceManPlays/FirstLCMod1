using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstLCMod.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatch
    {
        static ManualLogSource mls = FirstMod.mls;

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        static void roundManagerPatch(RoundManager __instance)
        {
            // update the current round in FirstMod.cs
            FirstMod.currentRound = __instance;
        }
    }
}
