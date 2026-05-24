using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Redmist.RedmistCode.Cards;
using Redmist.RedmistCode.Powers;

namespace Redmist.RedmistCode.Cards;

public class TestBleed() : RedmistCard(0,
    CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        
        await PowerCmd.Apply<BleedPower>(
            play.Target,
            10,
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {

    }
}