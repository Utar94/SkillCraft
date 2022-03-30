using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SkillCraft.Core
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
      Assembly assembly = typeof(ServiceCollectionExtensions).Assembly;

      return services.AddAutoMapper(assembly);
    }
  }
}
