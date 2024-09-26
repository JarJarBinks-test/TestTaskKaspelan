using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Common.Http.Discovery;
using TestTaskKaspelan.Common.Constants;
using TestTaskKaspelan.Saga.Repositories.Interfaces;
using TestTaskKaspelan.Saga.Repositories.Repositories;

namespace TestTaskKaspelan.Saga.Repositories
{
    /// <summary>
    /// Static class for extention methods for repositories.
    /// </summary>
    public static class InitRepositories
    {
        /// <summary>
        /// Add repositories and contexts to services collections.
        /// </summary>
        /// <param name="services">Services collection.</param>
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddHttpClient(ServicesNames.Order)
                .AddServiceDiscovery()
                .AddTypedClient<IOrderRepository, OrderRepository>();
        }
    }
}
