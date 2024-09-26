using Microsoft.Extensions.Logging;
using TestTaskKaspelan.Notification.Repositories.Entities;
using TestTaskKaspelan.Notification.Repositories.Interfaces;

namespace TestTaskKaspelan.Notification.Repositories.Repositories
{
    /// <summary>
    /// Send email reporitory.
    /// </summary>
    /// <seealso cref="TestTaskKaspelan.Notification.Repositories.Interfaces.IEmailRepository" />
    public class EmailRepository: IEmailRepository
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public EmailRepository(ILogger<EmailRepository> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public Task SendAsync(EmailMessage message)
        {
            // Should be prepopuplated email settings from global config like rabitmq and call send email provider service.
            _logger.LogInformation($"Sent with message: {message.Message}");

            return Task.CompletedTask;
        }
    }
}
