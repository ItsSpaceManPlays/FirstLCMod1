using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstLCMod.Patches
{
    [HarmonyPatch(typeof(DoorLock))]
    internal class DoorLockPatch
    {
        static ManualLogSource mls = BepInEx.Logging.Logger.CreateLogSource("iTappedSpace.FirstMod");

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        static void doorReLock()
        {

        }
    }
}
