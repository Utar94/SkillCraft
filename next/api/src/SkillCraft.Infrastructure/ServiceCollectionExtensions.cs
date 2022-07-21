using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Core;
using SkillCraft.Core.Aspects;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Worlds;
using SkillCraft.Infrastructure.Queriers;
using SkillCraft.Infrastructure.Repositories;

namespace SkillCraft.Infrastructure
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddSkillCraftInfrastructure(this IServiceCollection services)
    {
      return services
        .AddDbContext<SkillCraftDbContext>()
        .AddQueriers()
        .AddRepositories()
        .AddScoped<IDatabaseService, DatabaseService>();
    }

    private static IServiceCollection AddQueriers(this IServiceCollection services)
    {
      return services
        .AddScoped<IAspectQuerier, AspectQuerier>()
        .AddScoped<ICustomizationQuerier, CustomizationQuerier>()
        .AddScoped<IWorldQuerier, WorldQuerier>();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
      return services
        .AddScoped<IRepository<Aspect>, Repository<Aspect>>()
        .AddScoped<IRepository<Customization>, Repository<Customization>>()
        .AddScoped<IRepository<World>, Repository<World>>();
    }
  }
}
