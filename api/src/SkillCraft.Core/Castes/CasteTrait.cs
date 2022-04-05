namespace SkillCraft.Core.Castes
{
  public class CasteTrait
  {
    public CasteTrait(Caste caste)
    {
      Caste = caste ?? throw new ArgumentNullException(nameof(caste));
      CasteId = caste.Id;
      Key = Guid.NewGuid();
    }
    private CasteTrait()
    {
    }

    public int CasteId { get; set; }
    public string? Description { get; set; }
    public int Id { get; set; }
    public Guid Key { get; set; }
    public string Name { get; set; } = null!;

    public Caste? Caste { get; set; }

    public override bool Equals(object? obj) => obj is CasteTrait trait && trait.Id == Id;
    public override int GetHashCode() => HashCode.Combine(GetType(), Id);
    public override string ToString() => $"{Name} | {GetType()} (Id={Id})";
  }
}
