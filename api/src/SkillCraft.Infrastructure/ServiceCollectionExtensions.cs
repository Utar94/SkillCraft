using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Core;
using SkillCraft.Core.Repositories;
using SkillCraft.Infrastructure.Repositories;

namespace SkillCraft.Infrastructure
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddSkillCraftInfrastructure(this IServiceCollection services)
    {
      return services
        .AddDbContext<IDbContext, SkillCraftDbContext>(ConfigureDbContext)
        .AddDbContext<SkillCraftDbContext>(ConfigureDbContext)
        .AddScoped<ICharacterRepository, CharacterRepository>();
    }

    private static void ConfigureDbContext(IServiceProvider provider, DbContextOptionsBuilder builder)
    {
      var configuration = provider.GetRequiredService<IConfiguration>();
      builder.UseNpgsql(configuration.GetValue<string>($"POSTGRESQLCONNSTR_{nameof(SkillCraftDbContext)}"));
    }
  }
}
