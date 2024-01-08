

using BuberDiner.Infrastructure.Authentication;
using BuberDiner.Infrastructure.Persistence;
using BuberDiner.Infrastructure.Services;
using BuberDinner.Application.common.interfaces.Authentication;
using BuberDinner.Application.common.interfaces.Persistance;
using BuberDinner.Application.common.interfaces.Services;
using BuberDinner.Application.Common.Interfaces.Persistance;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BuberDinner.Application
{  
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services
                .AddAuth(configuration)
                .AddPersistance(configuration);
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            return services;
        }

        public static IServiceCollection AddPersistance(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            return services;
        }

        public static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, jwtSettings);

            //services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.AddSingleton(Options.Create(jwtSettings));
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // authication handler - passing parameters that we want to validate to populate identity for user login
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                    });

            return services;
        }
    }
}