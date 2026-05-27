using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Redmist.RedmistCode.Powers;

public class PartialEgoPower: CustomPowerModel
{
    
    private int _statsApplied;
    
    private int statsApplied
    {
        get => _statsApplied;
        set
        {
            AssertMutable();
            _statsApplied = value;
        }
    }
    
    private int _damageDeltLastRound;

    private int damageDeltLastRound
    {
        get => _damageDeltLastRound;
        set
        {
            AssertMutable();
            _damageDeltLastRound = value;
        }
    }
    
    private bool _wasApplied;

    private bool wasApplied
    {
        get => _wasApplied;
        set
        {
            AssertMutable();
            _wasApplied = value;
        }
    }


    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterApplied(Creature applier, CardModel cardSource)
    {
        await PowerCmd.Apply<StrengthPower>(
            Owner,
            2,
            Owner,
            null
        );
        
        await PowerCmd.Apply<DexterityPower>(
            Owner,
            2,
            Owner,
            null
        );

        wasApplied = true;

        statsApplied += 2;

    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != Owner)
            return;

        if (damageDeltLastRound < 10 && !wasApplied)
        {
            await PowerCmd.Remove(this);
        }
        else
        {
            damageDeltLastRound = 0;
        }

        if (wasApplied)
        {
            wasApplied = false;
        }
    }

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer,
        DamageResult result, ValueProp props,
        Creature target, CardModel? cardSource)
    {
        if (dealer != Owner || !props.IsPoweredAttack())
            return;


        damageDeltLastRound += result.UnblockedDamage;
        

        if (result.WasTargetKilled)
        {
            Flash();
            
            await PowerCmd.Apply<StrengthPower>(
                Owner,
                1,
                Owner,
                null
            );
        
            Flash();
            
            await PowerCmd.Apply<DexterityPower>(
                Owner,
                1,
                Owner,
                null
            );

            statsApplied += 1;
        }
    }


    public override async Task AfterRemoved(Creature oldOwner)
    {
        await PowerCmd.Apply<StrengthPower>(
            Owner,
            -statsApplied,
            Owner,
            null
        );
        
        await PowerCmd.Apply<DexterityPower>(
            Owner,
            -statsApplied,
            Owner,
            null
        );
    }
}