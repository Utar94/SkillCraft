namespace SkillCraft.Core.Attributes
{
  internal class RollAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute
  {
    public const string RollPattern = "\\d{1,2}d\\d{1,2}(\\+\\d{1,3})?";

    public RollAttribute() : base(RollPattern)
    {
    }
  }
}
