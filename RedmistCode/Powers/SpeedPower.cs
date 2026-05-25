using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using Redmist.RedmistCode.Extensions;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Redmist.RedmistCode.Powers;

public class SpeedPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (card.Owner.Creature != this.Owner)
        {
            return playCount;
        }

        if (card.Keywords.Contains((CardKeyword)KeywordExtensions.AGILE))
        {
            return playCount + this.Amount; 
        }

        return playCount;
    }

    public override Task AfterModifyingCardPlayCount(CardModel card)
    {
        this.Flash();
        return Task.CompletedTask;
    }
}