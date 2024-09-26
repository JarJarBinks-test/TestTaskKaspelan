using DbOrder = TestTaskKaspelan.Order.DataAccess.Entities.Order;

namespace TestTaskKaspelan.Order.DataAccess.Interfaces
{
    /// <summary>
    /// Interface for access to orders.
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Get the order by id.
        /// </summary>
        /// <param name="orderId">Order id.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>
        /// Order be requested id.
        /// </returns>
        Task<DbOrder> GetAsync(Guid orderId, CancellationToken token);

        /// <summary>
        /// Create order.
        /// </summary>
        /// <param name="order">New order.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>
        /// New order.
        /// </returns>
        Task<DbOrder> CreateAsync(DbOrder order, CancellationToken token);
    }
}
