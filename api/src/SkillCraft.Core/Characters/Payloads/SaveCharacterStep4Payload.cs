using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Characters.Payloads
{
  public class SaveCharacterStep4Payload : IValidatableObject
  {
    public Guid CasteId { get; set; }

    public Guid EducationId { get; set; }

    public IEnumerable<CharacterTalentPayload>? Talents { get; set; }

    public IEnumerable<SkillRankPayload>? SkillRanks { get; set; }

    public string? Description { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var results = new List<ValidationResult>(capacity: 2);

      if (SkillRanks != null)
      {
        IEnumerable<Guid> skillRankIds = SkillRanks.GroupBy(x => x.Id)
          .Where(x => x.Key.HasValue && x.Count() > 1)
          .Select(x => x.Key!.Value);
        if (skillRankIds.Any())
        {
          results.Add(new ValidationResult(
            errorMessage: $"Each skill rank must only appear once: {string.Join(", ", skillRankIds)}.",
            memberNames: new[] { nameof(SkillRanks) }
          ));
        }
      }

      if (Talents != null)
      {
        IEnumerable<Guid> talentIds = Talents.GroupBy(x => x.Id)
          .Where(x => x.Key.HasValue && x.Count() > 1)
          .Select(x => x.Key!.Value);
        if (talentIds.Any())
        {
          results.Add(new ValidationResult(
            errorMessage: $"Each talent must only appear once: {string.Join(", ", talentIds)}.",
            memberNames: new[] { nameof(Talents) }
          ));
        }
      }

      return results;
    }
  }
}
