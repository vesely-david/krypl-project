using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Infrastructure
{
    public class CurrencyRepository : Repository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(MasterDataContext dbContext) : base(dbContext)
        {
        }

        public Currency GetByCode(string code)
        {
            return _dbContext.Currencies
                .FirstOrDefault(o => o.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
        }
    }
}
