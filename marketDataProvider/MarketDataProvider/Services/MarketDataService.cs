using DataLayer.Services.Interfaces;
using MarketDataProvider.Services.MarketDataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MarketDataProvider.Services
{
    public class MarketDataService
    {
        private Dictionary<string, DataProvider> _marketDataDict;

        public MarketDataService()
        {
            _marketDataDict = new Dictionary<string, DataProvider>
            {
                { "marketCal", new MarketCal() },
            };
        }

        public DataProvider GetProvider(string exchange)
        {
            if (_marketDataDict.ContainsKey(exchange))
            {
                return _marketDataDict[exchange];
            }
            return null;
        }

        public async Task UpdateInfo(CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            foreach (var exchange in _marketDataDict.Keys)
            {
                tasks.Add(_marketDataDict[exchange].UpdateData());
            }
            await Task.WhenAll(tasks);
            return;
        }
    }
}
