using DataLayer.Repositories.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Repositories
{
    public class ExchangeCurrencyRepository : Repository<ExchangeCurrency>, IExchangeCurrencyRepository
    {
        public ExchangeCurrencyRepository(MarketDataContext dbContext) : base(dbContext)
        {
        }
    }
}
