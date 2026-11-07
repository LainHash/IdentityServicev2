using API.Application.Features.Business.Email;
using API.Domain.Common.Models.Messages;
using API.Infrastructure.Features.Business.Email.Background;
using Microsoft.Extensions.Logging;

namespace API.Infrastructure.Features.Business.Email
{
    internal class EmailService : IEmailService
    {
        private readonly IEmailQueue _emailQueue;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IEmailQueue emailQueue,
            ILogger<EmailService> logger)
        {
            _emailQueue = emailQueue;
            _logger = logger;
        }

        public async Task SendEmailAsync(
            string to,
            EmailMessage message,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Enqueuing email for {Email} into background processing queue", to);
            await _emailQueue.QueueBackgroundEmailAsync(to, message);
        }
    }
}
