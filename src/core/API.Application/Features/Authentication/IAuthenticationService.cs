using API.Application.Features.Authentication.Commands.GoogleLogin;
using API.Application.Features.Authentication.Commands.Login;
using API.Application.Features.Authentication.Commands.Logout;
using API.Application.Features.Authentication.Commands.RefreshToken;
using API.Application.Features.Authentication.Commands.Register;
using API.Contract.DTOs.Authentication;
using API.Domain.Common.Models.Results;
namespace API.Application.Features.Authentication
{
    public interface IAuthenticationService
    {
        Task<Result<AuthenticationResponse>> LoginAsync(
            LoginCommand command,
            CancellationToken cancellationToken = default);

        Task<Result<AuthenticationResponse>> GoogleLoginAsync(
            GoogleLoginCommand command,
            CancellationToken cancellationToken = default);

        Task<Result> RegisterAsync(
            RegisterCommand command,
            CancellationToken cancellationToken = default);

        Task<Result<AuthenticationResponse>> RefreshTokenAsync(
            RefreshTokenCommand command,
            CancellationToken cancellationToken = default);

        Task<Result> LogoutAsync(
            LogoutCommand command,
            CancellationToken cancellationToken = default);

        Task<Result> LogoutAllAsync(
            LogoutAllCommand command,
            CancellationToken cancellationToken = default);
    }
}
