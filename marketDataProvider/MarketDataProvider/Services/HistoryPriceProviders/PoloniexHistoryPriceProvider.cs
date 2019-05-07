using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Services.Interfaces;
using MarketDataProvider.Services.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace MarketDataProvider.Services.HistoryPriceProviders
{
    public class PoloniexHistoryPriceProvider : HistoryPriceProvider
    {
        private string _exchangeName = "poloniex";
        private string historyUrl = "https://poloniex.com/public?command=returnChartData&currencyPair={0}&start={1}&period={2}&resolution=auto";
        private int basePeriod = 300;

        private IMarketDataMemCacheService _marketDataMemCacheService;
        private IMemoryCache _historyDataMemCache;
        //private ExchangeMemCache _exchangeInfo;

        public PoloniexHistoryPriceProvider(IMarketDataMemCacheService marketDataMemCacheService, IMemoryCache memCache)
           : base()
        {
            _marketDataMemCacheService = marketDataMemCacheService;
            _historyDataMemCache = memCache;
            //_exchangeInfo = marketDataMemCacheService.GetExchange(_exchangeName);
        }

        private async Task<List<PoloniexCandle>> GetHistoryData(string market, long timestamp)
        {
            var url = String.Format(historyUrl, market, timestamp, basePeriod);
            var rawCandles = await _client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<List<PoloniexCandle>>(rawCandles);
        }

        public override async Task<decimal?> GetHistoryRate(string market, string currency, long timestamp)
        {
            var marketId = market + "_" + currency;
            var id = _exchangeName + "_" + marketId + "_5";
            List<PoloniexCandle> coinData;
            var result = _historyDataMemCache.TryGetValue(id, out coinData);

            if(!result || coinData.First().date > timestamp || coinData.Last().date + basePeriod < timestamp)
            {
                coinData = await GetHistoryData(marketId, timestamp);
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromDays(1));

                _historyDataMemCache.Set(id, coinData, cacheEntryOptions);
            }

            var candle = coinData.FirstOrDefault(o => timestamp >= o.date);
            if (candle == null) return null;
            return GetWeightedRate(candle, basePeriod, timestamp);
        }


        public decimal GetWeightedRate(PoloniexCandle candle, int period, long timestamp)
        {
            var rate = (timestamp - candle.date) / period;
            return candle.low + (candle.high - candle.low) * rate;
        }
    }
}


//