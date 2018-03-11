using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Infrastructure
{
    public class ExchangeRepository : Repository<Exchange>, IExchangeRepository
    {
        public ExchangeRepository(MasterDataContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Exchange> GetAllWithCurrencies()
        {
            return _dbContext.Exchanges
                .Include(o => o.ExchangeCurrencies).ThenInclude(o => o.Currency);
        }

        public Exchange GetByExchangeId(int id)
        {
            return _dbContext.Exchanges
                .Include(o => o.ExchangeSecrets)
                .Include(o => o.ExchangeMarkets).ThenInclude(o => o.Market)
                .FirstOrDefault(o => o.Id == id);
        }


        public Exchange GetByName(string name)
        {
            if (name == null) return null;

            return _dbContext.Exchanges
                .Include(o => o.ExchangeSecrets)
                .Include(o => o.ExchangeMarkets).ThenInclude(o => o.Market)
                .FirstOrDefault(o => o.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

    }
}
