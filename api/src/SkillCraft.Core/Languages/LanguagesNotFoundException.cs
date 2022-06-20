using Logitar.WebApiToolKit.Core.Exceptions;
using System.Text;

namespace SkillCraft.Core.Languages
{
  internal class LanguagesNotFoundException : NotFoundException
  {
    public LanguagesNotFoundException(IEnumerable<Guid> ids) : base(message: GetMessage(ids))
    {
      Ids = ids ?? throw new ArgumentNullException(nameof(ids));
      Value = new
      {
        LanguageIds = string.Join(',', ids)
      };
    }

    public IEnumerable<Guid> Ids { get; }

    private static string GetMessage(IEnumerable<Guid> ids)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified languages could not be found.");
      message.AppendLine($"Ids: {string.Join(", ", ids ?? Enumerable.Empty<Guid>())}");

      return message.ToString();
    }
  }
}
