namespace API.Contract.DTOs.Guest.Customers
{
    public class CustomerResponse : SoftDeletableResponse
    {
        public string Code { get; set; } = string.Empty;
        public int LoyaltyPoints { get; set; }
    }
}
