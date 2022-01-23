using Mail.Api.Core.Mail.DTO.Mailbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mail.Api.Core.Mail.DTO.MailGet
{
    public class MailGetModel
    {
        public MailboxModel mailbox { get; set; }
        public int interval { get; set; }
    }
}
