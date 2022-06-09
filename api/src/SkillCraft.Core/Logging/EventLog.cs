using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace SkillCraft.Core.Logging
{
  public class EventLog : EntityBase
  {
    public EventLog(Guid userId) : base(userId)
    {
      StartedAt = DateTime.UtcNow;
    }
    private EventLog() : base()
    {
    }

    public string Name { get; set; } = null!;
    public string? Method { get; set; }
    public string? Url { get; set; }
    public string? Data { get; set; }

    public int? StatusCode { get; set; }

    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public TimeSpan? RequestTime
    {
      get => EndedAt.HasValue ? EndedAt.Value - StartedAt : null;
      set
      {
      }
    }

    public bool IsCompleted
    {
      get => StatusCode.HasValue && EndedAt.HasValue;
      set
      {
      }
    }

    public LogLevel Level
    {
      get
      {
        if (HasErrors)
        {
          ErrorSeverity severity = Errors.Max(x => x.Severity);
          switch (severity)
          {
            case ErrorSeverity.Critical:
              return LogLevel.Critical;
            case ErrorSeverity.Failure:
              return LogLevel.Error;
            case ErrorSeverity.Warning:
              return LogLevel.Warning;
          }
        }

        return LogLevel.Information;
      }
      set
      {
      }
    }
    public bool HasErrors
    {
      get => Errors.Any();
      set
      {
      }
    }
    public ICollection<Error> Errors { get; set; } = new List<Error>();
    #region Serialization
    /// <summary>
    /// NOTE(fpion): nécessaire afin d'obtenir les propriétés des classes héritant de Error.
    /// <see href="https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-polymorphism">Source</see>
    /// </summary>
    public string? SerializedErrors
    {
      get
      {
        if (Errors.Any())
        {
          return string.Concat(
              '[',
              string.Join(',', Errors.Select(error => JsonSerializer.Serialize<object>(error))),
              ']'
          );
        }

        return null;
      }
      set
      {
        if (string.IsNullOrEmpty(value))
        {
          Errors.Clear();
        }
        else
        {
          Errors = JsonSerializer.Deserialize<ICollection<Error>>(value) ?? new List<Error>();
        }
      }
    }
    #endregion

    public Guid? WorldId { get; set; }
    public Guid? EntityId { get; set; }
    public Type? EntityType { get; set; }
    public string? EntityTypeName
    {
      get => EntityType?.AssemblyQualifiedName ?? EntityType?.FullName ?? EntityType?.Name;
      set => EntityType = value == null ? null : Type.GetType(value);
    }

    public void Complete(int statusCode)
    {
      EndedAt = DateTime.UtcNow;
      StatusCode = statusCode;
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
