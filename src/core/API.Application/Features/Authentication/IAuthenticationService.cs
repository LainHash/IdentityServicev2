using API.Application.Features.Authentication.Commands.GoogleLogin;
using API.Application.Features.Authentication.Commands.Login;
using API.Application.Features.Authentication.Commands.Logout;
using API.Application.Features.Authentication.Commands.RefreshToken;
using API.Application.Features.Authentication.Commands.Register;
using API.Application.Features.Identity.Users.Commands.ChangePassword;
using API.Contract.DTOs.Authentication;
using API.Domain.Common.Models.Results;
namespace API.Application.Features.Authentication
{
    public interface IAuthenticationService
    {
        Task<APIResponse<AuthenticationResponse>> LoginAsync(
            LoginCommand command,
            CancellationToken cancellationToken = default);

        Task<APIResponse<AuthenticationResponse>> GoogleLoginAsync(
            GoogleLoginCommand command,
            CancellationToken cancellationToken = default);

        Task<APIResponse> RegisterAsync(
            RegisterCommand command,
            CancellationToken cancellationToken = default);

        Task<APIResponse<AuthenticationResponse>> RefreshTokenAsync(
            RefreshTokenCommand command,
            CancellationToken cancellationToken = default);

        Task<APIResponse> LogoutAsync(
            LogoutCommand command,
            CancellationToken cancellationToken = default);

        Task<APIResponse> LogoutAllAsync(
            LogoutAllCommand command,
            CancellationToken cancellationToken = default);
    }
}
