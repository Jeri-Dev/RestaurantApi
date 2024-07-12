using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace RestaurantApi.Presentation.WebApi.Extensions;

public static class ServiceExtension
{
    public static void AddSwaggerExtension(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            List<string> xmlFiles = Directory
                .GetFiles(AppContext.BaseDirectory, "*.xml", searchOption: SearchOption.TopDirectoryOnly).ToList();
            xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Restaurant API",
                Description =
                    "This API includes information about the restaurant like ingredients, dishes, tables and orders.",
                Contact = new OpenApiContact
                {
                    Name = "Jeriel Elieser Gomez Susaña",
                    Email = "20230296@itla.edu.do",
                    Url = new Uri("https://jerielgomez.netlify.app")
                }
            });

            options.DescribeAllParametersInCamelCase();
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Input your Bearer token in this format: Bearer {your token here}"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    }, new List<string>()
                }
            });
        });
    }

    public static void AddApiVersioningExtension(this IServiceCollection services)
    {
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });
    }
}