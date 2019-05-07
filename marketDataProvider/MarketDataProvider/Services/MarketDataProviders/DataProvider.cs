using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MarketDataProvider.Services.MarketDataProviders
{
    public abstract class DataProvider
    {
        protected HttpClient _client;

        protected DataProvider()
        {
            _client = new HttpClient();
        }

        public abstract Task UpdateData();
        public abstract IEnumerable<object> GetData();
    }
}
