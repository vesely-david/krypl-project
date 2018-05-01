using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Repositories.Interfaces
{
    public interface IMarketRepository : IRepository<Market>
    {
        Market GetMarketByCurrencies(string marketCurrencyId, string currencyId);
    }
}
