using SkillCraft.Core.Logging;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core
{
  public interface IApplicationContext
  {
    EventLog EventLog { get; }
    World World { get; }

    void SetEntity(EntityBase entity);
    bool TryGetWorld(out World? world);
  }
}
