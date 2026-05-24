using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using Redmist.RedmistCode.Powers;
using Redmist.RedmistCode.Relics;

namespace Redmist.RedmistCode.Relics;

public class Kali() : RedmistRelic
{
    private bool _EGOApplied;
    
    private bool EGOApplied
    {
        get => _EGOApplied;
        set
        {
            AssertMutable();
            _EGOApplied = value;
        }
    }
    
    public override RelicRarity Rarity =>
        RelicRarity.Starter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("HpThreshold", 50M),
        new PowerVar<PartialEGOPower>(1),
    ];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner && Owner.Creature.CombatState.RoundNumber == 1)
        {
            Flash();
            
            await PlayerCmd.GainEnergy(1, Owner);
            await CardPileCmd.Draw(choiceContext, 1, Owner);
            
        }
        
    }
    
    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (!(room is CombatRoom))
            return;
        await ApplyEGOIfNecessary();
    }
    
    public override async Task AfterCurrentHpChanged(Creature creature, Decimal _)
    {
        if (!CombatManager.Instance.IsInProgress)
            return;
        await ApplyEGOIfNecessary();
    }
    
    public override Task AfterCombatEnd(CombatRoom _)
    {
        EGOApplied = false;
        return Task.CompletedTask;
    }

    private async Task ApplyEGOIfNecessary()
    {
        bool isHalfHP = (Decimal) Owner.Creature.CurrentHp < (Decimal) Owner.Creature.MaxHp * (DynamicVars["HpThreshold"].BaseValue / 100M);

        if (isHalfHP && !EGOApplied)
        {
            await PowerCmd.Apply<PartialEGOPower>(
                Owner.Creature,
                DynamicVars[nameof(PartialEGOPower)].BaseValue,
                Owner.Creature,
                null,
                false
            );
            
            EGOApplied = true;
        }
        else if (!isHalfHP && EGOApplied)
        {
            await PowerCmd.Apply<PartialEGOPower>(
                Owner.Creature,
                -DynamicVars[nameof(PartialEGOPower)].BaseValue,
                Owner.Creature,
                null,
                false
            );
        
            EGOApplied = false;
        }
        
    }

}