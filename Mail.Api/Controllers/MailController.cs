using Mail.Api.Core.Mail.DTO.MailGet;
using Mail.Api.Core.Mail.DTO.MailSend;
using Mail.Api.Core.Mail.Interfaces;
using Mail.Api.Core.Mail.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mail.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMail([FromBody] MailGetModel mailGet, CancellationToken cancellationToken)
        {
            var mailbox = new Mailbox
            {
                ImapCredentials = new MailCredentials
                {
                    Host = mailGet.mailbox.ImapCredentials.Host,
                    Port = mailGet.mailbox.ImapCredentials.Port
                },
                SmtpCredentials = new MailCredentials
                {
                    Host = mailGet.mailbox.SmtpCredentials.Host,
                    Port = mailGet.mailbox.SmtpCredentials.Port
                },
                Login = mailGet.mailbox.Login,
                Password = mailGet.mailbox.Password,
                UseSsl = mailGet.mailbox.UseSSL,
            };

            var mail = await _mailService.GetMailAsync(mailbox, TimeSpan.FromDays(mailGet.interval), cancellationToken);

            var response = mail.ToList()
                .Select(email => new
                {
                    From = email.From.Select(i => i.Name),
                    Subject = email.Subject,
                    Body = email.TextBody,
                    Attachments = email.Attachments.Count(),
                    Date = email.Date
                });

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SendMail([FromBody] MailSendModel mailSend, CancellationToken cancellationToken)
        {
            await _mailService.SendMailAsyc(mailSend, cancellationToken);

            return Ok();
        }
    }
}
