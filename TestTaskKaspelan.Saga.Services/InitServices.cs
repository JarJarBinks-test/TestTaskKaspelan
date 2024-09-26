using Microsoft.Extensions.DependencyInjection;
using TestTaskKaspelan.Saga.Services.Interfaces;
using TestTaskKaspelan.Saga.Services.Services;
using TestTaskKaspelan.Saga.Repositories;

namespace TestTaskKaspelan.Saga.Services
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
            services.AddRepositories();
            services.AddScoped<IOrderService, OrderService>();
        }
    }
}
