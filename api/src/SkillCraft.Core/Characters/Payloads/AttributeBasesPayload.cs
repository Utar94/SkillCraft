using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Characters.Payloads
{
  public class AttributeBasesPayload
  {
    [Range(3, 11)]
    public int Agility { get; set; }

    [Range(3, 11)]
    public int Coordination { get; set; }

    [Range(3, 11)]
    public int Intellect { get; set; }

    [Range(3, 11)]
    public int Mind { get; set; }

    [Range(3, 11)]
    public int Presence { get; set; }

    [Range(3, 11)]
    public int Sensitivity { get; set; }

    [Range(3, 11)]
    public int Vigor { get; set; }
  }
}
