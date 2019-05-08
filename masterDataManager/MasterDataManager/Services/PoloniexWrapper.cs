
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MasterDataManager.Services.ServiceModels;
using MasterDataManager.Utils;

namespace MasterDataManager.Services
{
    public class PoloniexWrapper
    {
        private HttpClient _client;
        private readonly string _baseEndpoint = "https://poloniex.com/tradingApi";

        public PoloniexWrapper()
        {
            _client = new HttpClient();
        }

        public async Task<List<PoloniexBalance>> GetBalances(string apiKey, string apiSecret)
        {;
            var response = await _client.PoloniexSignedRequest<Dictionary<string, decimal>>(_baseEndpoint, HttpMethod.Post, new Dictionary<string, string> { {"command", "returnBalances" } }, apiKey, apiSecret);
            return response.Where(o => o.Value > 0m).Select(o => new PoloniexBalance
            {
                amount = o.Value,
                asset = o.Key,
            }).ToList();
        }
    }
}
