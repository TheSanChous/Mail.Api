using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mail.Api.Core.Mail.Models
{
    public class Mailbox
    {
        public MailCredentials ImapCredentials { get; set; }
        public MailCredentials SmtpCredentials { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }
    }
}
