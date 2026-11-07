using API.Domain.Common.Models.Messages;

namespace API.Application.Features.Business.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(
            string to,
            EmailMessage message,
            CancellationToken cancellationToken = default);
    }
}
