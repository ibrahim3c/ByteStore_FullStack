using brevo_csharp.Api;
using brevo_csharp.Model;
using BytStore.Application.Helpers;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ByteStore.Persistance.Services
{
    internal class EmailService : IEmailService
    {
        private readonly Brevo _brevo;
        private readonly TransactionalEmailsApi _apiInstance;

        public EmailService(IOptions<Brevo> brevo)
        {
            _brevo = brevo.Value;

            // Validate required configuration
            if (string.IsNullOrEmpty(_brevo.ApiKey))
                throw new ArgumentNullException(nameof(_brevo.ApiKey), "Brevo API Key is not configured.");

            if (string.IsNullOrEmpty(_brevo.SenderEmail))
                throw new ArgumentNullException(nameof(_brevo.SenderEmail), "Brevo Sender Email is not configured.");

            // Configure Brevo API client
            var config = new brevo_csharp.Client.Configuration();
            config.ApiKey.Add("api-key", _brevo.ApiKey);

            _apiInstance = new TransactionalEmailsApi(config);
        }

        public async System.Threading.Tasks.Task SendMailByBrevoAsync(string mailTo, string subject, string body, IList<IFormFile>? files = null)
        {
            try
            {
                // 1. Create the Sender using configured values
                var sender = new SendSmtpEmailSender(
                    _brevo.SenderName ?? "BytStore", // Fallback if SenderName is null
                    _brevo.SenderEmail
                );

                // 2. Create the list of receivers
                var to = new List<SendSmtpEmailTo> { new SendSmtpEmailTo(mailTo) };

                // 3. Handle attachments if any files are provided
                var attachments = new List<SendSmtpEmailAttachment>();

                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            using var memoryStream = new MemoryStream();
                            await file.CopyToAsync(memoryStream);
                            var fileBytes = memoryStream.ToArray();
                            //var content = Convert.ToBase64String(fileBytes);

                            attachments.Add(new SendSmtpEmailAttachment(
                                name: file.FileName,
                                content: fileBytes
                            ));
                        }
                    }
                }

                // 4. Create the email object for Brevo API
                var sendSmtpEmail = new SendSmtpEmail(
                    sender: sender,
                    to: to,
                    htmlContent: body,
                    subject: subject,
                    attachment: attachments
                );

                // 5. Send the email
                var result = await _apiInstance.SendTransacEmailAsync(sendSmtpEmail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email to {mailTo}: {ex.Message}");
                throw;
            }
        }
    }
}