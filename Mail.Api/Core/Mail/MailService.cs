using Mail.Api.Core.Mail.Interfaces;
using Mail.Api.Core.Mail.Models;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MimeKit;
using MailKit;
using System.Net.Mail;
using Mail.Api.Core.Mail.DTO.MailSend;

namespace Mail.Api.Core.Mail
{
    public class MailService : Interfaces.IMailService
    {
        private readonly ImapClient imapClient;

        private readonly MailKit.Net.Smtp.SmtpClient smtpClient;

        public MailService()
        {
            imapClient = new ImapClient();
            smtpClient = new MailKit.Net.Smtp.SmtpClient();
        }

        public async Task<IQueryable<MimeMessage>> GetMailAsync(Mailbox mailbox, TimeSpan interval, CancellationToken cancellationToken)
        {
            await imapClient.ConnectAsync(mailbox.ImapCredentials.Host, mailbox.ImapCredentials.Port, mailbox.UseSsl, cancellationToken);

            await imapClient.AuthenticateAsync(mailbox.Login, mailbox.Password, cancellationToken);

            imapClient.Inbox.Open(FolderAccess.ReadOnly);

            var date = DateTimeOffset.Now - interval;

            var mail = imapClient.Inbox
                .AsQueryable()
                .Where(email => email.Date.ToUniversalTime() > date.ToUniversalTime());

            return mail;
        }

        public void SaveAttachments(IEnumerable<MimeEntity> mimeEntities, string path = "files")
        {
            foreach (var attachment in mimeEntities)
            {
                var fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;

                using (var stream = File.Create($"{path}\\" + Guid.NewGuid().ToString() + fileName))
                {
                    if (attachment is MessagePart)
                    {
                        var rfc822 = (MessagePart)attachment;

                        rfc822.Message.WriteTo(stream);
                    }
                    else
                    {
                        var part = (MimePart)attachment;

                        part.Content.DecodeTo(stream);
                    }
                }
            }
        }

        public async Task SendMailAsyc(MailSendModel mailSend, CancellationToken cancellationToken)
        {
            var mailbox = new Mailbox
            {
                ImapCredentials = new MailCredentials
                {
                    Host = mailSend.mailbox.ImapCredentials.Host,
                    Port = mailSend.mailbox.ImapCredentials.Port
                },
                SmtpCredentials = new MailCredentials
                {
                    Host = mailSend.mailbox.SmtpCredentials.Host,
                    Port = mailSend.mailbox.SmtpCredentials.Port
                },
                Login = mailSend.mailbox.Login,
                Password = mailSend.mailbox.Password,
                UseSsl = mailSend.mailbox.UseSSL,
            };

            await smtpClient.ConnectAsync(mailbox.SmtpCredentials.Host,
                mailbox.SmtpCredentials.Port,
                mailbox.UseSsl,
                cancellationToken);

            await smtpClient.AuthenticateAsync(mailbox.Login, mailbox.Password, cancellationToken);
            
            var to = mailSend.message.To
                .Select(item => InternetAddress.Parse(item));

            var message = new MimeMessage();

            message.From.Add(InternetAddress.Parse(mailbox.Login));

            message.To.AddRange(to);
            message.Cc.AddRange(mailSend.message.CC.Select(item => InternetAddress.Parse(item)));
            message.Bcc.AddRange(mailSend.message.BCC.Select(item => InternetAddress.Parse(item)));

            message.Body = new TextPart(mailSend.message.IsBodyHtml ?
                                         MimeKit.Text.TextFormat.Html :
                                         MimeKit.Text.TextFormat.Text)
            {
                Text = mailSend.message.Body,
            };

            message.Subject = "Hello from c#!";

            await smtpClient.SendAsync(message,
                cancellationToken);
        }
    }
}
