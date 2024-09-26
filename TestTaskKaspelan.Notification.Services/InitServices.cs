using Microsoft.Extensions.DependencyInjection;
using TestTaskKaspelan.Notification.Services.Interfaces;
using TestTaskKaspelan.Notification.Services.Services;
using TestTaskKaspelan.Notification.Repositories;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using TestTaskKaspelan.Common.Constants;

namespace TestTaskKaspelan.Notification.Services
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

            // Add queue to service container to be declared at startup
            var orderExchArgs = new Dictionary<String, Object>();
            orderExchArgs.Add("alternate-exchange", $"{RabbitMqNameTemplates.NewOrder}-unsupported");
            services.AddRabbitExchange(new DirectExchange(RabbitMqNameTemplates.NewOrder, true, false, orderExchArgs));

            // Queue for version 1.0
            var currentVersion = "1.0";
            var version1QueueName = $"{RabbitMqNameTemplates.NewOrder}-{currentVersion}";
            services.AddRabbitQueue(new Queue(version1QueueName, true, false, false));
            services.AddRabbitBinding(new Binding($"{RabbitMqNameTemplates.NewOrder} to {currentVersion}", version1QueueName, Binding.DestinationType.QUEUE, RabbitMqNameTemplates.NewOrder, currentVersion, null));

            // Queue for unsupported version
            var versionUnsupportedQueueName = $"{RabbitMqNameTemplates.NewOrder}-unsupported";
            services.AddRabbitQueue(new Queue(versionUnsupportedQueueName, true, false, false));
            services.AddRabbitBinding(new Binding($"{RabbitMqNameTemplates.NewOrder} to unsupported", versionUnsupportedQueueName, Binding.DestinationType.QUEUE, RabbitMqNameTemplates.NewOrder, "", null));

            // Add the rabbit listener
            services.AddSingleton<NewOrderNotificationReceiver>();
            services.AddRabbitListeners<NewOrderNotificationReceiver>();
        }
    }
}
