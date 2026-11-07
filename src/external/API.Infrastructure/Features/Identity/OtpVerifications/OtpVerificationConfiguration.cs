using API.Domain.Entities.Identity.OtpVerifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Features.Identity.OtpVerifications
{
    internal class OtpVerificationConfiguration
        : IEntityTypeConfiguration<OtpVerification>
    {
        public void Configure(EntityTypeBuilder<OtpVerification> builder)
        {
            builder.ToTable("OtpVerifications");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .UseIdentityByDefaultColumn();

            builder.Property(x => x.PublicId)
                .IsRequired();
            builder.HasIndex(x => x.PublicId)
                .IsUnique();

            builder.Property(x => x.IsAvailable)
                .HasDefaultValue(true);

            builder.Property(x => x.Purpose)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.FailedAttempts)
                .HasDefaultValue(0);

            builder.HasOne(x => x.User)
                .WithMany(x => x.OtpVerifications)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
