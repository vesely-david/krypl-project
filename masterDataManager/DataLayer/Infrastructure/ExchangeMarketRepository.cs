using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Infrastructure
{
    public class ExchangeMarketRepository : Repository<ExchangeMarket>, IExchangeMarketRepository
    {
        public ExchangeMarketRepository(MasterDataContext dbContext) : base(dbContext)
        {
        }
    }
}
