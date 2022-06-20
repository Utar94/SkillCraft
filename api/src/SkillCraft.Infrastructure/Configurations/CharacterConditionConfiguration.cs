using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Characters;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class CharacterConditionConfiguration : IEntityTypeConfiguration<CharacterCondition>
  {
    public void Configure(EntityTypeBuilder<CharacterCondition> builder)
    {
      builder.HasKey(x => new { x.CharacterId, x.ConditionId });
    }
  }
}
