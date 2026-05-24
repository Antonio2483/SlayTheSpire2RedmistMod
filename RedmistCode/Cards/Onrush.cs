using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Redmist.RedmistCode.Cards;
using Redmist.RedmistCode.Character;

namespace Redmist.RedmistCode.Cards;

[Pool(typeof(RedmistCardPool))]
public class Onrush() : RedmistCard(2, CardType.Attack,
    CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(8m, ValueProp.Move)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        
        var attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        while (attackCommand.Results.Any(r => r.WasTargetKilled))
        {
            // Pega inimigos vivos usando a lista do CombatState
            var aliveEnemies = CombatState.HittableEnemies.ToList();
        
            if (aliveEnemies.Count == 0)
                break;

            var newTarget = aliveEnemies[Owner.RunState.Rng.CombatTargets.NextInt(aliveEnemies.Count)];

            attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(newTarget)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}