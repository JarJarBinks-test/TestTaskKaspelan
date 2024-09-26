using Microsoft.Extensions.Logging;
using TestTaskKaspelan.Order.DataAccess.Contexts;
using TestTaskKaspelan.Order.DataAccess.Interfaces;
using DbOrder = TestTaskKaspelan.Order.DataAccess.Entities.Order;

namespace TestTaskKaspelan.Order.DataAccess.Repositories
{
    /// <summary>
    /// Repository for acces to order.
    /// <seealso cref="TestTaskKaspelan.Order.DataAccess.Interfaces.IOrderRepository"/> successor.
    /// </summary>
    public class OrderRepository: IOrderRepository
    {
        private readonly OrderContext _context;
        private readonly ILogger<OrderRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        public OrderRepository(OrderContext context, ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<DbOrder> GetAsync(Guid orderId, CancellationToken token)
        {
            if (orderId == Guid.Empty)
            {
                throw new ArgumentException(nameof(orderId));
            }

            return await _context.Orders.FindAsync([orderId], token);
        }

        /// <inheritdoc />
        public async Task<DbOrder> CreateAsync(DbOrder order, CancellationToken token)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            var result = await _context.Orders.AddAsync(order, token);
            await _context.SaveChangesAsync(token);
            return order;
        }
    }
}
