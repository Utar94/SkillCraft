namespace SkillCraft.Web
{
  public struct ErrorHandler
  {
    public ErrorHandler(int statusCode, object? value = null)
    {
      StatusCode = statusCode;
      Value = value;
    }

    public int StatusCode { get; }
    public object? Value { get; }
  }
}
