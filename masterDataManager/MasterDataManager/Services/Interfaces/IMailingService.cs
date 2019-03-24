using System;
using System.Threading.Tasks;

namespace MasterDataManager.Services.Interfaces
{
    public interface IMailingService
    {
        Task SendEmailVerification(string to, string userToken);
    }
}
