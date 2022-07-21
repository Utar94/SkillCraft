using System.Net;
using System.Text;

namespace SkillCraft.Core
{
  internal class ForbiddenException<T> : ApiException where T : Aggregate
  {
    public ForbiddenException(T aggregate, Guid userId)
      : base(HttpStatusCode.Forbidden, GetMessage(aggregate, userId))
    {
      Aggregate = aggregate ?? throw new ArgumentNullException(nameof(aggregate));
      UserId = userId;
    }

    public T Aggregate { get; }
    public Guid UserId { get; }

    private static string GetMessage(T aggregate, Guid userId)
    {
      var message = new StringBuilder();

      message.AppendLine("An unauthorized operation has been prevented.");
      message.AppendLine($"Aggregate: {aggregate}");
      message.AppendLine($"UserId: {userId}");

      return message.ToString();
    }
  }
}
