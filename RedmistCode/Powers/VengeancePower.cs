using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Redmist.RedmistCode.Cards;

namespace Redmist.RedmistCode.Powers;

public class VengeancePower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    private int _damageTakenLastRound;

    private int damageTakenLastRound
    {
        get => _damageTakenLastRound;
        set
        {
            AssertMutable();
            _damageTakenLastRound = value;
        }
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != Owner || damageTakenLastRound == 0)
            return;
        
        var streanghToRecieve = damageTakenLastRound / 3;
        
        var finalStrength = streanghToRecieve > Amount? Amount : streanghToRecieve;

        if (finalStrength >= 1)
        {
            Flash();
            
            await PowerCmd.Apply<VengeanceStrengthPower>(
                Owner,
                finalStrength,
                Owner,
                ModelDb.Card<Vengeance>()
            );
            
        }

        damageTakenLastRound = 0;
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner)
            return;


        damageTakenLastRound += result.UnblockedDamage;
    }
}