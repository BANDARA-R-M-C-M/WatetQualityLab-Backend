using SendGrid.Helpers.Mail;
using SendGrid;

namespace Project_v1.Services.EmailService {
    public class EmailService {
        private readonly string _sendGridApiKey;

        public EmailService(IConfiguration configuration) {
            _sendGridApiKey = configuration["SendGrid:ApiKey"];
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message) {
            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress("no-reply@yourdomain.com", "Your App Name");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
