using API.Domain.Features.Guest.CustomerLoyaltyTransactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Features.Guest.CustomerLoyaltyTransactions
{
    internal class CustomerLoyaltyTransactionConfiguration
        : IEntityTypeConfiguration<CustomerLoyaltyTransaction>
    {
        public void Configure(EntityTypeBuilder<CustomerLoyaltyTransaction> builder)
        {
            builder.ToTable("CustomerLoyaltyTransactions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .UseIdentityByDefaultColumn();

            builder.Property(x => x.PublicId)
                .IsRequired();
            builder.HasIndex(x => x.PublicId)
                .IsUnique();

            builder.Property(x => x.Points)
                .IsRequired();

            builder.Property(x => x.BalanceAfter)
                .IsRequired();

            builder.Property(x => x.TransactionType)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.LoyaltyTransactions)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.CustomerId);
            builder.HasIndex(x => x.CreatedAt);
        }
    }
}
