namespace TestTaskKaspelan.Common.Contracts
{
    /// <summary>
    /// New order notification data.
    /// </summary>
    /// <seealso cref="TestTaskKaspelan.Common.Contracts.BaseNotificationData" />
    public class NewOrderNotificationData: BaseNotificationData
    {
        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewOrderNotificationData"/> class.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        public NewOrderNotificationData(Guid orderId) {
            OrderId = orderId;
        }
    }
}
