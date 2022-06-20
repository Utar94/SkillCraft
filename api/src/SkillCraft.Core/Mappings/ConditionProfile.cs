using AutoMapper;
using SkillCraft.Core.Conditions;
using SkillCraft.Core.Conditions.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Mappings
{
  internal class ConditionProfile : Profile
  {
    public ConditionProfile()
    {
      CreateMap<Condition, ConditionModel>()
        .IncludeBase<EntityBase, EntityBaseModel>();
    }
  }
}
