using API.Domain.Features.Business.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Features.Business.Configurations
{
    internal class SystemSettingConfiguration : IEntityTypeConfiguration<SystemSetting>
    {
        public void Configure(EntityTypeBuilder<SystemSetting> builder)
        {
            builder.ToTable("SystemSettings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .UseIdentityByDefaultColumn();

            builder.Property(x => x.PublicId)
                .IsRequired();

            builder.Property(x => x.Key)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Value)
                .IsRequired();

            builder.Property(x => x.DefaultValue);

            builder.Property(x => x.DataType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.Group)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.IsPublic)
                .HasDefaultValue(false);

            builder.Property(x => x.IsSystem)
                .HasDefaultValue(false);

            builder.Property(x => x.IsEncrypted)
                .HasDefaultValue(false);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            // ── Indexes ──────────────────────────────────────────────
            builder.HasIndex(x => x.Key)
                .IsUnique();

            builder.HasIndex(x => x.Group);
            builder.HasIndex(x => x.IsPublic);
        }
    }
}
