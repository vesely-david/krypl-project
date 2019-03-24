using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MasterDataManager.Services.Interfaces;

namespace MasterDataManager.Services
{
    public class MailingService : IMailingService
    {
        private SmtpClient client;
        private readonly string fromUsername = "kryplproject@seznam.cz";

        public MailingService()
        {
            client = new SmtpClient("smtp.seznam.cz", 25)
            {
                UseDefaultCredentials = false,
                //DeliveryMethod = SmtpDeliveryMethod.Network,
                //EnableSsl = true,
                Credentials = new NetworkCredential(fromUsername, "KryplPr0ject!")
            };
        }

        public async Task SendEmailVerification(string to, string userToken)
        {
            var mail = new MailMessage(fromUsername, to, "Krypl project email verification", "");
            await client.SendMailAsync(mail);
        }
    }
}
