using Logitar.WebApiToolKit.Core.Exceptions;
using System.Text;

namespace SkillCraft.Core
{
  internal class UnauthorizedOperationException<T> : ForbiddenException
  {
    public UnauthorizedOperationException(T entity, Guid userId) : base(GetMessage(entity, userId))
    {
      Entity = entity ?? throw new ArgumentNullException(nameof(entity));
      UserId = userId;
    }

    public T Entity { get; }
    public Guid UserId { get; }

    private static string GetMessage(T entity, Guid userId)
    {
      var message = new StringBuilder();

      message.AppendLine("An unauthorized operation has been prevented.");
      message.AppendLine($"Entity: {entity}");
      message.AppendLine($"UserId: {userId}");

      return message.ToString();
    }
  }
}
