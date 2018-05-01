using MarketDataProvider.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public abstract IEnumerable<Tick> GetMarketPrices(string market);
        public abstract decimal? GetPrice(string symbol);
        public abstract decimal? GetPrice(string market, string currency);
    }
}
