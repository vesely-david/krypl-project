﻿using MasterDataManager.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MasterDataManager.Services
{
    public class MarketDataService : IMarketDataService
    {
        private HttpClient _client;
        private string _baseUrl = "https://www.marketdata.kryplproject.cz/"; //Use docker network instead

        public MarketDataService()
        {
            _client = new HttpClient();
        }
        public async Task<Dictionary<string, string>> GetCurrencyTranslationsAsync(string exchange)
        {
            try
            {
                var exchangeInfo = await _client.GetStringAsync(_baseUrl + "exchanges/" + exchange);
                var template = new { currencies = new[] { new { id = "", currencyExchangeId = "" } } };
                var currencies = JsonConvert.DeserializeAnonymousType(exchangeInfo, template).currencies;
                return currencies.ToDictionary(o => o.currencyExchangeId, o => o.id);

            } catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<Dictionary<string, ValueTuple<decimal, decimal>>> GetCurrentPrices(string exchange)
        {
            try
            {
                var exchangeInfo = await _client.GetStringAsync(_baseUrl + "exchanges/" + exchange + "/prices");
                var template = new { currencies = new[] { new { id = "", btcValue= 0, usdValue = 0 } } };
                var currencies = JsonConvert.DeserializeAnonymousType(exchangeInfo, template).currencies;
                return currencies.ToDictionary(o => o.id, o => new ValueTuple<decimal, decimal>(o.btcValue, o.usdValue));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
