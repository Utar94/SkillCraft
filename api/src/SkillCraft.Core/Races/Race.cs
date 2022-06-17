using SkillCraft.Core.Languages;
using SkillCraft.Core.Worlds;
using System.Text.Json;

namespace SkillCraft.Core.Races
{
  public class Race : EntityBase
  {
    public Race(Guid userId, World world) : base(userId)
    {
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Race() : base()
    {
    }

    public World? World { get; set; }
    public int WorldId { get; set; }

    public string? Description { get; set; }
    public string Name { get; set; } = null!;

    public Dictionary<Attribute, int> Attributes { get; set; } = new();
    public string? AttributesSerialized
    {
      get => Attributes.Any()? string.Join('|', Attributes.Select(x => $"{x.Key}:{x.Value}")) : null;
      set
      {
        Attributes.Clear();

        if (value != null)
        {
          Attributes = value.Split('|')
            .Select(x => x.Split(':'))
            .ToDictionary(x => Enum.Parse<Attribute>(x[0]), x => int.Parse(x[1]));
        }
      }
    }
    public Dictionary<string, HashSet<string>> Names { get; set; } = new();
    public string? NamesSerialized
    {
      get => Names.Any() ? JsonSerializer.Serialize(Names) : null;
      set
      {
        Names.Clear();

        if (value != null)
        {
          Names = JsonSerializer.Deserialize<Dictionary<string, HashSet<string>>>(value) ?? new();
        }
      }
    }
    public Dictionary<SpeedType, int> Speeds { get; set; } = new();
    public string? SpeedsSerialized
    {
      get => Speeds.Any() ? string.Join('|', Speeds.Select(x => $"{x.Key}:{x.Value}")) : null;
      set
      {
        Speeds.Clear();

        if (value != null)
        {
          Speeds = value.Split('|')
            .Select(x => x.Split(':'))
            .ToDictionary(x => Enum.Parse<SpeedType>(x[0]), x => int.Parse(x[1]));
        }
      }
    }

    public int[]? AgeThresholds { get; set; }
    public SizeCategory Size { get; set; }
    public string? StatureRoll { get; set; }
    public string[]? WeightRolls { get; set; }

    public ICollection<Language> Languages { get; set; } = new List<Language>();

    public ICollection<RacialTrait> Traits { get; set; } = new List<RacialTrait>();
    public string? TraitsSerialized
    {
      get => Traits.Any() ? JsonSerializer.Serialize(Traits) : null;
      set
      {
        Traits.Clear();

        if (value != null)
        {
          Traits = JsonSerializer.Deserialize<List<RacialTrait>>(value) ?? new();
        }
      }
    }

    public int ExtraAttributes { get; set; }
    public int ExtraLanguages { get; set; }

    public string? AgeText { get; set; }
    public string? AttributesText { get; set; }
    public string? LanguagesText { get; set; }
    public string? NamesText { get; set; }
    public string? SizeText { get; set; }
    public string? SpeedText { get; set; }
    public string? SubraceText { get; set; }
    public string? TraitsText { get; set; }
    public string? WeightText { get; set; }
  }
}
