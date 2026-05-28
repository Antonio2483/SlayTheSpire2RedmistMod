using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Redmist.RedmistCode.Cards;

namespace Redmist.RedmistCode.Powers;

public class HollowPointShellPower : TemporaryStrengthPower
{
    public override AbstractModel OriginModel => (AbstractModel) ModelDb.Card<HollowPointShell>();

    protected override bool IsPositive => false;
}
