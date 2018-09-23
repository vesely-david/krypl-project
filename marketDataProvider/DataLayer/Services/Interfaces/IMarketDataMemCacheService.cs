using DataLayer.Models;
using DataLayer.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Services.Interfaces
{
    public interface IMarketDataMemCacheService
    {
        IEnumerable<ExchangeMemCache> ExchangeList();
        ExchangeMemCache GetExchange(string exchangeId);
    }
}
