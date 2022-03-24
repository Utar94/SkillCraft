using Microsoft.Extensions.DependencyInjection;

namespace SkillCraft.Core
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
      return services;
    }
  }
}
