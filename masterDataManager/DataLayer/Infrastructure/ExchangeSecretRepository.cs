using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Infrastructure
{
    public class ExchangeSecretRepository : Repository<ExchangeSecret>, IExchangeSecretRepository
    {
        public ExchangeSecretRepository(MasterDataContext dbContext) : base(dbContext)
        {
        }

        public ExchangeSecret GetByUserAndExchange(string userId, string exchange)
        {
            return _dbContext.ExchangeSecrets
                .FirstOrDefault(o => o.ExchangeId == exchange && o.UserId == userId);
        }
    }
}
