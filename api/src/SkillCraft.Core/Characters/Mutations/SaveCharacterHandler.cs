using AutoMapper;
using Logitar;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Characters.Payloads;

namespace SkillCraft.Core.Characters.Mutations
{
  internal abstract class SaveCharacterHandler
  {
    protected SaveCharacterHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      AppContext = appContext;
      DbContext = dbContext;
      Mapper = mapper;
    }

    protected IApplicationContext AppContext { get; }
    protected IDbContext DbContext { get; }
    protected IMapper Mapper { get; }

    protected async Task<CharacterModel> ExecuteAsync(Character character, SaveCharacterPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(character);
      ArgumentNullException.ThrowIfNull(payload);

      character.Name = payload.Name.Trim();
      character.Player = payload.Player?.CleanTrim();

      // Aspect1Id
      // Aspect2Id
      // RaceId
      // NatureId
      // CasteId
      // EducationId

      character.Size = payload.Size;
      character.Stature = payload.Stature;
      character.Weight = payload.Weight;
      character.Age = payload.Age;

      character.Experience = payload.Experience;
      character.Vitality = payload.Vitality;
      character.Stamina = payload.Stamina;

      character.BloodAlcoholContent = payload.BloodAlcoholContent;
      character.Intoxication = payload.Intoxication;

      character.Description = payload.Description?.CleanTrim();

      UpdateCharacterCreation(character, payload);
      UpdateSkillRanks(character, payload);
      // Bonuses
      // Conditions
      // CustomizationIds
      // LanguageIds
      // Talents

      await DbContext.SaveChangesAsync(cancellationToken);

      AppContext.SetEntity(character);

      return Mapper.Map<CharacterModel>(character);
    }

    private static void UpdateCharacterCreation(Character character, SaveCharacterPayload payload)
    {
      if (payload.Creation == null)
      {
        character.Creation = null;
      }
      else
      {
        character.Creation ??= new();
        character.Creation.AttributeBases ??= new();

        character.Creation.AttributeBases.Agility = payload.Creation.AttributeBases.Agility;
        character.Creation.AttributeBases.Coordination = payload.Creation.AttributeBases.Coordination;
        character.Creation.AttributeBases.Intellect = payload.Creation.AttributeBases.Intellect;
        character.Creation.AttributeBases.Mind = payload.Creation.AttributeBases.Mind;
        character.Creation.AttributeBases.Presence = payload.Creation.AttributeBases.Presence;
        character.Creation.AttributeBases.Sensitivity = payload.Creation.AttributeBases.Sensitivity;
        character.Creation.AttributeBases.Vigor = payload.Creation.AttributeBases.Vigor;

        character.Creation.BestAttribute = payload.Creation.BestAttribute;
        character.Creation.WorstAttribute = payload.Creation.WorstAttribute;
        character.Creation.MandatoryAttribute1 = payload.Creation.MandatoryAttribute1;
        character.Creation.MandatoryAttribute2 = payload.Creation.MandatoryAttribute2;
        character.Creation.OptionalAttribute1 = payload.Creation.OptionalAttribute1;
        character.Creation.OptionalAttribute2 = payload.Creation.OptionalAttribute2;

        //character.Creation.Step = null; // TODO(fpion): implement
      }
    }

    private void UpdateSkillRanks(Character character, SaveCharacterPayload payload)
    {
      throw new NotImplementedException(); // TODO(fpion): implement
    }
  }
}
