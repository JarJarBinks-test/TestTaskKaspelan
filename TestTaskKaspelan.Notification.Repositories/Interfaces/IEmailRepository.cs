using TestTaskKaspelan.Notification.Repositories.Entities;

namespace TestTaskKaspelan.Notification.Repositories.Interfaces
{
    /// <summary>
    /// Send email repository interface.
    /// </summary>
    public interface IEmailRepository
    {
        /// <summary>
        /// Sends the email message.
        /// </summary>
        /// <param name="message">The message data.</param>
        Task SendAsync(EmailMessage message);
    }
}
