using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Redmist.RedmistCode.Cards;
using Redmist.RedmistCode.Character;
using Redmist.RedmistCode.Powers;

namespace Redmist.RedmistCode.Cards;

[Pool(typeof(RedmistCardPool))]
public class FearOfWater() : RedmistCard(2,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<FearOfWaterPower>(1),
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<FearOfWaterPower>(
            Owner.Creature, 
            DynamicVars[nameof(FearOfWaterPower)].BaseValue,
            Owner.Creature, 
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(FearOfWaterPower)].UpgradeValueBy(1);
    }
}
