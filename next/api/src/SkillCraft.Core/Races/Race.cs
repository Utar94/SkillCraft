using Logitar;
using SkillCraft.Core.Languages;
using SkillCraft.Core.Races.Events;
using SkillCraft.Core.Races.Payload;
using SkillCraft.Core.Worlds;
using System.Text.Json;

namespace SkillCraft.Core.Races
{
  public class Race : Aggregate
  {
    public Race(CreateRacePayload payload, Guid userId, World world, Race? parent = null)
    {
      ApplyChange(new CreatedEvent(payload, userId));

      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldSid = world.Sid;

      Parent = parent;
      ParentSid = parent?.Sid;
    }
    private Race()
    {
    }

    public World? World { get; private set; }
    public int WorldSid { get; private set; }

    public Race? Parent { get; private set; }
    public int? ParentSid { get; private set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public Dictionary<Attribute, int> Attributes { get; private set; } = new();
    public string? AttributesSerialized
    {
      get => string.Join('|', Attributes.Where(x => x.Value > 0).Select(x => string.Join(':', x.Key, x.Value))).CleanTrim();
      set
      {
        Attributes.Clear();
        if (value != null)
        {
          string[] values = value.Split('|');
          foreach (string pair in values)
          {
            string[] split = pair.Split(':');
            Attributes.Add(Enum.Parse<Attribute>(split[0]), int.Parse(split[1]));
          }
        }
      }
    }
    public int ExtraAttributes { get; private set; }
    public string? AttributesText { get; private set; }

    public Dictionary<string, HashSet<string>> Names { get; private set; } = new();
    public string? NamesSerialized
    {
      get => Names.Any() ? JsonSerializer.Serialize(Names) : null;
      set
      {
        Names.Clear();
        if (value != null)
        {
          var nameCategories = JsonSerializer.Deserialize<Dictionary<string, HashSet<string>>>(value);
          if (nameCategories != null)
          {
            foreach (var (category, values) in nameCategories)
            {
              Names.Add(category, values);
            }
          }
        }
      }
    }
    public string? NamesText { get; private set; }

    public Dictionary<SpeedType, int> Speeds { get; private set; } = new();
    public string? SpeedsSerialized
    {
      get => string.Join('|', Speeds.Where(x => x.Value > 0).Select(x => string.Join(':', x.Key, x.Value))).CleanTrim();
      set
      {
        Speeds.Clear();
        if (value != null)
        {
          string[] values = value.Split('|');
          foreach (string pair in values)
          {
            string[] split = pair.Split(':');
            Speeds.Add(Enum.Parse<SpeedType>(split[0]), int.Parse(split[1]));
          }
        }
      }
    }
    public string? SpeedsText { get; private set; }

    public int[]? AgeThresholds { get; private set; }
    public SizeCategory Size { get; private set; }
    public string? StatureRoll { get; private set; }
    public string[]? WeightRolls { get; private set; }
    public string? AgeText { get; private set; }
    public string? SizeText { get; private set; }
    public string? WeightText { get; private set; }

    public List<Language> Languages { get; private set; } = new();
    public int ExtraLanguages { get; private set; }
    public string? LanguagesText { get; private set; }

    public List<RacialTrait> Traits { get; private set; } = new();
    public string? TraitsText { get; private set; }

    public List<Race> People { get; private set; } = new();
    public string? PeopleText { get; private set; }

    public void Delete(Guid userId) => ApplyChange(new DeletedEvent(userId));
    public void Update(UpdateRacePayload payload, Guid userId) => ApplyChange(new UpdatedEvent(payload, userId));

    protected virtual void Apply(CreatedEvent @event)
    {
      Apply(@event.Payload);
    }
    protected virtual void Apply(DeletedEvent @event)
    {
    }
    protected virtual void Apply(UpdatedEvent @event)
    {
      Apply(@event.Payload);
    }

    private void Apply(SaveRacePayload payload)
    {
      Name = payload.Name.Trim();
      Description = payload.Description?.CleanTrim();

      Attributes[Attribute.Agility] = payload.Attributes?.Agility ?? 0;
      Attributes[Attribute.Coordination] = payload.Attributes?.Coordination ?? 0;
      Attributes[Attribute.Intellect] = payload.Attributes?.Intellect ?? 0;
      Attributes[Attribute.Mind] = payload.Attributes?.Mind ?? 0;
      Attributes[Attribute.Presence] = payload.Attributes?.Presence ?? 0;
      Attributes[Attribute.Sensitivity] = payload.Attributes?.Sensitivity ?? 0;
      Attributes[Attribute.Vigor] = payload.Attributes?.Vigor ?? 0;
      ExtraAttributes = payload.ExtraAttributes;
      AttributesText = payload.AttributesText?.CleanTrim();

      Names.Clear();
      if (payload.Names != null)
      {
        foreach (NameCategoryPayload nameCategory in payload.Names)
        {
          if (!Names.TryGetValue(nameCategory.Category, out HashSet<string>? names))
          {
            names = new();
            Names.Add(nameCategory.Category, names);
          }

          foreach (string name in nameCategory.Values)
          {
            if (!string.IsNullOrWhiteSpace(name))
            {
              names.Add(name.Trim());
            }
          }
        }
      }
      NamesText = payload.NamesText?.CleanTrim();

      Speeds[SpeedType.Burrow] = payload.Speeds?.Burrow ?? 0;
      Speeds[SpeedType.Climb] = payload.Speeds?.Climb ?? 0;
      Speeds[SpeedType.Fly] = payload.Speeds?.Fly ?? 0;
      Speeds[SpeedType.Swim] = payload.Speeds?.Swim ?? 0;
      Speeds[SpeedType.Walk] = payload.Speeds?.Walk ?? 0;
      SpeedsText = payload.SpeedsText?.CleanTrim();

      AgeThresholds = payload.AgeThresholds == null ? null : new[]
      {
        payload.AgeThresholds.Teenager,
        payload.AgeThresholds.Adult,
        payload.AgeThresholds.Mature,
        payload.AgeThresholds.Venerable
      };
      Size = payload.Size;
      StatureRoll = payload.StatureRoll;
      WeightRolls = payload.WeightRolls == null ? null : new[]
      {
        payload.WeightRolls.Skinny,
        payload.WeightRolls.Thin,
        payload.WeightRolls.Normal,
        payload.WeightRolls.Overweight,
        payload.WeightRolls.Obese
      };
      AgeText = payload.AgeText?.CleanTrim();
      SizeText = payload.SizeText?.CleanTrim();
      WeightText = payload.WeightText?.CleanTrim();

      ExtraLanguages = payload.ExtraLanguages;
      LanguagesText = payload.LanguagesText?.CleanTrim();

      TraitsText = payload.TraitsText?.CleanTrim();

      PeopleText = payload.PeopleText?.CleanTrim();
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
