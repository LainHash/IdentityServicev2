namespace API.Application.Features.Authentication
{
    public interface ICurrentUserService
    {
        Guid? UserPublicId { get; }

        string? IpAddress { get; }
    }
}
