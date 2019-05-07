using System;
using System.Collections.Generic;
using DataLayer.Services.Interfaces;
using MarketDataProvider.Services.HistoryPriceProviders;
using Microsoft.Extensions.Caching.Memory;

namespace MarketDataProvider.Services
{
    public class HistoryPriceService
    {
        private Dictionary<string, HistoryPriceProvider> _marketDataDict;

        public HistoryPriceService(
            IMarketDataMemCacheService marketDataMemCacheService,
            IMemoryCache memoryCache)
        {
            _marketDataDict = new Dictionary<string, HistoryPriceProvider>
            {
                { "poloniex", new PoloniexHistoryPriceProvider(marketDataMemCacheService, memoryCache) },
            };
        }

        public HistoryPriceProvider GetExchange(string exchange)
        {
            if (_marketDataDict.ContainsKey(exchange))
            {
                return _marketDataDict[exchange];
            }
            return null;
        }
    }
}
