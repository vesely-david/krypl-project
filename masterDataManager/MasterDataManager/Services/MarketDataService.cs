﻿using DataLayer.Models;
using MasterDataManager.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private string _baseUrl = "http://marketdataprovider/"; //Use docker network instead
        //private string _baseUrl = "https://marketdata.jankirchner.cz/"; //Use docker network instead
         //private string _baseUrl = "http://localhost:9999/"; //Use docker network instead

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
                return new Dictionary<string, string>();
            }

        }

        public async Task<Dictionary<string, string>> GetMarketTranslationsAsync(string exchange)
        {
            try
            {
                var exchangeInfo = await _client.GetStringAsync(_baseUrl + "exchanges/" + exchange);
                var template = new { markets = new[] { new { id = "", marketExchangeId = "" } } };
                var markets = JsonConvert.DeserializeAnonymousType(exchangeInfo, template).markets;
                return markets.ToDictionary(o => o.id, o => o.marketExchangeId);

            }
            catch (Exception ex)
            {
                return new Dictionary<string, string>();
            }
        }

        public async Task<Dictionary<string, (decimal BtcValue, decimal UsdValue )>> GetCurrentPrices(string exchange)
        {
            try
            {
                var exchangeInfo = await _client.GetStringAsync(_baseUrl + "business/price/" + exchange);
                var template = new[] { new { currency = "", btcValue= 0m, usdValue = 0m } };
                var currencies = JsonConvert.DeserializeAnonymousType(exchangeInfo, template);
                return currencies.ToDictionary(o => o.currency, o => (BtcValue: o.btcValue, UsdValue: o.usdValue));
            }
            catch (Exception ex)
            {
                //TODO: Logger
                return new Dictionary<string, (decimal BtcValue, decimal UsdValue)>();
            }
        }

        public async Task<Dictionary<string, decimal>> GetCurrentRates(string exchange)
        {
            try
            {
                var exchangeInfo = await _client.GetStringAsync(_baseUrl + "business/rate/" + exchange);
                var template = new[] { new { symbol = "", rate = 0m } };
                var currencies = JsonConvert.DeserializeAnonymousType(exchangeInfo, template);
                return currencies.ToDictionary(o => o.symbol, o => o.rate);
            }
            catch (Exception ex)
            {
                //TODO: Logger
                return new Dictionary<string, decimal>();
            }
        }

        public async Task<decimal?> GetHistoryRate(string exchange, string market, long timestamp)
        {
            try
            {
                var rate = await _client.GetStringAsync(_baseUrl + "business/rate/" + exchange + "/" + market + "/" + timestamp.ToString());


                return decimal.Parse(rate);
            }
            catch (Exception ex)
            {
                //TODO: Logger
                return null;
            }
        }

        public async Task<EvaluationTick> EvaluateAssetSet(IEnumerable<(string currency, decimal amount)> assets, string exchange)
        {
            var result = new EvaluationTick
            {
                TimeStamp = DateTime.Now,
                BtcValue = 0,
                UsdValue = 0
            };
            var prices = await GetCurrentPrices(exchange);
            return assets.Aggregate(result, (res, val) =>
            {
                if (prices.ContainsKey(val.currency))
                {
                    res.BtcValue += prices[val.currency].BtcValue * val.amount;
                    res.UsdValue += prices[val.currency].UsdValue * val.amount;
                }
                return res;
            });
        }
    }
}
