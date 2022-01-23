using Mail.Api.Core.Mail.DTO.MailSend;
using Mail.Api.Core.Mail.Models;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mail.Api.Core.Mail.Interfaces
{
    public interface IMailService
    {
        public Task SendMailAsyc(MailSendModel mailbox, CancellationToken cancellationToken);
        public Task<IQueryable<MimeMessage>> GetMailAsync(Mailbox mailbox, TimeSpan interval, CancellationToken cancellationToken);
        public void SaveAttachments(IEnumerable<MimeEntity> mimeEntities, string path = "files");
    }
}
