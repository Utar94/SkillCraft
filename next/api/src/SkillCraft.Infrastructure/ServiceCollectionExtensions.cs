using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Core;
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
        .AddScoped<IWorldQuerier, WorldQuerier>();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
      return services
        .AddScoped<IRepository<World>, Repository<World>>();
    }
  }
}
