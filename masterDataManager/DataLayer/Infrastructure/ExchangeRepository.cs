using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
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

        public Exchange GetByName(string name)
        {
            return (name != null) ? _dbContext.Exchanges
                .FirstOrDefault(o => o.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) : null;
        }
    }
}
