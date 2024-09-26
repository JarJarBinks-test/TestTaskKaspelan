using Microsoft.Extensions.Logging;
using Steeltoe.Messaging;
using Steeltoe.Messaging.RabbitMQ.Core;
using System.Text;
using System.Text.Json;
using TestTaskKaspelan.Common.Constants;
using TestTaskKaspelan.Common.Contracts;
using TestTaskKaspelan.Order.DataAccess.Interfaces;
using TestTaskKaspelan.Order.Services.Exceptions;
using TestTaskKaspelan.Order.Services.Interfaces;
using ContractOrder = TestTaskKaspelan.Common.Contracts.Order;
using DbOrder = TestTaskKaspelan.Order.DataAccess.Entities.Order;

namespace TestTaskKaspelan.Order.Services.Services
{
    /// <summary>
    /// Service for access to order.
    /// <seealso cref="TestTaskKaspelan.Order.Services.Interfaces.IOrderService"/> successor.
    /// </summary>
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IRabbitTemplate _rabbitTemplate;
        private readonly ILogger<OrderService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="rabbitTemplate">The rabbit template.</param>
        /// <param name="logger">The logger.</param>
        public OrderService(IOrderRepository repository, IRabbitTemplate rabbitTemplate, ILogger<OrderService> logger)
        {
            _repository = repository;
            _rabbitTemplate = rabbitTemplate;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<ContractOrder> GetAsync(Guid orderId, CancellationToken token)
        {
            if (orderId == Guid.Empty)
            {
                throw new ArgumentException(nameof(orderId));
            }

            var result = await _repository.GetAsync(orderId, token);
            if (result == null)
            {
                throw new NotFoundException($"Order# '{orderId}' not found.");
            }

            // TODO: Should be rewrited to AutoMapper.
            return new ContractOrder()
            {
                Id = result.Id,
                Details = result.Details,
            };
        }

        /// <inheritdoc />
        public async Task<ContractOrder> CreateAsync(ContractOrder order, CancellationToken token)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            // TODO: Should be rewrited to AutoMapper.
            var dbOrder = new DbOrder()
            {
                Details = order.Details,
            };

            var result = await _repository.CreateAsync(dbOrder, token);

            // TODO: Should be rewrited to AutoMapper.
            var resultOrder = new ContractOrder()
            {
                Id = result.Id,
                Details = order.Details,
            };

            // TODO: I think it should be in SAGA but in requirement - here.
            await SendNotificationAboutNewOrder(resultOrder, token);

            return resultOrder;
        }

        Task SendNotificationAboutNewOrder(ContractOrder order, CancellationToken cancellationToken)
        {
            var payload = new Notification<NewOrderNotificationData>(new NewOrderNotificationData(order.Id), Common.Enums.NotificationType.Email);
            var message = Message.Create(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload)));
            return _rabbitTemplate.SendAsync(RabbitMqNameTemplates.NewOrder, "1.0", message, cancellationToken);
        } 
    }
}
