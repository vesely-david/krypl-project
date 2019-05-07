using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MarketDataProvider.Services.HistoryPriceProviders
{
    public abstract class HistoryPriceProvider
    {
        protected HttpClient _client;

        public HistoryPriceProvider()
        {
            _client = new HttpClient();
        }

        public abstract Task<decimal?> GetHistoryRate(string market, string currency, long timestamp);
    }
}
