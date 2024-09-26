using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TestTaskKaspelan.Order.DataAccess;
using TestTaskKaspelan.Order.Services.Interfaces;
using TestTaskKaspelan.Order.Services.Services;
using Steeltoe.Messaging.RabbitMQ.Extensions;

namespace TestTaskKaspelan.Order.Services
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
        /// <param name="configuration">Configuration.</param>
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepositories(configuration);
            services.AddScoped<IOrderService, OrderService>();
            services.AddRabbitServices();
            services.AddRabbitAdmin();
            services.AddRabbitTemplate();
        }

        /// <summary>
        /// Setup services.
        /// </summary>
        /// <param name="services">Services provider.</param>
        public static void SetupServices(this IServiceProvider services)
        {
            services.SetupRepositories();
        }
    }
}
