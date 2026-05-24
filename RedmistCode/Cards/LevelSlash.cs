using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Redmist.RedmistCode.Cards;
using Redmist.RedmistCode.Powers;

namespace Redmist.RedmistCode.Cards;

public class LevelSlash() : RedmistCard(2,
    CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6m, ValueProp.Move),
        new RepeatVar(2),
        new PowerVar<BleedPower>(4),
        
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
            
            await PowerCmd.Apply<BleedPower>(
                play.Target,
                DynamicVars[nameof(BleedPower)].BaseValue,
                Owner.Creature,
                this
            );
            
            IEnumerable<LevelSlash> levelSlashes = this.Owner.PlayerCombatState.AllCards.OfType<LevelSlash>();
            foreach (LevelSlash levelSlash in levelSlashes)
            {
                ReduceLevelSlashCost(levelSlash);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }

    private static void ReduceLevelSlashCost(LevelSlash levelSlash)
    {
        levelSlash.EnergyCost.AddThisCombat(-1);
    }
}