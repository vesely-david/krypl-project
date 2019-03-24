using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Infrastructure.Interfaces
{
    public interface IExchangeSecretRepository : IRepository<ExchangeSecret>
    {
        ExchangeSecret GetByUserAndExchange(string userId, string exchangeId);
        IEnumerable<ExchangeSecret> GetByUser(string userId);
    }
}
