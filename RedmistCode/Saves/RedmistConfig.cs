using BaseLib.Config;
using Godot;

namespace Redmist.RedmistCode.Saves;

public class RedmistConfig : ModConfig
{
    public static bool IsRedmistUnlocked { get; set; } = false;

    [ConfigIgnore]
    public static RedmistConfig Instance { get; private set; }

    public static void Setup()
    {
        Instance = new RedmistConfig();
    }

    public override void SetupConfigUI(Control optionContainer)
    {
    }
}