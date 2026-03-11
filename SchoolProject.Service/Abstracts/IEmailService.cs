namespace SchoolProject.Service.Abstracts
{
    public interface IEmailService
    {
        public Task<string> SendEmailAsync(string Email, string Message, string? reason);
    }
}
