using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Core.Characters;
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
        .AddMediatR(assembly)
        .AddScoped<ICharacterService, CharacterService>();
    }
  }
}
