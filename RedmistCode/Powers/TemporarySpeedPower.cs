using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Redmist.RedmistCode.Powers;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public abstract class TemporarySpeedPower : CustomPowerModel, ITemporaryPower
{
    private bool _shouldIgnoreNextInstance;

    public override PowerType Type => !IsPositive ? PowerType.Debuff : PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public abstract AbstractModel OriginModel { get; }

    public PowerModel InternallyAppliedPower => (PowerModel)ModelDb.Power<SpeedPower>();

    protected virtual bool IsPositive => true;

    private int Sign => !IsPositive ? -1 : 1;

    public override LocString Title
    {
        get
        {
            return OriginModel switch
            {
                CardModel card     => card.TitleLocString,
                PotionModel potion => potion.Title,
                RelicModel relic   => relic.Title,
                _                  => throw new InvalidOperationException($"Origem desconhecida para TemporarySpeedPower: {OriginModel?.GetType()}")
            };
        }
    }

    public override LocString Description 
        => new(locTable, IsPositive ? "REDMIST-TEMPORARY_SPEED_POWER.description" : "REDMIST-TEMPORARY_SPEED_DOWN.description");

    protected override string SmartDescriptionLocKey 
        => !IsPositive ? "REDMIST-TEMPORARY_SPEED_DOWN.smartDescription" : "REDMIST-TEMPORARY_SPEED_POWER.smartDescription";

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            var items = new List<IHoverTip>();

            switch (OriginModel)
            {
                case CardModel card:
                    items.Add(HoverTipFactory.FromCard(card));
                    break;
                case PotionModel potion:
                    items.Add(HoverTipFactory.FromPotion(potion));
                    break;
                case RelicModel relic:
                    items.AddRange(HoverTipFactory.FromRelic(relic));
                    break;
                default:
                    throw new InvalidOperationException($"Modelo de origem inválido para dicas flutuantes: {OriginModel?.GetType()}");
            }

            items.Add(HoverTipFactory.FromPower<SpeedPower>());
            return items.AsReadOnly();
        }
    }

    public void IgnoreNextInstance() => _shouldIgnoreNextInstance = true;

    public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (_shouldIgnoreNextInstance)
        {
            _shouldIgnoreNextInstance = false;
        }
        else
        {
            await PowerCmd.Apply<SpeedPower>(target, Sign * amount, applier, cardSource, true);
        }
    }

    public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (amount == Amount || power != this)
            return;

        if (_shouldIgnoreNextInstance)
        {
            _shouldIgnoreNextInstance = false;
        }
        else
        {
            await PowerCmd.Apply<SpeedPower>(Owner, Sign * amount, applier, cardSource, true);
        }
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side)
            return;

        Flash();
        await PowerCmd.Remove(this);
        await PowerCmd.Apply<SpeedPower>(Owner, -Sign * Amount, Owner, null);
    }
}