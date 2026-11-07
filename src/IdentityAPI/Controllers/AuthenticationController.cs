using API.Application.Features.Authentication.Commands.GoogleLogin;
using API.Application.Features.Authentication.Commands.Login;
using API.Application.Features.Authentication.Commands.Logout;
using API.Application.Features.Authentication.Commands.RefreshToken;
using API.Application.Features.Authentication.Commands.Register;
using API.Application.Features.Authentication.Services;
using API.Application.Features.Identity.OtpVerifications.Commands.VerifyOtp;
using API.Contract.DTOs.Authentication;
using API.Contract.DTOs.Identity.OtpVerifications;
using API.Contract.Settings;
using IdentityAPI.Extensions;
using IdentityAPI.Settings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace IdentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(
        IMediator mediator,
        ICurrentUserService currentUserService,
        IOptions<AuthCookieSettings> cookieOptions,
        IOptions<AppSettings> appOptions) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly AuthCookieSettings _cookieSettings = cookieOptions.Value;
        private readonly JwtSettings _jwtSettings = appOptions.Value.Jwt;

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest body,
            CancellationToken cancellationToken = default)
        {
            var command = new LoginCommand(body);
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsSucceed && result.Data is not null)
            {
                Response.SetAuthCookies(result.Data, _cookieSettings, _jwtSettings);
            }

            return this.ToActionResult(result);
        }

        [AllowAnonymous]
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(
            [FromBody] GoogleLoginRequest body,
            CancellationToken cancellationToken = default)
        {
            var command = new GoogleLoginCommand(body);
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsSucceed && result.Data is not null)
            {
                Response.SetAuthCookies(result.Data, _cookieSettings, _jwtSettings);
            }

            return this.ToActionResult(result);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequest body,
            CancellationToken cancellationToken = default)
        {
            var command = new RegisterCommand(body);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [AllowAnonymous]
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(
            [FromBody] VerifyOtpRequest body,
            CancellationToken cancellationToken = default)
        {
            var command = new VerifyOtpCommand(body);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(
            [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] RefreshTokenRequest? body,
            CancellationToken cancellationToken)
        {
            var refreshToken = ResolveRefreshToken(body);
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                Response.ClearAuthCookies(_cookieSettings);
                return BadRequest("Refresh token is missing.");
            }

            var command = new LogoutCommand(refreshToken);
            var result = await _mediator.Send(command, cancellationToken);

            Response.ClearAuthCookies(_cookieSettings);
            return this.ToActionResult(result);
        }

        [Authorize]
        [HttpPost("logout-all")]
        public async Task<IActionResult> LogoutAll(
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId is null)
            {
                return Unauthorized();
            }

            var command = new LogoutAllCommand(userId.Value);
            var result = await _mediator.Send(command, cancellationToken);

            Response.ClearAuthCookies(_cookieSettings);
            return this.ToActionResult(result);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(
            [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] RefreshTokenRequest? body,
            CancellationToken cancellationToken)
        {
            var refreshToken = ResolveRefreshToken(body);
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return Unauthorized("Refresh token is missing.");
            }

            var command = new RefreshTokenCommand(new RefreshTokenRequest(refreshToken));
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsSucceed && result.Data is not null)
            {
                Response.SetAuthCookies(result.Data, _cookieSettings, _jwtSettings);
            }
            else
            {
                Response.ClearAuthCookies(_cookieSettings);
            }

            return this.ToActionResult(result);
        }

        private string? ResolveRefreshToken(RefreshTokenRequest? body)
        {
            if (!string.IsNullOrWhiteSpace(body?.RefreshToken))
            {
                return body.RefreshToken;
            }

            return Request.Cookies.TryGetValue(
                _cookieSettings.RefreshTokenCookieName,
                out var cookieToken)
                ? cookieToken
                : null;
        }
    }
}
