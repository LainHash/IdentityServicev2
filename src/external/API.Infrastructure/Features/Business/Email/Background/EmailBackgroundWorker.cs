using API.Contract.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace API.Infrastructure.Features.Business.Email.Background
{
    internal class EmailBackgroundWorker : BackgroundService
    {
        private readonly IEmailQueue _queue;
        private readonly IOptions<AppSettings> _options;
        private readonly ILogger<EmailBackgroundWorker> _logger;

        public EmailBackgroundWorker(
            IEmailQueue queue,
            IOptions<AppSettings> options,
            ILogger<EmailBackgroundWorker> logger)
        {
            _queue = queue;
            _options = options;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("EmailBackgroundWorker has started.");

            await foreach (var item in _queue.ReadAllAsync(stoppingToken))
            {
                await SendEmailWithRetryAsync(item, stoppingToken);
            }

            _logger.LogInformation("EmailBackgroundWorker has stopped.");
        }

        private async Task SendEmailWithRetryAsync(EmailWorkItem item, CancellationToken cancellationToken)
        {
            var settings = _options.Value.Email;
            const int maxRetries = 3;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    using var client = new SmtpClient(settings.SmtpServer, settings.SmtpPort)
                    {
                        Credentials = new NetworkCredential(settings.SenderEmail, settings.AppPassword),
                        EnableSsl = settings.EnableSsl,
                        Timeout = settings.Timeout
                    };

                    using var mail = new MailMessage
                    {
                        From = new MailAddress(settings.SenderEmail, settings.SenderName),
                        Subject = item.Message.Subject,
                        Body = item.Message.Body,
                        IsBodyHtml = true
                    };
                    mail.To.Add(item.To);

                    await client.SendMailAsync(mail, cancellationToken);
                    _logger.LogInformation("Background email successfully sent to {Email}", item.To);
                    return;
                }
                catch (Exception ex) when (attempt < maxRetries && !cancellationToken.IsCancellationRequested)
                {
                    _logger.LogWarning(ex, "Attempt {Attempt} failed to send email to {Email}. Retrying in {Seconds}s...", attempt, item.To, attempt * 2);
                    await Task.Delay(TimeSpan.FromSeconds(attempt * 2), cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send email to {Email} after attempt {Attempt}", item.To, attempt);
                }
            }
        }
    }
}
