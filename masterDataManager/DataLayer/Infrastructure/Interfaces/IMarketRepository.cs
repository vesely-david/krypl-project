using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Infrastructure.Interfaces
{
    public interface IMarketRepository : IRepository<Market>
    {
        Market GetMarketByCurrencies(int firstCurrencyId, int secondCurrencyId);
        Market GetByCode(string marketCode);
    }
}
