using Logitar.WebApiToolKit.Core.Exceptions;
using System.Text;

namespace SkillCraft.Core
{
  public class EntityNotFoundException<T> : NotFoundException
  {
    public EntityNotFoundException(Guid id, string? paramName = null, string? message = null, Exception? innerException = null)
      : this(id.ToString(), paramName, message, innerException)
    {
    }
    public EntityNotFoundException(
      string id,
      string? paramName = null,
      string? message = null,
      Exception? innerException = null
    ) : base(paramName, message ?? GetMessage(id), innerException)
    {
      Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    public string Id { get; }

    private static string GetMessage(string id)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified entity could not be found.");
      message.AppendLine($"Type: {typeof(T).AssemblyQualifiedName}");
      message.AppendLine($"Id: {id}");

      return message.ToString();
    }
  }
}
