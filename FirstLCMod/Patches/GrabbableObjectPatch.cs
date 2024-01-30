using HarmonyLib;
using UnityEngine;
using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Logging;

namespace FirstLCMod.Patches
{
    [HarmonyPatch(typeof(GrabbableObject))]
    internal class GrabbableObjectPatch
    {
        static ManualLogSource mls = FirstMod.mls;

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        static void lineOfSightPatch(GrabbableObject __instance)
        {
            mls.LogInfo("Attempting patch on object: " + __instance.gameObject.name);
            ScanNodeProperties scanProperties = __instance.gameObject.GetComponentInChildren<ScanNodeProperties>();

            if (scanProperties != null)
            {
                scanProperties.requiresLineOfSight = false;
            } 
            else
            {
                mls.LogWarning("Failed to find ScanNode for object.");
            }
        }
    }
}
