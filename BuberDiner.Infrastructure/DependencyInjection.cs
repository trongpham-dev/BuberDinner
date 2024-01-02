

using BuberDiner.Infrastructure.Authentication;
using BuberDiner.Infrastructure.Persistence;
using BuberDiner.Infrastructure.Services;
using BuberDinner.Application.common.interfaces.Authentication;
using BuberDinner.Application.common.interfaces.Persistance;
using BuberDinner.Application.common.interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuberDinner.Application
{  
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}