using Microsoft.Extensions.DependencyInjection;
using TestTaskKaspelan.Auth.Services.Interfaces;
using TestTaskKaspelan.Auth.Services.Services;

namespace TestTaskKaspelan.Auth.Services
{
    /// <summary>
    /// Static class for extention methods for service.
    /// </summary>
    public static class InitServices
    {
        /// <summary>
        /// Add services to services collections.
        /// </summary>
        /// <param name="services">Services collection.</param>
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
