using System.ComponentModel.DataAnnotations;

namespace API.Contract.Settings
{
    public class EmailSettings
    {
        public const string SectionName = "Email";

        [Required]
        public string SmtpServer { get; init; } = string.Empty;

        [Range(1, 65535)]
        public int SmtpPort { get; init; } = 587;

        [Required]
        public string SenderName { get; init; } = string.Empty;

        [Required]
        [EmailAddress]
        public string SenderEmail { get; init; } = string.Empty;

        [Required]
        public string AppPassword { get; init; } = string.Empty;

        public bool EnableSsl { get; init; } = true;

        public int Timeout { get; init; } = 15000;
    }
}
