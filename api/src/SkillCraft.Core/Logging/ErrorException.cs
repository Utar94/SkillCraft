namespace SkillCraft.Core.Logging
{
  public class ErrorException : Exception
  {
    public ErrorException(Error error, string? message = null, Exception? innerException = null)
      : base(message ?? error?.Message ?? "An unexpected error has occurred.", innerException)
    {
      Error = error ?? throw new ArgumentNullException(nameof(error));
    }

    public Error Error { get; }
  }
}
