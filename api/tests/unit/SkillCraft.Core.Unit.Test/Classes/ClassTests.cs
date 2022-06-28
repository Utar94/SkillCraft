using SkillCraft.Core.Fakers;
using SkillCraft.Core.Talents;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Classes
{
  [Trait(Traits.Category, Categories.Unit)]
  public class ClassTests
  {
    private static readonly WorldFaker _worldFaker = new();

    private readonly World _world = _worldFaker.Generate();

    private Guid UserId => _world.CreatedById;

    [Fact]
    public void Given_CorrectMandatoryTalents_When_Validate_Then_Succeeded()
    {
      var @class = new Class(2, UserId, _world);

      @class.Talents.Add(new ClassTalent(@class, new Talent(0, UserId, _world))
      {
        Mandatory = true
      });
      @class.Talents.Add(new ClassTalent(@class, new Talent(0, UserId, _world))
      {
        Mandatory = true
      });

      @class.Talents.Add(new ClassTalent(@class, new Talent(0, UserId, _world)));
      @class.Talents.Add(new ClassTalent(@class, new Talent(0, UserId, _world)));
      @class.Talents.Add(new ClassTalent(@class, new Talent(0, UserId, _world)));
      @class.Talents.Add(new ClassTalent(@class, new Talent(0, UserId, _world)));
      @class.Talents.Add(new ClassTalent(@class, new Talent(0, UserId, _world)));
      @class.Talents.Add(new ClassTalent(@class, new Talent(0, UserId, _world)));
      @class.Talents.Add(new ClassTalent(@class, new Talent(0, UserId, _world)));
      @class.Talents.Add(new ClassTalent(@class, new Talent(0, UserId, _world)));
      @class.Talents.Add(new ClassTalent(@class, new Talent(0, UserId, _world)));
      @class.Talents.Add(new ClassTalent(@class, new Talent(0, UserId, _world)));
    }

    [Fact]
    public void Given_MandatoryTalentsExceeded_When_Validate_Then_MandatoryTalentsExceededException()
    {
      var @class = new Class(0, UserId, _world);

      @class.Talents.Add(new ClassTalent(@class, new Talent(0, UserId, _world))
      {
        Mandatory = true
      });
      @class.Talents.Add(new ClassTalent(@class, new Talent(0, UserId, _world))
      {
        Mandatory = true
      });
      @class.Talents.Add(new ClassTalent(@class, new Talent(0, UserId, _world))
      {
        Mandatory = true
      });
      @class.Talents.Add(new ClassTalent(@class, new Talent(0, UserId, _world))
      {
        Mandatory = true
      });
      @class.Talents.Add(new ClassTalent(@class, new Talent(0, UserId, _world))
      {
        Mandatory = true
      });
      @class.Talents.Add(new ClassTalent(@class, new Talent(0, UserId, _world))
      {
        Mandatory = true
      });

      var exception = Assert.Throws<MandatoryTalentsExceededException>(() => @class.Validate());
      Assert.Same(@class, exception.Class);
    }
  }
}
