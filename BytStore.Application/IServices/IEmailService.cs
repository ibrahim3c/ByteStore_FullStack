using Microsoft.AspNetCore.Http;

namespace BytStore.Application.IServices
{
    public interface IEmailService
    {
        Task SendMailByBrevoAsync(string mailTo, string subject, string body, IList<IFormFile>? files = null);
    }
}
