using DotNetEnv;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using API.Application;
using API.Infrastructure;

namespace CatalogAPI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMyServices(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.AddInfrastructure(configuration);
            services.AddApplication();

            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.Converters.Add(
                    new JsonStringEnumConverter());
            });


            return services;
        }

        public static IServiceCollection AddCustomSwagger(
            this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Catalog Service",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Nhập JWT token."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }

        public static IServiceCollection AddEnvConfig(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            Env.Load();

            var searchDir = Directory.GetCurrentDirectory();
            while (searchDir is not null)
            {
                var envPath = Path.Combine(searchDir, ".env");
                if (File.Exists(envPath))
                {
                    Env.Load(envPath); break;
                }
                searchDir = Directory.GetParent(searchDir)?.FullName;
            }

            configuration.AddEnvironmentVariables();

            return services;
        }
    }
}
