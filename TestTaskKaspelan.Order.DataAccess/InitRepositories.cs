using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestTaskKaspelan.Order.DataAccess.Contexts;
using TestTaskKaspelan.Order.DataAccess.Interfaces;
using TestTaskKaspelan.Order.DataAccess.Repositories;

namespace TestTaskKaspelan.Order.DataAccess
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
        /// <param name="configuration">Configuration.</param>
        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderContext>((ob) => {
                var connectionString = configuration.GetConnectionString("order.mssql.connectionstring");
                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    ob.UseSqlServer(connectionString);
                }
                else
                {
                    ob.UseInMemoryDatabase($"{nameof(InitRepositories)}.InMemoryDb");
                }
            });
            services.AddScoped<IOrderRepository, OrderRepository>();
        }

        /// <summary>
        /// Setup repositories.
        /// </summary>
        /// <param name="services">Services provider.</param>
        public static void SetupRepositories(this IServiceProvider services)
        {
            using (var serviceScope = services.CreateScope())
            {
                var treeContext = serviceScope.ServiceProvider.GetRequiredService<OrderContext>();
                treeContext.Database.EnsureCreated();
            }
        }
    }
}
