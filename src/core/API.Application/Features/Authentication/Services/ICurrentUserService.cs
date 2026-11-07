namespace API.Application.Features.Authentication.Services
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }

        string? IpAddress { get; }
    }
}
