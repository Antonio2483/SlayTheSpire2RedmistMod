using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Redmist.RedmistCode.Powers;

public sealed class BleedPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props,
        Creature target, CardModel? cardSource)
    {
        if (dealer != Owner || !props.IsPoweredAttack())
            return;
        
        Flash();
        
        await CreatureCmd.Damage(
            choiceContext, 
            Owner, 
            Amount, 
            ValueProp.Unpowered | ValueProp.SkipHurtAnim, 
            Owner);
        
        var reduction = Amount / 2;
        if (reduction > 0)
        {
           await PowerCmd.Apply<BleedPower>(Owner, -reduction, Owner, null);
        }
        else 
        {
            await PowerCmd.Remove(this);
        }
    }
}