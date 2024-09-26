namespace TestTaskKaspelan.Notification.Services.Interfaces
{
    /// <summary>
    /// New order notification service interface.
    /// </summary>
    public interface INewOrderNotificationReceiver
    {
        /// <summary>
        /// Receives the messages from Order.NewOrder queue for api version 1.0 with maximum concurency is 1.
        /// </summary>
        /// <param name="input">The input.</param>
        public void Receive(string input);
    }
}
