using DataLayer.Models;
using DataLayer.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Services.Interfaces
{
    public interface IMarketDataMemCacheService
    {
        IEnumerable<Exchange> ExchangeList();
        IEnumerable<CurrencyMemCache> ExchangeCurrencies(string strategyId);
        IEnumerable<MarketMemCache> ExchangeMarkets(string strategyId);
        ExchangeMemCache GetExchange(string exchangeId);
    }
}
