using Logitar.WebApiToolKit.Core.Exceptions;
using System.Text;

namespace SkillCraft.Core.Castes
{
  public class CasteTraitsNotFoundException : NotFoundException
  {
    public CasteTraitsNotFoundException(
      IEnumerable<Guid> ids,
      string? message = null,
      Exception? innerException = null
    ) : base(message ?? GetMessage(ids), innerException: innerException)
    {
      ArgumentNullException.ThrowIfNull(ids);

      Value = new { ids };
    }

    private static string GetMessage(IEnumerable<Guid> ids)
    {
      ArgumentNullException.ThrowIfNull(ids);

      return $"The following caste traits could not be found: {string.Join(", ", ids.OrderBy(x => x))}.";
    }
  }
}
