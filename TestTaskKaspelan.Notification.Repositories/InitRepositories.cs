using Microsoft.Extensions.DependencyInjection;
using TestTaskKaspelan.Notification.Repositories.Interfaces;
using TestTaskKaspelan.Notification.Repositories.Repositories;

namespace TestTaskKaspelan.Notification.Repositories
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
            services.AddSingleton<IEmailRepository, EmailRepository>();
        }
    }
}
