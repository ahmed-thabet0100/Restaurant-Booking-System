namespace Restaurant.Service.Abstracts
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string ToEmail, string Subject, string Message);
    }
}
