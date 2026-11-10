using System.Net;
using API.Application.Features.Business.Email;
using API.Contract.DTOs.Business.Emails;
using API.Domain.Common.Models.Messages;
using API.Domain.Common.Models.Results;
using IdentityAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers.Communication
{
    [Route("api/identity/[controller]")]
    [ApiController]
    public class EmailsController(
        IEmailService emailService,
        ILogger<EmailsController> logger) : ControllerBase
    {
        private readonly IEmailService _emailService = emailService;
        private readonly ILogger<EmailsController> _logger = logger;

        [AllowAnonymous]
        [HttpPost("contact-confirmation")]
        public async Task<IActionResult> SendContactConfirmation(
            [FromBody] SendContactConfirmationEmailRequest request,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.ToEmail))
            {
                return this.ToActionResult(Result<bool>.Fail("Recipient email is required.", HttpStatusCode.BadRequest));
            }

            var recipientName = string.IsNullOrWhiteSpace(request.FullName) ? "Khách hàng" : request.FullName.Trim();
            var emailMessage = EmailMessage.ForContactConfirmation(recipientName, request.Subject);

            _logger.LogInformation("Received request to send contact confirmation email to {Email}", request.ToEmail);
            await _emailService.SendEmailAsync(request.ToEmail.Trim(), emailMessage, cancellationToken);

            return this.ToActionResult(Result<bool>.Succeed(true, "Contact confirmation email has been queued successfully."));
        }

        [AllowAnonymous]
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail(
            [FromBody] SendEmailRequest request,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.ToEmail))
            {
                return this.ToActionResult(Result<bool>.Fail("Recipient email is required.", HttpStatusCode.BadRequest));
            }

            var emailMessage = EmailMessage.Create(request.Subject, request.Body);
            _logger.LogInformation("Received request to send custom email to {Email}", request.ToEmail);
            await _emailService.SendEmailAsync(request.ToEmail.Trim(), emailMessage, cancellationToken);

            return this.ToActionResult(Result<bool>.Succeed(true, "Email has been queued successfully."));
        }
    }
}

