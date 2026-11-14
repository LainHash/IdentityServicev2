using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using API.Domain.Features.Personnel.Positions;

namespace API.Infrastructure.Features.Personnel.Positions
{
    internal class PositionConfiguration
        : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("Positions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .UseIdentityByDefaultColumn();

            builder.Property(x => x.PublicId)
                .IsRequired();
            builder.HasIndex(x => x.PublicId)
                .IsUnique();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.HasIndex(x => x.Code)
                .IsUnique();

            builder.HasMany(x => x.Employees)
                .WithOne(x => x.Position)
                .HasForeignKey(x => x.PositionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
