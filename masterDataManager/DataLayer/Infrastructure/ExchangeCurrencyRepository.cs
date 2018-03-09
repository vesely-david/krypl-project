using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Infrastructure
{
    public class ExchangeCurrencyRepository : Repository<ExchangeCurrency>, IExchangeCurrencyRepository
    {
        public ExchangeCurrencyRepository(MasterDataContext dbContext) : base(dbContext)
        {
        }
    }
}
