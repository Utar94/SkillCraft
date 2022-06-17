using Logitar.WebApiToolKit.Core.Exceptions;
using System.Text;

namespace SkillCraft.Core.Races
{
  internal class RacialTraitsNotFoundException : NotFoundException
  {
    public RacialTraitsNotFoundException(IEnumerable<Guid> ids) : base(message: GetMessage(ids))
    {
      Ids = ids ?? throw new ArgumentNullException(nameof(ids));
      Value = new
      {
        Traits = string.Join(',', ids)
      };
    }

    public IEnumerable<Guid> Ids { get; }

    private static string GetMessage(IEnumerable<Guid> ids)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified racial traits could not be found.");
      message.AppendLine($"Ids: {string.Join(", ", ids ?? Enumerable.Empty<Guid>())}");

      return message.ToString();
    }
  }
}
