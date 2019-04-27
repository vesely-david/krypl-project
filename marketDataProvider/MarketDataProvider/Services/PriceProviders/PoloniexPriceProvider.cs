using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Services.Interfaces;
using MarketDataProvider.Enums;
using MarketDataProvider.Services.Models;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MarketDataProvider.Services.PriceProviders
{
    public class PoloniexPriceProvider : PriceProvider
    {
        private string _exchangeName = "binance";


        private IMarketDataMemCacheService _marketDataMemCacheService;
        private Dictionary<string, decimal> _marketRates = new Dictionary<string, decimal>();
        //private ExchangeMemCache _exchangeInfo;

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
            var binancePrices = await _client.GetStringAsync("https://poloniex.com/public?command=returnTicker");

            var ticks  = JObject.Parse(binancePrices);
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


        public override string GetUrl(OrderType orderType, string market, string currency, decimal amount)
        {
            var url = QueryHelpers.AddQueryString(string.Empty, new Dictionary<string, string>
            {
                { "side", orderType.ToString() },
                { "amount", amount.ToString() },
                //......
            });
            return url;
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
                        return (currency: o.Id, (
                            btcValue: 1 / _marketRates[o.Id + "_BTC"],
                            usdValue: o.Id == "USDT" ? 1 : _marketRates["USDT_" + o.Id]
                        ));
                    }
                }).Select(o => new { o.currency, o.Item2.btcValue, o.Item2.usdValue });
            return result;

        }
    }
}
