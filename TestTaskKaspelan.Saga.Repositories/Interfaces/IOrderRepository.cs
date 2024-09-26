using ContractOrder = TestTaskKaspelan.Common.Contracts.Order;

namespace TestTaskKaspelan.Saga.Repositories.Interfaces
{
    /// <summary>
    /// Interface for access to orders.
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Create order.
        /// </summary>
        /// <param name="order">New order.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>
        /// New order.
        /// </returns>
        Task<ContractOrder> CreateAsync(ContractOrder order, CancellationToken token);
    }
}
