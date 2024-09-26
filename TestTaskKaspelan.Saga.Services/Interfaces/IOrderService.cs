using ContractOrder = TestTaskKaspelan.Common.Contracts.Order;

namespace TestTaskKaspelan.Saga.Services.Interfaces
{
    /// <summary>
    /// Interface for access to order.
    /// </summary>
    public interface IOrderService
    {
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
