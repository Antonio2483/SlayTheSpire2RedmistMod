using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Redmist.RedmistCode.Powers;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Threading.Tasks;

public sealed class LoseStrengthEndOfTurnPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (Owner == null)
            return;

        Flash();
        
        await PowerCmd.Apply<StrengthPower>(Owner, -Amount, Owner, null);
        await PowerCmd.Remove(this);
    }
}