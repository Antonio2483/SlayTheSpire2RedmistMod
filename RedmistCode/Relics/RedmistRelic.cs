using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Redmist.RedmistCode.Character;
using Redmist.RedmistCode.Extensions;
using Godot;

namespace Redmist.RedmistCode.Relics;

[Pool(typeof(RedmistRelicPool))]
public abstract class RedmistRelic : CustomRelicModel
{
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();

    protected override string PackedIconOutlinePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();

    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
}