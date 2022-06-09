using Logitar.WebApiToolKit.Core.Exceptions;

namespace SkillCraft.Web
{
  public class ExceptionData
  {
    public ExceptionData(Exception exception)
    {
      ArgumentNullException.ThrowIfNull(exception);

      HelpLink = exception.HelpLink;
      HResult = exception.HResult;
      InnerException = exception.InnerException == null ? null : new ExceptionData(exception.InnerException);
      Message = exception.Message;
      Source = exception.Source;
      StackTrace = exception.StackTrace;
      Type = exception.GetType().AssemblyQualifiedName ?? exception.GetType().Name;
      Value = exception is ApiException apiException ? apiException.Value : null;
    }

    public string? HelpLink { get; }
    public int HResult { get; }
    public ExceptionData? InnerException { get; }
    public string Message { get; }
    public string? Source { get; }
    public string? StackTrace { get; }
    public string Type { get; }
    public object? Value { get; }
  }
}
