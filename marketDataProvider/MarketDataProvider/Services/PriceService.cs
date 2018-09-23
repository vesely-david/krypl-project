using DataLayer.Services.Interfaces;
using MarketDataProvider.Services.PriceProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MarketDataProvider.Services
{
    public class PriceService {

        private Dictionary<string, PriceProvider> _marketDataDict;

        public PriceService(IMarketDataMemCacheService marketDataMemCacheService)
        {
            _marketDataDict = new Dictionary<string, PriceProvider>
            {
                { "binance", new BinancePriceProvider(marketDataMemCacheService) }
            };
        }

        public PriceProvider GetExchange(string exchange)
        {
            if (_marketDataDict.ContainsKey(exchange))
            {
                return _marketDataDict[exchange];
            }
            return null;
        }

        public async Task UpdatePrices(CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            foreach(var exchange in _marketDataDict.Keys)
            {
                tasks.Add(_marketDataDict[exchange].UpdatePrices());
            }
            await Task.WhenAll(tasks);
            return;
        }
    }
}
