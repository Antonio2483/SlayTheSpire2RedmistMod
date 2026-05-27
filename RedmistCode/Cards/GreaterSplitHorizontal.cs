using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Redmist.RedmistCode.Cards;
using Redmist.RedmistCode.Character;
using Redmist.RedmistCode.Powers;

namespace Redmist.RedmistCode.Cards;

[Pool(typeof(RedmistCardPool))]
public class GreaterSplitHorizontal() : RedmistCard(6,
    CardType.Attack, CardRarity.Rare,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(28m, ValueProp.Move),
        new PowerVar<BleedPower>(5),
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(CombatState)
            .WithHitFx("vfx/vfx_attack_blunt", null, "heavy_attack.mp3")
            .Execute(choiceContext);
        
        await PowerCmd.Apply<BleedPower>(
            CombatState.Enemies,
            DynamicVars[nameof(BleedPower)].BaseValue,
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(14m);
    }
}