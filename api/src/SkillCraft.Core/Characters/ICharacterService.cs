using SkillCraft.Core.Characters.Payloads;

namespace SkillCraft.Core.Characters
{
  public interface ICharacterService
  {
    void UpdateBonuses(Character character, IEnumerable<BonusPayload>? payloads);
    void UpdateCharacterCreation(Character character, CharacterCreationPayload payload);
    void UpdateSkillRanks(Character character, IEnumerable<SkillRankPayload>? payloads);

    Task UpdateConditionsAsync(Character character, IEnumerable<CharacterConditionPayload>? payloads, CancellationToken cancellationToken = default);
    Task UpdateCustomizationsAsync(Character character, ISet<Guid>? customizationIds, CancellationToken cancellationToken = default);
    Task UpdateLanguagesAsync(Character character, ISet<Guid>? languageIds, CancellationToken cancellationToken = default);
    Task UpdatePowersAsync(Character character, IEnumerable<CharacterPowerPayload>? payloads, CancellationToken cancellationToken = default);
    Task UpdateTalentsAsync(Character character, IEnumerable<CharacterTalentPayload>? payloads, CancellationToken cancellationToken = default);
  }
}
