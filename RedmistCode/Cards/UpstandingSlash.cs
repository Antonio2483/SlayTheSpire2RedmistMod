using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Redmist.RedmistCode.Cards;

namespace Redmist.RedmistCode.Cards;

public class UpstandingSlash() : RedmistCard(2,
    CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6m, ValueProp.Move),
        new RepeatVar(2),
        
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        
        ArgumentNullException.ThrowIfNull(play.Target);

        var attackCommand = await  DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        var totalUnblocked = attackCommand.Results.Sum(r => r.UnblockedDamage);

        if (totalUnblocked >= 10)
        {
            await PowerCmd.Apply<DrawCardsNextTurnPower>(
                Owner.Creature,
                1,
                Owner.Creature,
                this);
            
            IEnumerable<UpstandingSlash> upstandingSlashes = this.Owner.PlayerCombatState.AllCards.OfType<UpstandingSlash>();
            foreach (UpstandingSlash upstandingSlash in upstandingSlashes)
            {
                
                
                ReduceUpstandingSlashCost(upstandingSlash);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }

    private static void ReduceUpstandingSlashCost(UpstandingSlash upstandingSlash)
    {
        upstandingSlash.EnergyCost.AddThisCombat(-1);
    }
}