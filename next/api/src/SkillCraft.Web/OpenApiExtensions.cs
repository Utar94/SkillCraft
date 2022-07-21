using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SkillCraft.Web
{
  internal static class OpenApiExtensions
  {
    private const string AuthorizationHeader = "Authorization";
    private const string BearerScheme = "Bearer";

    public static IServiceCollection AddOpenApi(this IServiceCollection services)
    {
      ArgumentNullException.ThrowIfNull(services);

      return services.AddSwaggerGen(config =>
      {
        config.ConfigureJwtBearerSecurity();

        config.OperationFilterDescriptors.Add(new FilterDescriptor
        {
          Arguments = Array.Empty<object>(),
          Type = typeof(AddHeaderParameters)
        });

        config.SwaggerDoc(name: "v1", new OpenApiInfo
        {
          Contact = new OpenApiContact
          {
            Email = "francispion@hotmail.com",
            Name = "Francis Pion",
            Url = new Uri("https://www.francispion.ca/")
          },
          Description = "SkillCraft management Web API.",
          License = new OpenApiLicense
          {
            Name = "Use under MIT",
            Url = new Uri("https://github.com/Utar94/SkillCraft/blob/develop/LICENSE")
          },
          Title = "SkillCraft API",
          Version = "v1.0"
        });
      });
    }

    private static void ConfigureJwtBearerSecurity(this SwaggerGenOptions options)
    {
      options.AddSecurityDefinition(BearerScheme, new OpenApiSecurityScheme
      {
        Description = "Enter your access token in the input below.",
        In = ParameterLocation.Header,
        Name = AuthorizationHeader,
        Scheme = BearerScheme,
        Type = SecuritySchemeType.Http
      });

      options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
              In = ParameterLocation.Header,
              Name = AuthorizationHeader,
              Reference = new OpenApiReference
              {
                Id = BearerScheme,
                Type = ReferenceType.SecurityScheme
              },
              Scheme = BearerScheme
            },
            new List<string>()
          }
        });
    }
  }
}
