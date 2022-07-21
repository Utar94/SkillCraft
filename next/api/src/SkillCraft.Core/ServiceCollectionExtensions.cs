using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Core.Worlds;
using System.Reflection;

namespace SkillCraft.Core
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddSkillCraftCore(this IServiceCollection services)
    {
      Assembly assembly = typeof(ServiceCollectionExtensions).Assembly;

      return services
        .AddAutoMapper(assembly)
        .AddValidatorsFromAssembly(assembly, includeInternalTypes: true)
        .AddScoped<IWorldService, WorldService>();
    }
  }
}
