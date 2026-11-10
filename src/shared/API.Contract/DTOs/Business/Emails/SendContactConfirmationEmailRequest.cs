namespace API.Contract.DTOs.Business.Emails
{
    public class SendContactConfirmationEmailRequest
    {
        public string ToEmail { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
    }
}

