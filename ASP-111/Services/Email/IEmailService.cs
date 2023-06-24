namespace ASP_111.Services.Email
{
    public interface IEmailService
    {
        void Send(string email, string message, string subject);
    }
}