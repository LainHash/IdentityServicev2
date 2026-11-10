namespace API.Domain.Common.Models.Messages
{
    public class EmailMessage
    {
        public string Subject { get; private set; } = string.Empty;
        public string Body { get; private set; } = string.Empty;

        private EmailMessage() { }

        public EmailMessage(string userName, string verificationCode)
        {
            Subject = "Restaurant - Email Verification Code";
            Body = $"Hello {userName},<br/><br/>Your verification code is: <b>{verificationCode}</b><br/>This code will expire in 15 minutes.";
        }

        public static EmailMessage ForCurrentEmailConfirmation(string userName, string verificationCode)
        {
            return new EmailMessage
            {
                Subject = "Restaurant - Current Email Verification Code",
                Body = $"Hello {userName},<br/><br/>We received a request to change your email address. Your verification code is: <b>{verificationCode}</b><br/>If you did not request this, please ignore this email.<br/>This code will expire in 15 minutes."
            };
        }

        public static EmailMessage ForEmailChange(string userName, string verificationCode)
        {
            return new EmailMessage
            {
                Subject = "Confirm your new email address",
                Body = $"Hello {userName},<br/><br/>"
                     + $"We received a request to change your email address.<br/>"
                     + $"Your verification code is: <b>{verificationCode}</b><br/>"
                     + $"This code will expire in 15 minutes.<br/><br/>"
                     + $"If you did not request this change, please ignore this email."
            };
        }

        public static EmailMessage Create(string subject, string body)
        {
            return new EmailMessage
            {
                Subject = subject,
                Body = body
            };
        }

        public static EmailMessage ForContactConfirmation(string fullName, string messageSubject)
        {
            return new EmailMessage
            {
                Subject = $"[Restaurant] Xác nhận tiếp nhận thông tin: {messageSubject}",
                Body = $"<p>Xin chào <b>{fullName}</b>,</p>"
                     + $"<p>Cảm ơn bạn đã liên hệ với nhà hàng chúng tôi. Chúng tôi đã nhận được thông điệp của bạn về chủ đề: <i>{messageSubject}</i>.</p>"
                     + $"<p>Đội ngũ hỗ trợ của chúng tôi sẽ xem xét và phản hồi bạn trong thời gian sớm nhất.</p>"
                     + $"<br/><p>Trân trọng,<br/>Đội ngũ Hỗ trợ Khách hàng Restaurant</p>"
            };
        }


    }
}
