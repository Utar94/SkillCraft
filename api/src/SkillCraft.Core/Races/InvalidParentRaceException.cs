using Logitar.WebApiToolKit.Core.Exceptions;

namespace SkillCraft.Core.Races
{
  internal class InvalidParentRaceException : BadRequestException
  {
    public InvalidParentRaceException(Guid id)
      : base("InvalidParent", $"The race (Id={id}) cannot be a people.")
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
