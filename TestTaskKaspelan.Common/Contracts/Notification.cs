using TestTaskKaspelan.Common.Enums;

namespace TestTaskKaspelan.Common.Contracts
{
    /// <summary>
    /// Notification.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Notification<T> where T: BaseNotificationData
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public NotificationType Type { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public T Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Notification{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="type">The type.</param>
        public Notification(T data, NotificationType type)
        {
            Data = data;
            Type = type;
        }
    }
}
