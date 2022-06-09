using Logitar.WebApiToolKit.Core.Exceptions;
using System.Text;

namespace SkillCraft.Core
{
  internal class EntityNotFoundException<T> : NotFoundException
  {
    public EntityNotFoundException(Guid id, string? paramName = null) : base(paramName, GetMessage(id))
    {
      Id = id;
    }

    public Guid Id { get; }

    private static string GetMessage(Guid id)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified entity could not be found.");
      message.AppendLine($"Type: {typeof(T)}");
      message.AppendLine($"Id: {id}");

      return message.ToString();
    }
  }
}
