using API.Domain.Features.Identity.Users;

namespace API.Contract.DTOs.Authentication
{
    public class AuthenticationResponse
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;

        public AuthenticationResponse() { }

        public AuthenticationResponse(
            Guid userId,
            string userName,
            string email,
            string accessToken,
            string refreshToken)
        {
            UserId = userId;
            UserName = userName;
            Email = email;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public AuthenticationResponse(
            User user,
            string accessToken,
            string refreshToken)
            : this(
                  user.PublicId,
                  user.UserName,
                  user.Email,
                  accessToken,
                  refreshToken)
        {

        }
    }
}
