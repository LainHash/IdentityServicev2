using System.Text.Json.Serialization;

namespace API.Domain.Features.Guest.CustomerLoyaltyTransactions.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LoyaltyTransactionType
    {
        GoogleRegistrationReward = 1,
        OrderReward = 2,
        OrderCancelled = 3,
        AdminAdjustment = 4
    }
}
