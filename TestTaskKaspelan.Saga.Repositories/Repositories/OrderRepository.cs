using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using TestTaskKaspelan.Common.Constants;
using TestTaskKaspelan.Saga.Repositories.Interfaces;
using ContractOrder = TestTaskKaspelan.Common.Contracts.Order;

namespace TestTaskKaspelan.Saga.Repositories.Repositories
{
    /// <summary>
    /// Repository for acces to order.
    /// </summary>
    /// <seealso cref="TestTaskKaspelan.Saga.Repositories.Interfaces.IOrderRepository" />
    public class OrderRepository: IOrderRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OrderRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderRepository"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="logger">The logger.</param>
        public OrderRepository(HttpClient httpClient, ILogger<OrderRepository> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<ContractOrder> CreateAsync(ContractOrder order, CancellationToken token)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            var result = await _httpClient.PostAsJsonAsync($"http://{ServicesNames.Order}/api/v1/order", order, token);
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadFromJsonAsync<ContractOrder>(token);
        }
    }
}
