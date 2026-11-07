using API.Application;
using API.Contract.Settings;
using API.Infrastructure;
using DotNetEnv;
using IdentityAPI.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace IdentityAPI
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
                    Title = "Identity Service",
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

        public static IServiceCollection AddJwtConfig(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.AddOptions<AuthCookieSettings>()
                    .Bind(configuration.GetSection(AuthCookieSettings.SectionName));

            var jwtSettings = configuration
                .GetSection("Jwt")
                .Get<JwtSettings>()
                ?? throw new InvalidOperationException(
                    "JwtSettings configuration is missing.");

            if (jwtSettings is not null)
            {
                services
                    .AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSettings.Issuer,
                            ValidAudience = jwtSettings.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                System.Text.Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                        };
                    });
            }

            services
                .AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .Configure<IOptions<AuthCookieSettings>>((options, cookieOptions) =>
                {
                    var cookieName = cookieOptions.Value.AccessTokenCookieName;
                    var events = options.Events ?? new JwtBearerEvents();
                    var previous = events.OnMessageReceived;

                    events.OnMessageReceived = async context =>
                    {
                        if (previous is not null)
                        {
                            await previous(context);
                        }

                        if (string.IsNullOrEmpty(context.Token)
                            && !context.Request.Headers.ContainsKey("Authorization")
                            && context.Request.Cookies.TryGetValue(cookieName, out var token)
                            && !string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }

                        if (string.IsNullOrEmpty(context.Token))
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                            {
                                context.Token = accessToken;
                            }
                        }
                    };

                    options.Events = events;
                });

            return services;
        }
    }
}
