using MarketDataProvider.Enums;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MarketDataProvider.Services.PriceProviders
{
    public abstract class PriceProvider
    {
        protected HttpClient _client;

        public PriceProvider()
        {
            _client = new HttpClient();
        }

        public abstract Task UpdatePrices();
        public abstract decimal? GetRate(string symbol);
        public abstract decimal? GetRate(string market, string currency);
        public abstract IEnumerable<object> GetValues();
        public abstract IEnumerable<object> GetRates();
        public abstract Task<Models.OrderBook> GetOrderBook(string market);
        public abstract string GetUrl(OrderType orderType, string market, string currency, decimal amount);
    }
}
