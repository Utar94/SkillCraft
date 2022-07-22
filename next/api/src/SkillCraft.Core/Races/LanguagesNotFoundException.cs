using System.Net;
using System.Text;

namespace SkillCraft.Core.Races
{
  internal class LanguagesNotFoundException : ApiException
  {
    public LanguagesNotFoundException(IEnumerable<Guid> ids, string paramName)
      : base(HttpStatusCode.NotFound, GetMessage(ids, paramName))
    {
      Ids = ids ?? throw new ArgumentNullException(nameof(ids));
      ParamName = paramName ?? throw new ArgumentNullException(nameof(paramName));

      Value = new Dictionary<string, object>
      {
        [paramName] = ids
      };
    }

    public IEnumerable<Guid> Ids { get; }
    public string ParamName { get; }

    private static string GetMessage(IEnumerable<Guid> ids, string paramName)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified langages could not be found.");
      message.AppendLine($"Ids: {string.Join(", ", ids)}");
      message.AppendLine($"ParamName: {paramName}");

      return message.ToString();
    }
  }
}
