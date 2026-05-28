using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Redmist.RedmistCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
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

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
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
    
    // Dentro da sua classe Redmist : PlaceholderCharacterModel

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
    
}