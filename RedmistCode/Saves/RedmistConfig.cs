using BaseLib.Config;
using Godot;

namespace Redmist.RedmistCode.Saves;

public class RedmistConfig : ModConfig
{
    public static bool IsRedmistUnlocked { get; set; } = false;

    // Guardamos a instância aqui
    public static RedmistConfig Instance { get; private set; }
    
    

    // Chamamos isso uma vez no começo do jogo
    public static void Setup()
    {
        Instance = new RedmistConfig();
    }

    public override void SetupConfigUI(Control optionContainer)
    {
        // Deixamos vazio para não aparecer no menu
    }
}