namespace Mail.Api.Core.Mail.DTO.Mailbox
{
    public class MailboxModel
    {
        public MailboxCredentialsModel ImapCredentials { get; set; }
        public MailboxCredentialsModel SmtpCredentials { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool UseSSL { get; set; }
    }
}