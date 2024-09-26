using ContractOrder = TestTaskKaspelan.Common.Contracts.Order;

namespace TestTaskKaspelan.Order.Services.Interfaces
{
    /// <summary>
    /// Interface for access to order.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Get order by id.
        /// </summary>
        /// <param name="orderId">Order identificator.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>
        /// Result order.
        /// </returns>
        Task<ContractOrder> GetAsync(Guid orderId, CancellationToken token);

        /// <summary>
        /// Create order.
        /// </summary>
        /// <param name="order">Order for create.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>
        /// New order.
        /// </returns>
        Task<ContractOrder> CreateAsync(ContractOrder order, CancellationToken token);
    }
}
