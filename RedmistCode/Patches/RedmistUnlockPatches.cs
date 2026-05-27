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
    [HarmonyPatch(typeof(AchievementsHelper), nameof(AchievementsHelper.AfterRunEnded))]
    [HarmonyPostfix]
    public static void AfterRunEnded(RunState state, Player player, bool isVictory)
    {
        if (RedmistConfig.IsRedmistUnlocked) return;

        RedmistConfig.IsRedmistUnlocked = true;
        RedmistConfig.Instance?.Save();
    }

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