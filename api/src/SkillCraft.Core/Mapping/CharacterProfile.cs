using AutoMapper;
using SkillCraft.Core.Characters;
using SkillCraft.Core.Characters.Models;

namespace SkillCraft.Core.Mapping
{
  internal class CharacterProfile : Profile
  {
    public CharacterProfile()
    {
      CreateMap<Character, CharacterModel>().MapAggregate();
    }
  }
}
