using DataLayer.Services.Interfaces;
using MarketDataProvider.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketDataProvider.Enums;
using Microsoft.AspNetCore.WebUtilities;

namespace MarketDataProvider.Services.PriceProviders
{
    public class BinancePriceProvider : PriceProvider
    {
        private string _exchangeName = "binance";


        private IMarketDataMemCacheService _marketDataMemCacheService;
        private Dictionary<string, decimal> _marketRates;
        //private ExchangeMemCache _exchangeInfo;

        public BinancePriceProvider(IMarketDataMemCacheService marketDataMemCacheService)
           : base() {
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
            var binancePrices = await _client.GetStringAsync("https://www.binance.com/api/v3/ticker/price");
            var ticks = JsonConvert.DeserializeObject<List<BinanceTick>>(binancePrices);
            var exchangeInfo = _marketDataMemCacheService.GetExchange(_exchangeName);
            var market = "";
            _marketRates = ticks.Where(o => exchangeInfo.TranslateMarket(o.symbol, out market))
                .ToDictionary(o => market, o => decimal.Parse(o.price));
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

        public override IEnumerable<object> GetValues()
        {
            var exchangeInfo = _marketDataMemCacheService.GetExchange(_exchangeName);

            var result = exchangeInfo.Currencies
                .Select(o => {
                    if(o.Id == "BTC")
                    {
                        return (currency: "BTC", (btcValue: 1m, usdValue: _marketRates["USDT_BTC"]));
                    }
                    else if(_marketRates.ContainsKey("BTC_" + o.Id)) //weaker coins
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
                            btcValue: 1/_marketRates[o.Id + "_BTC"],
                            usdValue: o.Id == "USDT" ? 1 : _marketRates["USDT_" + o.Id]
                        ));
                    }
                }).Select(o => new { o.currency, o.Item2.btcValue, o.Item2.usdValue});
            return result;

        }
    }
}
