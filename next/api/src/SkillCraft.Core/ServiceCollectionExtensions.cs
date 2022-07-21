using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Core.Aspects;
using SkillCraft.Core.Castes;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Educations;
using SkillCraft.Core.Natures;
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
        .AddScoped<IAspectService, AspectService>()
        .AddScoped<ICasteService, CasteService>()
        .AddScoped<ICustomizationService, CustomizationService>()
        .AddScoped<IEducationService, EducationService>()
        .AddScoped<INatureService, NatureService>()
        .AddScoped<IWorldService, WorldService>();
    }
  }
}
