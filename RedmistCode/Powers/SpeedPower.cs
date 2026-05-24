using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Redmist.RedmistCode.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Rooms;

namespace Redmist.RedmistCode.Powers;

public class SpeedPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    private readonly Dictionary<CardModel, int> _buffedCardsRegistry = new Dictionary<CardModel, int>();
    
    public override async Task AfterApplied(Creature applier, CardModel cardSource)
    {
        if (Owner.Player == null) return;
        
        UpdateAllCombatCards();
    }

    public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (Owner.Player == null) return;
        
        UpdateAllCombatCards();
    }

    public override async Task AfterCardEnteredCombat(CardModel card)
    {
        ApplyRepeatIfNeeded(card);
        await Task.CompletedTask;
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        ApplyRepeatIfNeeded(card);
        await Task.CompletedTask;
    }

    public override async Task AfterRemoved(Creature oldOwner)
    {
        ResetAllBuffedCards();
        await Task.CompletedTask;
    }

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        _buffedCardsRegistry.Clear();
    }

    private void UpdateAllCombatCards()
    {
        if (Owner.Player?.PlayerCombatState == null) return;

        foreach (CardModel card in Owner.Player.PlayerCombatState.AllCards)
        {
            ApplyRepeatIfNeeded(card);
        }
    }

    private void ApplyRepeatIfNeeded(CardModel cardModel)
    {
        if (!cardModel.Keywords.Contains((CardKeyword)KeywordExtensions.AGILE)) return;

        _buffedCardsRegistry.TryGetValue(cardModel, out int currentBonusGiven);

        int targetBonus = Amount;

        if (currentBonusGiven != targetBonus)
        {
            int difference = targetBonus - currentBonusGiven;
            
            cardModel.BaseReplayCount += difference;
            _buffedCardsRegistry[cardModel] = targetBonus;
        }
    }

    private void ResetAllBuffedCards()
    {
        foreach (var kvp in _buffedCardsRegistry)
        {
            CardModel card = kvp.Key;
            int bonusGiven = kvp.Value;

            card.BaseReplayCount -= bonusGiven;
        }
        
        _buffedCardsRegistry.Clear();
    }
}