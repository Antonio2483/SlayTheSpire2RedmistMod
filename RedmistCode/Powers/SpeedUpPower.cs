using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Redmist.RedmistCode.Cards;

namespace Redmist.RedmistCode.Powers;

public class SpeedUpPower : TemporarySpeedPower
{
    public override AbstractModel OriginModel => (AbstractModel) ModelDb.Card<SpeedUp>();

    protected override bool IsPositive => true;
}