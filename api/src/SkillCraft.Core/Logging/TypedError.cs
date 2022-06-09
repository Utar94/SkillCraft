namespace SkillCraft.Core.Logging
{
  public class TypedError<T> : Error
  {
    protected TypedError() : base()
    {
    }
    protected TypedError(ErrorSeverity severity, ErrorCode? code = null, string? message = null, T? data = default, object? value = null)
        : base(severity, code, message)
    {
      Data = data;
      base.Value = value;
    }

    public T? Data { get; set; }

    public static TypedError<T> Critical(ErrorCode? code = null, string? message = null, T? data = default, object? value = null)
      => new(ErrorSeverity.Critical, code, message, data, value);
    public static TypedError<T> Failure(ErrorCode? code = null, string? message = null, T? data = default, object? value = null)
      => new(ErrorSeverity.Failure, code, message, data, value);
    public static TypedError<T> Warning(ErrorCode? code = null, string? message = null, T? data = default, object? value = null)
      => new(ErrorSeverity.Warning, code, message, data, value);

    public static ErrorException CriticalException(ErrorCode? code = null, string? message = null, T? data = default, object? value = null)
      => new(Critical(code, message, data, value));
    public static ErrorException FailureException(ErrorCode? code = null, string? message = null, T? data = default, object? value = null)
      => new(Failure(code, message, data, value));
    public static ErrorException WarningException(ErrorCode? code = null, string? message = null, T? data = default, object? value = null)
      => new(Warning(code, message, data, value));
  }
}
