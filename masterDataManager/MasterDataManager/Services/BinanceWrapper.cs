using MasterDataManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using System.Net.Http;
using Newtonsoft.Json;
using MasterDataManager.Services.ServiceModels;

namespace MasterDataManager.Services
{
    public class BinanceWrapper : IHistoryExchangeService, IExchangeService
    {
        private HttpClient _client;

        public BinanceWrapper()
        {
            _client = new HttpClient();
        }

        public async Task<List<UserAsset> > GetBalances()
        {
            var url = @"/api/v3/account";

            var rawResponse = await _client.GetStringAsync(url);

            var response = JsonConvert.DeserializeObject<BinanceAccountInfo>(rawResponse);
            throw new NotImplementedException();
        }

        public int GetHistoryPrice(Currency currency, DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}
