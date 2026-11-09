using API.Domain.Features.Guest.Customers;
using API.Domain.Features.Guest.CustomerLoyaltyTransactions.Enums;
using API.Domain.Common.Entities;

namespace API.Domain.Features.Guest.CustomerLoyaltyTransactions
{
    public class CustomerLoyaltyTransaction : Entity
    {
        public long CustomerId { get; private set; }
        public Customer Customer { get; private set; } = null!;

        public int Points { get; private set; }
        public int BalanceAfter { get; private set; }
        public LoyaltyTransactionType TransactionType { get; private set; }

        public Guid? OrderPublicId { get; private set; }

        public string? Description { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private CustomerLoyaltyTransaction() { }

        public CustomerLoyaltyTransaction(
            long customerId,
            int points,
            int balanceAfter,
            LoyaltyTransactionType transactionType,
            Guid? orderId = null,
            string? description = null)
        {
            CustomerId = customerId;
            Points = points;
            BalanceAfter = balanceAfter;
            TransactionType = transactionType;
            OrderPublicId = orderId;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }

        public static CustomerLoyaltyTransaction Create(
            long customerId,
            int points,
            int balanceAfter,
            LoyaltyTransactionType transactionType,
            Guid? orderId = null,
            string? description = null)
        {
            return new CustomerLoyaltyTransaction(customerId, points, balanceAfter, transactionType, orderId, description);
        }
    }
}
