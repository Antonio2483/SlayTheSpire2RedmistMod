using HarmonyLib;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Unlocks;
using MegaCrit.Sts2.Core.Localization;
using BaseLib.Config;
using System.Collections.Generic;
using System.Linq;
using Redmist.RedmistCode.Saves;
using Redmist.RedmistCode.Character;

namespace Redmist.RedmistCode.Patches;

[HarmonyPatch]
public static class RedmistUnlockPatches
{
    // 1. O desbloqueio real: Gastar 1000 de Ouro em lojas
    [HarmonyPatch(typeof(AchievementsHelper), nameof(AchievementsHelper.AfterRunEnded))]
    [HarmonyPostfix]
    public static void AfterRunEnded(RunState state, Player player, bool isVictory)
    {
        if (RedmistConfig.IsRedmistUnlocked) return;

        /*int goldSpent = 0;
        foreach (var actHistory in state.MapPointHistory)
        {
            foreach (var pointHistory in actHistory)
            {
                foreach (var room in pointHistory.Rooms)
                {
                    if (room.RoomType == MegaCrit.Sts2.Core.Rooms.RoomType.Shop)
                    {
                        foreach (var playerStat in pointHistory.PlayerStats)
                        {
                            if (playerStat.PlayerId == player.NetId)
                            {
                                goldSpent += playerStat.GoldSpent;
                            }
                        }
                    }
                }
            }
        }

        if (goldSpent >= 1000)
        {
            RedmistConfig.IsRedmistUnlocked = true;
            ModConfig.SaveDebounced<RedmistConfig>(0);
            MainFile.Logger.Info("[Redmist Mod] Jogador gastou 1000 de ouro. Personagem desbloqueada com sucesso!");
        }*/
        
        RedmistConfig.IsRedmistUnlocked = true;
        ModConfig.SaveDebounced<RedmistConfig>(0);
    }

    // 2. O Texto da Dica
    [HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.GetUnlockText))]
    [HarmonyPostfix]
    public static void GetUnlockText(CharacterModel __instance, ref LocString __result)
    {
        if (__instance is Character.Redmist)
        {
            __result = new LocString("characters", "REDMIST-REDMIST.unlockText"); 
        }
    }

    // 3. O Bloqueio Silencioso
    [HarmonyPatch(typeof(UnlockState), "get_Characters")]
    [HarmonyPostfix]
    public static void UnlockState_Characters(ref IEnumerable<CharacterModel> __result)
    {
        if (!RedmistConfig.IsRedmistUnlocked)
        {
            __result = __result.Where(c => !(c is Character.Redmist)).ToList();
        }
    }
}