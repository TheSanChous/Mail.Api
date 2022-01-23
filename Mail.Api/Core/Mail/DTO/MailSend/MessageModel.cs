using System.Collections;
using System.Collections.Generic;

namespace Mail.Api.Core.Mail.DTO.MailSend
{
    public class MessageSendModel
    {
        public ICollection<string> To { get; set; }
        public ICollection<string> CC { get; set; }
        public ICollection<string> BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
    }
}