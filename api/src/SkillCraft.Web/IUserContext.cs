using SkillCraft.Core.Worlds;

namespace SkillCraft.Web
{
  public interface IUserContext : Logitar.Identity.Core.IUserContext
  {
    World World { get; }
  }
}
