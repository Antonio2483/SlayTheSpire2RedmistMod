using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using Redmist.RedmistCode.Extensions;
using Redmist.RedmistCode.Saves;

namespace Redmist.RedmistCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "Redmist"; 
    public const string ResPath = $"res://{ModId}";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        
        // Inicia nosso sistema de unlock/save pelo BaseLib
        RedmistConfig.Setup();

        // O Harmony acha nossa classe RedmistUnlockPatches sozinho!
        Harmony harmony = new(ModId);
        harmony.PatchAll();
        
        KeywordExtensions.registerKeywords();
    }
}