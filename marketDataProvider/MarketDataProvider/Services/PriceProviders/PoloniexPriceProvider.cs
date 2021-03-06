﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Services.Interfaces;
using MarketDataProvider.Enums;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MarketDataProvider.Services.PriceProviders
{
    public class PoloniexPriceProvider : PriceProvider
    {
        private string _exchangeName = "poloniex";


        private IMarketDataMemCacheService _marketDataMemCacheService;
        private Dictionary<string, decimal> _marketRates = new Dictionary<string, decimal>();

        public PoloniexPriceProvider(IMarketDataMemCacheService marketDataMemCacheService)
           : base()
        {
            _marketDataMemCacheService = marketDataMemCacheService;
            //_exchangeInfo = marketDataMemCacheService.GetExchange(_exchangeName);
        }


        public override decimal? GetRate(string symbol)
        {
            if (_marketRates.ContainsKey(symbol)) return _marketRates[symbol];
            return null;
        }

        public override decimal? GetRate(string market, string currency)
        {
            var symbol = market + "_" + currency;
            return GetRate(symbol);
        }


        public override async Task UpdatePrices()
        {
            var poloniexPrices = await _client.GetStringAsync("https://poloniex.com/public?command=returnTicker");

            var ticks  = JObject.Parse(poloniexPrices);
            foreach(var pair in ticks)
            {
                try
                {
                    var ask = Decimal.Parse(pair.Value.SelectToken("lowestAsk").ToString());
                    var bid = Decimal.Parse(pair.Value.SelectToken("highestBid").ToString());
                    if (!_marketRates.ContainsKey(pair.Key)) _marketRates.Add(pair.Key, (ask + bid) / 2);
                    else _marketRates[pair.Key] = (ask + bid) / 2;
                } catch { }
            }
        }


        public override IEnumerable<object> GetRates()
        {
            return _marketRates.Select(o => new { symbol = o.Key, rate = o.Value });
        }

        public override IEnumerable<object> GetValues()
        {
            var exchangeInfo = _marketDataMemCacheService.GetExchange(_exchangeName);

            var result = exchangeInfo.Currencies
                .Select(o => {
                    if (o.Id == "BTC")
                    {
                        return (currency: "BTC", (btcValue: 1m, usdValue: _marketRates["USDT_BTC"]));
                    }
                    else if (_marketRates.ContainsKey("BTC_" + o.Id)) //weaker coins
                    {
                        if (_marketRates.ContainsKey("USDT_" + o.Id))
                        {
                            return (currency: o.Id, (
                                btcValue: _marketRates["BTC_" + o.Id],
                                usdValue: _marketRates["USDT_" + o.Id]
                            ));
                        }
                        else
                        {
                            return (currency: o.Id, (
                                btcValue: _marketRates["BTC_" + o.Id],
                                usdValue: _marketRates["USDT_BTC"] * _marketRates["BTC_" + o.Id]
                            ));
                        }
                    }
                    else //stronger coins
                    {
                        var x = _marketRates.ContainsKey(o.Id + "_BTC");
                        var y = _marketRates.ContainsKey("USDT_" + o.Id);
                        return (currency: o.Id, (
                            btcValue: 1 / _marketRates[o.Id + "_BTC"],
                            usdValue: o.Id == "USDT" ? 1 : o.Id == "USDC" ? 1 / _marketRates["USDC_USDT"] : _marketRates["USDT_" + o.Id]
                        ));
                    }
                }).Select(o => new { o.currency, o.Item2.btcValue, o.Item2.usdValue });
            return result;
        }

        public override async Task<Models.OrderBook> GetOrderBook(string market)
        {
            var url = "https://poloniex.com/public?command=returnOrderBook&currencyPair={0}";

            var rawOrderBook = await _client.GetStringAsync(String.Format(url, market));
            //var test2 = JsonConvert.DeserializeObject<List<BinanceTick>>(binancePrices);
            var orderBook = JObject.Parse(rawOrderBook);
            var asks = orderBook.SelectToken("asks").ToObject(typeof(List<List<decimal>>));
            var bids = orderBook.SelectToken("bids").ToObject(typeof(List<List<decimal>>));

            return new Models.OrderBook
            {
                Asks = ((List<List<decimal>>)asks).Select(o => new Models.OrderBookTuple(o.ElementAt(0), o.ElementAt(1))).ToList(),
                Bids = ((List<List<decimal>>)bids).Select(o => new Models.OrderBookTuple(o.ElementAt(0), o.ElementAt(1))).ToList()
            };
        }
    }
}
