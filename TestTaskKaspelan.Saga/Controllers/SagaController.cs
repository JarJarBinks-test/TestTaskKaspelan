using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TestTaskKaspelan.Saga.Services.Interfaces;
using ContractOrder = TestTaskKaspelan.Common.Contracts.Order;

namespace TestTaskKaspelan.Saga.Controllers
{
    /// <summary>
    /// Order controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiVersion(1.0)]
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="orderService">The order service.</param>
        /// <param name="logger">The logger.</param>
        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        /// <summary>
        /// Creates the specified order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="token">The token.</param>
        /// <returns>
        /// New order.
        /// </returns>
        [MapToApiVersion(1.0)]
        [HttpPost]
        public Task<ContractOrder> Create([FromBody] ContractOrder order, CancellationToken token)
        {
            return _orderService.CreateAsync(order, token);
        }
    }
}
