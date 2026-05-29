using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using Redmist.RedmistCode.Cards;

namespace Redmist.RedmistCode.Powers;

public class VengeanceStrengthPower: TemporaryStrengthPower
{
    public override AbstractModel OriginModel => (AbstractModel) ModelDb.Card<Vengeance>();

    protected override bool IsPositive => true;
}