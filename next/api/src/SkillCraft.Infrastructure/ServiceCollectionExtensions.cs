using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Core;
using SkillCraft.Core.Aspects;
using SkillCraft.Core.Castes;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Educations;
using SkillCraft.Core.Natures;
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
        .AddScoped<ICasteQuerier, CasteQuerier>()
        .AddScoped<ICustomizationQuerier, CustomizationQuerier>()
        .AddScoped<IEducationQuerier, EducationQuerier>()
        .AddScoped<INatureQuerier, NatureQuerier>()
        .AddScoped<IWorldQuerier, WorldQuerier>();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
      return services
        .AddScoped<IRepository<Aspect>, Repository<Aspect>>()
        .AddScoped<IRepository<Caste>, Repository<Caste>>()
        .AddScoped<IRepository<Customization>, Repository<Customization>>()
        .AddScoped<IRepository<Education>, Repository<Education>>()
        .AddScoped<IRepository<Nature>, Repository<Nature>>()
        .AddScoped<IRepository<World>, Repository<World>>();
    }
  }
}
