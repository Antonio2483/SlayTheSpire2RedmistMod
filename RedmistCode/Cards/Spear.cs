using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Redmist.RedmistCode.Cards;

namespace Redmist.RedmistCode.Cards;

public class Spear() : RedmistCard(2, CardType.Attack,
    CardRarity.Basic, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(4m, ValueProp.Move),
        new RepeatVar(3),
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
            
            IEnumerable<Spear> spears = this.Owner.PlayerCombatState.AllCards.OfType<Spear>();
            foreach (Spear spear in spears)
            {
                
                
                ReduceSpearCost(spear);
            }
        }
    }

    protected override void OnUpgrade()
    {

    }
    
    private static void ReduceSpearCost(Spear spear)
    {
        spear.EnergyCost.AddThisCombat(-1);
    }
}