using Logitar.WebApiToolKit.Core.Exceptions;
using System.Text;

namespace SkillCraft.Core.Conditions
{
  internal class ConditionsNotFoundException : NotFoundException
  {
    public ConditionsNotFoundException(IEnumerable<Guid> ids) : base(message: GetMessage(ids))
    {
      Ids = ids ?? throw new ArgumentNullException(nameof(ids));
      Value = new
      {
        Conditions = string.Join(',', ids)
      };
    }

    public IEnumerable<Guid> Ids { get; }

    private static string GetMessage(IEnumerable<Guid> ids)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified conditions could not be found.");
      message.AppendLine($"Ids: {string.Join(", ", ids ?? Enumerable.Empty<Guid>())}");

      return message.ToString();
    }
  }
}
