using BaseLib.Abstracts;
using BaseLib.Utils;
using Redmist.RedmistCode.Character;

namespace Redmist.RedmistCode.Potions;

[Pool(typeof(RedmistPotionPool))]
public abstract class RedmistPotion : CustomPotionModel;