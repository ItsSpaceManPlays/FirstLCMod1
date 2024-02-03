using GameNetcodeStuff;
using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstLCMod.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class HUDManagerPatch
    {
        [HarmonyPatch("AddTextToChatOnServer")]
        [HarmonyPrefix]
        private static void ReadChatMessage(HUDManager __instance, ref string chatMessage, ref int playerId)
        {
            if (playerId != -1)
            {
                PlayerControllerB speakingPlayer = FirstMod.currentRound.playersManager.allPlayerScripts[playerId];

                string nameOfUserWhoTyped = speakingPlayer.playerUsername;

                FirstMod.mls.LogInfo("Chat message: " + chatMessage + " sent by: " + nameOfUserWhoTyped);

                FirstMod.mls.LogInfo(FirstMod.isHost);

                if (chatMessage.StartsWith("/") && FirstMod.isHost && FirstMod.isServer && FirstMod.commandsEnabled)
                {
                    if (chatMessage.StartsWith("/scrap"))
                    {
                        // get scrap name

                        string[] splitCommand = chatMessage.Substring(1).Split(' ');

                        string scrapName = splitCommand[1];
                        if (splitCommand.Length > 2)
                        {
                            scrapName = string.Join(" ", splitCommand.Skip(1).ToArray());
                        }

                        // say all of the scrap types and spawn an item

                        FirstMod.mls.LogFatal(splitCommand.Join() + " : \"" + scrapName + "\"");

                        if (scrapName != null && scrapName != "")
                        {
                            //FirstMod.mls.LogInfo("Logging current item list");
                            //for (int i = 0; i < FirstMod.currentRound.currentLevel.spawnableScrap.Count(); i++)
                            //{
                            //    Item scrap = FirstMod.currentRound.currentLevel.spawnableScrap[i].spawnableItem;

                            //    FirstMod.mls.LogInfo("Scrap name: " + scrap.itemName + " with id: " + scrap.itemId);
                            //}

                            Vector3 spawnPos = FirstMod.currentPlayer.gameObject.transform.position + new Vector3(0, 3, 0);

                            Item newScrap = FirstMod.getItem(scrapName);
                            if (newScrap != null)
                            {
                                FirstMod.mls.LogInfo("Scrap spawned at " + spawnPos.x + " " + spawnPos.y + " " + spawnPos.z + " with item name: " + newScrap.itemName);
                                FirstMod.spawnItem(newScrap, spawnPos);
                            }
                        }
                    }
                }
            }
        }
    }
}
