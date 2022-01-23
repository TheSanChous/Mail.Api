using Mail.Api.Core.Mail.DTO.Mailbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mail.Api.Core.Mail.DTO.MailSend
{
    public class MailSendModel
    {
        public MailboxModel mailbox { get; set; }
        public MessageSendModel message { get; set; }
    }
}
