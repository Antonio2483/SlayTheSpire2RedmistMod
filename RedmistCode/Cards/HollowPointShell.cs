using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Redmist.RedmistCode.Cards;
using Redmist.RedmistCode.Character;
using Redmist.RedmistCode.Powers;

namespace Redmist.RedmistCode.Cards;

[Pool(typeof(RedmistCardPool))]
public class HollowPointShell() : RedmistCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6m, ValueProp.Move),
        new PowerVar<TemporaryStrengthPower>(1),
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);

        var attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        
        await PowerCmd.Apply<TemporaryStrengthPower>(
            play.Target,
            (DynamicVars[nameof(TemporaryStrengthPower)].BaseValue * -1),
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars["TemporaryStrengthPower"].UpgradeValueBy(1m);
    }
}