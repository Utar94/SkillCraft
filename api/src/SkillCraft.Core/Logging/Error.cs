namespace SkillCraft.Core.Logging
{
  public class Error
  {
    protected Error()
    {
    }
    protected Error(ErrorSeverity severity, ErrorCode? code = null, string? message = null)
    {
      Code = code;
      Message = message;
      Severity = severity;
    }

    public ErrorCode? Code { get; set; }
    public string? Message { get; set; }
    public ErrorSeverity Severity { get; set; }

    public virtual object? Value { get; set; }

    public static Error Critical(ErrorCode? code = null, string? message = null)
      => new(ErrorSeverity.Critical, code, message);
    public static Error Failure(ErrorCode? code = null, string? message = null)
      => new(ErrorSeverity.Failure, code, message);
    public static Error Warning(ErrorCode? code = null, string? message = null)
      => new(ErrorSeverity.Warning, code, message);

    public override string ToString() => string.Join(' ', new[]
    {
      $"[{Severity}]",
      (Code.HasValue ? (int?)Code.Value : null).ToString(),
      Code?.ToString(),
      Message
    }.Where(part => !string.IsNullOrEmpty(part)));
  }
}
