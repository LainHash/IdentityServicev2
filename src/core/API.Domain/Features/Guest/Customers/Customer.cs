using API.Domain.Extensions;
using API.Domain.Features.Identity.Users;
using API.Domain.Common.Entities;
using API.Domain.Features.Guest.CustomerLoyaltyTransactions;

namespace API.Domain.Features.Guest.Customers
{
    public partial class Customer : SoftDeletableEntity
    {
        public string Code { get; private set; } = null!;

        public long UserId { get; private set; }

        public int LoyaltyPoints { get; private set; }

        public User User { get; private set; } = null!;

        public ICollection<CustomerLoyaltyTransaction> LoyaltyTransactions { get; private set; } = [];
    }

    public partial class Customer
    {
        public Customer() { }

        public Customer(long userId, int loyaltyPoints = 0)
        {
            UserId = userId;
            LoyaltyPoints = loyaltyPoints;
        }

        public Customer SetUser(long userId)
        {
            UserId = userId;
            return this;
        }

        public Customer AddLoyaltyPoints(int points)
        {
            if (points <= 0)
                throw new ArgumentOutOfRangeException(nameof(points), "Số điểm cộng phải lớn hơn 0.");

            LoyaltyPoints += points;
            return this;
        }

        public Customer DeductLoyaltyPoints(int points)
        {
            if (points <= 0)
                throw new ArgumentOutOfRangeException(nameof(points), "Số điểm trừ phải lớn hơn 0.");

            if (LoyaltyPoints < points)
                throw new InvalidOperationException("Điểm thưởng hiện tại không đủ để khấu trừ.");

            LoyaltyPoints -= points;
            return this;
        }

        public Customer SetLoyaltyPoints(int points)
        {
            if (points < 0)
                throw new ArgumentOutOfRangeException(nameof(points), "Số điểm không thể âm.");

            LoyaltyPoints = points;
            return this;
        }
    }
}
