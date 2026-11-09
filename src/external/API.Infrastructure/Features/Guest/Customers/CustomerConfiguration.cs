using API.Domain.Common.Helpers;
using API.Domain.Features.Guest.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Features.Guest.Customers
{
    internal class CustomerConfiguration
        : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .UseIdentityByDefaultColumn();

            builder.Property(x => x.PublicId)
                .IsRequired();
            builder.HasIndex(x => x.PublicId)
                .IsUnique();

            builder.Property(x => x.Code)
                .HasValueGenerator<CodeValueGenerator>()
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.HasIndex(x => x.Code)
                .IsUnique();

            builder.Property(x => x.LoyaltyPoints)
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasIndex(x => x.UserId)
                .IsUnique();

            builder.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<Customer>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.LoyaltyTransactions)
                .WithOne(x => x.Customer)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
