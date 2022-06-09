using Logitar.WebApiToolKit.Core.Exceptions;

namespace SkillCraft.Core.Worlds
{
  internal class AliasAlreadyUsedException : ConflictException
  {
    public AliasAlreadyUsedException(string alias, string paramName)
      : base(paramName, $"The alias \"{alias}\" is already used.")
    {
      Alias = alias ?? throw new ArgumentNullException(nameof(alias));
    }

    public string Alias { get; }
  }
}
