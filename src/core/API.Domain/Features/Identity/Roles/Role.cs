using API.Domain.Common.Entities;
using API.Domain.Features.Identity.Users;

namespace API.Domain.Features.Identity.Roles
{
    public partial class Role : SoftDeletableEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }

        public ICollection<User> Users { get; private set; } = [];
    }

    public partial class Role
    {
        private Role() { }

        public Role(string name, string? description = null)
        {
            Name = name;
            Description = description;
        }

        public static Role Create(string name, string? description = null)
        {
            return new Role(name.Trim(), description?.Trim());
        }

        public Role UpdateDetails(string name, string? description = null)
        {
            Name = name.Trim();
            Description = description?.Trim();
            return this;
        }
    }
}
