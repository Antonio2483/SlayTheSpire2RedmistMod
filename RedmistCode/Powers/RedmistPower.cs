using BaseLib.Abstracts;
using BaseLib.Extensions;
using Redmist.RedmistCode.Extensions;
using Godot;

namespace Redmist.RedmistCode.Powers;

public abstract class RedmistPower : CustomPowerModel
{
    //Loads from Redmist/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}