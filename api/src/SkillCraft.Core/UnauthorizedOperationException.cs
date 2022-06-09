using Logitar.WebApiToolKit.Core.Exceptions;
using SkillCraft.Core.Worlds;
using System.Text;

namespace SkillCraft.Core
{
  internal class UnauthorizedOperationException<T> : ForbiddenException
  {
    public UnauthorizedOperationException(T entity, Guid userId, World? world = null)
      : base(GetMessage(entity, userId, world))
    {
      Entity = entity ?? throw new ArgumentNullException(nameof(entity));
      UserId = userId;
      World = world;
    }

    public T Entity { get; }
    public Guid UserId { get; }
    public World? World { get; }

    private static string GetMessage(T entity, Guid userId, World? world)
    {
      var message = new StringBuilder();

      message.AppendLine("An unauthorized operation has been prevented.");
      message.AppendLine($"Entity: {entity}");
      message.AppendLine($"UserId: {userId}");

      if (world != null)
      {
        message.AppendLine($"World: {world}");
      }

      return message.ToString();
    }
  }
}
