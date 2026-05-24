using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Redmist.RedmistCode.Cards;
using Redmist.RedmistCode.Extensions;

namespace Redmist.RedmistCode.Cards;

public class DebugDraw() : RedmistCard(0,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Innate,
        CardKeyword.Retain,
        (CardKeyword) KeywordExtensions.AGILE,
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CardPileCmd.Draw(choiceContext, 10, Owner);
        await Cmd.Wait(0.25f);
    }
    
    protected override PileType GetResultPileType()
    {
        PileType resultPileType = base.GetResultPileType();
        return resultPileType != PileType.Discard ? resultPileType : PileType.Hand;
    }

    protected override void OnUpgrade()
    {

    }
}