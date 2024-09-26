using Microsoft.Extensions.Logging;
using TestTaskKaspelan.Saga.Repositories.Interfaces;
using TestTaskKaspelan.Saga.Services.Interfaces;
using ContractOrder = TestTaskKaspelan.Common.Contracts.Order;

namespace TestTaskKaspelan.Saga.Services.Services
{
    /// <summary>
    /// Service for access to order.
    /// <seealso cref="TestTaskKaspelan.Order.Services.Interfaces.IOrderService"/> successor.
    /// </summary>
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<OrderService> _logger;

        /// <summary>
        /// Constructore of order service.
        /// </summary>
        /// <param name="repository">Repository for access to order.</param>
        /// <param name="logger">Order service logger.</param>
        public OrderService(IOrderRepository repository, ILogger<OrderService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<ContractOrder> CreateAsync(ContractOrder order, CancellationToken token)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            ContractOrder resultOrder = null;
            try
            {
                resultOrder = await _repository.CreateAsync(order, token);
            }
            catch (Exception ex)
            {
                // TODO: Need implementation compensation actions.
                throw;
            }
            
            // TODO: Need send notification here, but in requirement i should send in Order service.

            return resultOrder;
        }
    }
}
