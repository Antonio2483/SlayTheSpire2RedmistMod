using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Redmist.RedmistCode.Cards;
using Redmist.RedmistCode.Character;
using Redmist.RedmistCode.Powers;

namespace Redmist.RedmistCode.Cards;

[Pool(typeof(RedmistCardPool))]
public class Vengeance() : RedmistCard(2,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VengeancePower>(2),
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<VengeancePower>(
            Owner.Creature,
            DynamicVars[nameof(VengeancePower)].BaseValue,
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(VengeancePower)].UpgradeValueBy(1);
    }
}