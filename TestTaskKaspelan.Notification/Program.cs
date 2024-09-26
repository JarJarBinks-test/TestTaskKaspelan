using Steeltoe.Discovery.Client;
using Steeltoe.Extensions.Configuration.ConfigServer;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Host;
using TestTaskKaspelan.Notification.Services;

namespace TestTaskKaspelan.Notification
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RabbitMQHost.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Add service discovery.
                    services.AddDiscoveryClient(hostContext.Configuration);

                    services.AddServices();

                    services.AddOptions();
                    // Configure rabbit options from the configuration
                    services.Configure<RabbitOptions>(hostContext.Configuration.GetSection(RabbitOptions.PREFIX));
                })
                .AddConfigServer()
                .Build()
                .Run();
        }
    }
}