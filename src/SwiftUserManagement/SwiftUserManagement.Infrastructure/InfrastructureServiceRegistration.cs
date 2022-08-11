using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SwiftUserManagement.Application.Contracts.Infrastructure;
using SwiftUserManagement.Application.Contracts.Persistence;
using SwiftUserManagement.Infrastructure.Persistence;
using SwiftUserManagement.Infrastructure.Repositories;

namespace SwiftUserManagement.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            // Setting up the repositories
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddScoped<IJWTManagementRepository, JWTManagementRepository>();
            services.AddScoped<IRabbitMQRepository, RabbitMQRepository>();
            return services;
        }
    }
    
}
