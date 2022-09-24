using System.Net;
using System.Net.Mail;

namespace Projeto02.Services.Api.Helpers
{
    public class EmailHelper
    {
        private readonly string? _conta;
        private readonly string? _senha;
        private readonly string? _smtp;
        private readonly int _porta;

        public EmailHelper(string? conta, string? senha, string? smtp, int porta)
        {
            _conta = conta;
            _senha = senha;
            _smtp = smtp;
            _porta = porta;
        }

        public void Send(string email, string assunto, string texto)
        {
            var mailMessage = new MailMessage(_conta, email);
            mailMessage.Subject = assunto;
            mailMessage.Body = texto;
            mailMessage.IsBodyHtml = true;

            var smtpClient = new SmtpClient(_smtp, _porta);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_conta, _senha);
            smtpClient.Send(mailMessage);
        }
    }
}
