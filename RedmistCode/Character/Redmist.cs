using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Redmist.RedmistCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using Redmist.RedmistCode.Cards;
using Redmist.RedmistCode.Relics;
using Redmist.RedmistCode.Saves;

namespace Redmist.RedmistCode.Character;

public class Redmist : PlaceholderCharacterModel
{
    public const string CharacterId = "Redmist";
    
    // 1. Voltamos para ironclad pro jogo usar o contador de energia e transições dele sem crashar
    public override string PlaceholderID => "ironclad";

    public static readonly Color Color = new("ffffff");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 70;
    
    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeRedmist>(),
        ModelDb.Card<StrikeRedmist>(),
        ModelDb.Card<StrikeRedmist>(),
        ModelDb.Card<StrikeRedmist>(),
        ModelDb.Card<DefendRedmist>(),
        ModelDb.Card<DefendRedmist>(),
        ModelDb.Card<DefendRedmist>(),
        ModelDb.Card<DefendRedmist>(),
        ModelDb.Card<Penitence>(),
        ModelDb.Card<RedEyes>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<Kali>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<RedmistCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<RedmistRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<RedmistPotionPool>();

    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }
    
    protected override CharacterModel? UnlocksAfterRunAs 
    {
        get 
        {
            if (RedmistConfig.IsRedmistUnlocked) return null;
            return ModelDb.Character<Redmist>();
        }
    }
    
    // 2. Substituímos os caminhos de UI para carregar os seus arquivos da pasta charui/
    public override string CustomIconTexturePath => "character_icon_redmist.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_redmist.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_redmist_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "character_icon_redmist.png".CharacterUiPath();
    
    // 3. Apontamos o visual de combate para um arquivo .tscn específico que vamos criar
    public override string CustomVisualPath => "res://mods/Redmist/animations/redmist/redmist.skel";
}