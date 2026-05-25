using System.Collections;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using Redmist.RedmistCode.Cards;
using Redmist.RedmistCode.Powers;
using Redmist.RedmistCode.Relics;

namespace Redmist.RedmistCode.Relics;

public class Debug() : RedmistRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (!(room is CombatRoom))
            return;
        await PowerCmd.Apply<SpeedPower>(Owner.Creature, 1, Owner.Creature, null,false);
    }
    
    /*public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner || Owner.Creature.CombatState.RoundNumber !=1)
            return;
        
        Flash();
        List<CardModel> cartasAdd = new List<CardModel>();

        DebugDraw canonicalCard = ModelDb.Card<DebugDraw>();
        if (canonicalCard == null) return;
    
        CombatState combatState = player.Creature.CombatState;
        if (combatState == null) return;
        
        DebugDraw debugDraw = combatState.CreateCard<DebugDraw>(player);
        if (debugDraw == null) return;
        
        cartasAdd.Add(debugDraw);
        
        var card = await CardPileCmd.AddGeneratedCardsToCombat(
            cartasAdd,
            PileType.Hand,
            true);
        
        CardCmd.ApplyKeyword(debugDraw, CardKeyword.Retain);

    }*/

    public override async Task AfterSideTurnStart(CombatSide combatSide, CombatState combatState)
    {
        if (combatSide != Owner.Creature.Side)
            return;
        
        Flash();
        
        await PlayerCmd.GainEnergy(99, this.Owner);
    }
    
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Creature.Side)
            return;
        
        Flash();
        
        await CreatureCmd.Heal(Owner.Creature, 100);
    }
    
}