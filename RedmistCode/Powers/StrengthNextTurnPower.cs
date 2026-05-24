using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace Redmist.RedmistCode.Powers;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Threading.Tasks;

#nullable enable
public sealed class StrengthNextTurnPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(CombatSide combatSide, CombatState combatState)
    {
        StrengthNextTurnPower power = this;
        if (power.Owner == null || combatSide != power.Owner.Side)
            return;

        power.Flash();
        await PowerCmd.Apply<StrengthPower>(
            power.Owner,
            power.Amount,
            power.Owner,
            null
        );
        
        await PowerCmd.Apply<LoseStrengthEndOfTurnPower>(Owner, Amount, Owner, null);
        
        await PowerCmd.Remove(this);
    }
}