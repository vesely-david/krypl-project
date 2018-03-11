﻿using MasterDataManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using System.Net.Http;
using Newtonsoft.Json;
using MasterDataManager.Services.ServiceModels;
using MasterDataManager.Utils;

namespace MasterDataManager.Services
{
    public class BinanceWrapper
    {
        private HttpClient _client;

        public BinanceWrapper()
        {
            _client = new HttpClient();
        }

        public async Task<List<BinanceAsset> > GetBalances(string apiKey, string apiSecret)
        {
            var url = @"https://www.binance.com/api//v3/account";
            var response = await _client.BinanceSignedRequest<BinanceAccountInfo>(url, HttpMethod.Get, null, apiKey, apiSecret);
            return response.balances.ToList();
        }

        public int GetHistoryPrice(Currency currency, DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}
