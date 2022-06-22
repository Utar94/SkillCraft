using System.Text.Json;

namespace SkillCraft.Core.Characters
{
  public abstract class BonusBase
  {
    private const char Separator = '|';

    public Guid Id { get; set; } = Guid.NewGuid();

    public string? Description { get; set; }
    public bool Permanent { get; set; }
    public int Value { get; set; }

    public static BonusBase Deserialize(string json)
    {
      ArgumentNullException.ThrowIfNull(json);

      int index = json.IndexOf(Separator);
      if (index < 0)
      {
        throw new ArgumentException($"The type/value separator '{Separator}' was not found.{Environment.NewLine}Input: {json}", nameof(json));
      }

      string typeName = json[..index];
      Type type = Type.GetType(typeName) ?? throw new ArgumentException($"The type could not be resolved.{Environment.NewLine}", nameof(json));

      string value = json[(index + 1)..];
      return (BonusBase)(JsonSerializer.Deserialize(value, type) ?? throw new ArgumentException($"The value could not be resolved.{Environment.NewLine}Value: {value}", nameof(json)));
    }

    public string Serialize()
    {
      Type type = GetType();

      return string.Join(Separator, new[]
      {
        type.AssemblyQualifiedName ?? type.FullName ?? type.Name,
        JsonSerializer.Serialize(this, type)
      });
    }
  }
}
