namespace TestTaskKaspelan.Notification.Repositories.Entities
{
    public class EmailMessage
    {
        public string Message { get; set; }
        public bool IsHtml { get; set; }

        public EmailMessage(string message, bool isHtml = true) {
            Message = message;
            IsHtml = isHtml;
            // TODO: Need implement required fields
        }
    }
}
