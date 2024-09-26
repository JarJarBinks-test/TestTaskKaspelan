using System.Text.Json;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;
using TestTaskKaspelan.Common.Contracts;
using TestTaskKaspelan.Notification.Repositories.Entities;
using TestTaskKaspelan.Notification.Repositories.Interfaces;
using TestTaskKaspelan.Notification.Services.Interfaces;

namespace TestTaskKaspelan.Notification.Services.Services
{
    /// <summary>
    /// New order notification receiver.
    /// </summary>
    public class NewOrderNotificationReceiver : INewOrderNotificationReceiver
    {
        private readonly IEmailRepository _emailRepository;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewOrderNotificationReceiver"/> class.
        /// </summary>
        /// <param name="emailRepository">The email repository.</param>
        /// <param name="logger">The logger.</param>
        public NewOrderNotificationReceiver(IEmailRepository emailRepository, ILogger<NewOrderNotificationReceiver> logger)
        {
            _emailRepository = emailRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        [RabbitListener(Queue = "Order.NewOrder-1.0", Concurrency = "1")]
        public async void Receive(string input)
        {
            var notificationData = JsonSerializer.Deserialize<Notification<NewOrderNotificationData>>(input);
            var message = new EmailMessage($"OrderID# {notificationData.Data.OrderId} sent.");
            _emailRepository.SendAsync(message).Wait();
        }
    }
}
