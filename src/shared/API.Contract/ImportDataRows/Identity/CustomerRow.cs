namespace API.Contract.ImportDataRows.Identity
{
    public class CustomerRow
    {
        public Guid PublicId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public int LoyaltyPoints { get; set; }
    }
}

