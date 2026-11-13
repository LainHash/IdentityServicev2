using API.Domain.Features.Identity.PersonalProfiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Features.Identity.PersonalProfiles
{
    internal class PersonalProfileConfiguration
        : IEntityTypeConfiguration<PersonalProfile>
    {
        public void Configure(EntityTypeBuilder<PersonalProfile> builder)
        {
            builder.ToTable("PersonalProfiles");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .UseIdentityByDefaultColumn();

            builder.Property(x => x.PublicId)
                .IsRequired();
            builder.HasIndex(x => x.PublicId)
                .IsUnique();

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.DateOfBirth)
                .IsRequired();

            builder.Property(x => x.Gender)
                .IsRequired();

            builder.OwnsOne(x => x.Address, addressBuilder =>
            {
                addressBuilder.Property(a => a.Street)
                    .HasColumnName("Address_Street")
                    .HasMaxLength(200)
                    .IsRequired();

                addressBuilder.Property(a => a.Ward)
                    .HasColumnName("Address_Ward")
                    .HasMaxLength(100);

                addressBuilder.Property(a => a.District)
                    .HasColumnName("Address_District")
                    .HasMaxLength(100);

                addressBuilder.Property(a => a.Province)
                    .HasColumnName("Address_Province")
                    .HasMaxLength(100)
                    .IsRequired();

                addressBuilder.Property(a => a.Country)
                    .HasColumnName("Address_Country")
                    .HasMaxLength(50)
                    .HasDefaultValue("Vietnam")
                    .IsRequired();

                addressBuilder.Property(a => a.FullAddress)
                    .HasColumnName("Address_FullAddress")
                    .HasMaxLength(500);

                addressBuilder.Property(a => a.Latitude)
                    .HasColumnName("Address_Latitude")
                    .HasPrecision(9, 6);

                addressBuilder.Property(a => a.Longitude)
                    .HasColumnName("Address_Longitude")
                    .HasPrecision(9, 6);

                addressBuilder.Property(a => a.AdministrativeDivisionPublicId)
                    .HasColumnName("Address_AdministrativeDivisionPublicId");
            });

            builder.Property(x => x.Phone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.CitizenCardId)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(x => x.UserId)
                .IsUnique();

            builder.HasOne(x => x.User)
                .WithOne(x => x.PersonalProfile)
                .HasForeignKey<PersonalProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
