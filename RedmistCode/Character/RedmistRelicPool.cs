using BaseLib.Abstracts;
using Redmist.RedmistCode.Extensions;
using Godot;

namespace Redmist.RedmistCode.Character;

public class RedmistRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => Redmist.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}