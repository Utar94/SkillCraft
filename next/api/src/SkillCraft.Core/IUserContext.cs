using SkillCraft.Core.Worlds;

namespace SkillCraft.Core
{
  public interface IUserContext
  {
    Guid Id { get; }

    World World { get; }
  }
}
