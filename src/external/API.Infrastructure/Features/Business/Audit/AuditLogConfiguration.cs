using API.Domain.Features.Business.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Features.Business.Audit
{
    internal class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .UseIdentityByDefaultColumn();

            builder.Property(x => x.PublicId)
                .IsRequired();
            builder.HasIndex(x => x.PublicId)
                .IsUnique();

            builder.Property(x => x.Action)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.EntityName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.EntityId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.OldValues)
                .HasColumnType("jsonb");

            builder.Property(x => x.NewValues)
                .HasColumnType("jsonb");

            builder.Property(x => x.IpAddress)
                .HasMaxLength(45);

            builder.Property(x => x.Timestamp)
                .IsRequired();

            builder.HasIndex(x => x.EntityName);
            builder.HasIndex(x => x.UserPublicId);
            builder.HasIndex(x => x.Timestamp);
            builder.HasIndex(x => new { x.EntityName, x.EntityId });
        }
    }
}
